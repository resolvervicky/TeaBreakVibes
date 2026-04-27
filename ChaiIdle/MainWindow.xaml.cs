using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Win32;
using System.Threading.Tasks;

namespace ChaiIdle
{
    public partial class MainWindow : Window
    {
        private IdleDetector?      _idleDetector;
        private SettingsService?   _settingsService;
        private TrayService?       _trayService;
        private OverlayWindow?     _overlayWindow;

        public MainWindow()
        {
            InitializeComponent();

            _settingsService = new SettingsService();
            _trayService     = new TrayService();
            _idleDetector    = new IdleDetector();

            Setup();

            Logger.Log("App started — Oru tea sollu ba!");

            // Auto-hide after 6 s so user can see it loaded
            var hideTimer = new System.Windows.Threading.DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(6)
            };
            hideTimer.Tick += (_, _) => { hideTimer.Stop(); HideToTray(); };
            hideTimer.Start();
        }

        // ─────────────────────────────────────────────────────────────────
        // Setup
        // ─────────────────────────────────────────────────────────────────
        private void Setup()
        {
            try
            {
                _trayService!.SettingsClicked += (_, _) => ShowSettings();
                _trayService.PauseClicked     += (_, _) => _idleDetector!.IsEnabled = false;
                _trayService.ResumeClicked    += (_, _) => _idleDetector!.IsEnabled = true;
                _trayService.ExitClicked      += (_, _) => Application.Current.Shutdown();
                _trayService.AboutClicked     += (_, _) => ShowAbout();
                _trayService.Initialize();

                _idleDetector!.IdleStateChanged += (_, e) =>
                {
                    if (e.IsIdle && !_settingsService!.GetSettings().IsPaused)
                        ShowOverlay();
                    else if (!e.IsIdle && _overlayWindow?.IsVisible == true)
                        HideOverlay();
                };

                _idleDetector.IdleThresholdMs = _settingsService!.GetIdleThresholdMs();
                _idleDetector.Start();

                // Populate UI
                var s = _settingsService.GetSettings();
                IdleSlider.Value      = s.IdleMinutes;
                IdleMinutesLabel.Text = $"{s.IdleMinutes} min";

                foreach (ComboBoxItem item in LanguageCombo.Items)
                {
                    if (item.Content.ToString() == s.PreferredLanguage)
                    {
                        LanguageCombo.SelectedItem = item;
                        break;
                    }
                }

                // Diagnostic timer — status pill only
                var diagTimer = new System.Windows.Threading.DispatcherTimer
                {
                    Interval = TimeSpan.FromSeconds(1)
                };
                diagTimer.Tick += (_, _) =>
                {
                    StatusText.Text = _idleDetector.IsEnabled
                        ? "Watching for idle..."
                        : "⏸  Paused";
                };
                diagTimer.Start();

                Logger.Success("Setup complete — Chai potta da!");
            }
            catch (Exception ex)
            {
                Logger.Error($"Setup failed: {ex.Message}");
            }
        }

        // ─────────────────────────────────────────────────────────────────
        // Overlay management
        // ─────────────────────────────────────────────────────────────────
        private void ShowOverlay()
        {
            try
            {
                if (_overlayWindow == null)
                {
                    _overlayWindow = new OverlayWindow(_settingsService!);
                    _overlayWindow.UserActivityDetected += (_, _) =>
                    {
                        HideOverlay();
                        _idleDetector?.ResetIdleTimer();
                    };
                    _overlayWindow.Closed += (_, _) => _overlayWindow = null;
                }

                _overlayWindow.Show();
                _overlayWindow.Activate();
                Logger.Log("Overlay shown — Chai time ba!");
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to show overlay: {ex.Message}");
                _overlayWindow = null;
            }
        }

        private void HideOverlay()
        {
            try
            {
                if (_overlayWindow?.IsVisible == true)
                    _overlayWindow.Hide();
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to hide overlay: {ex.Message}");
            }
        }

        // ─────────────────────────────────────────────────────────────────
        // Settings window
        // ─────────────────────────────────────────────────────────────────
        private void ShowSettings()
        {
            WindowState = WindowState.Normal;
            Show();
            Activate();
        }

        private void ShowAbout()
        {
            MessageBox.Show(
                "OruTeaDa — ChaiIdle v1.0\n\n" +
                "A chai break reminder for developers.\n" +
                "Idle for a few minutes and get a meme-worthy break!\n\n" +
                "Made with ❤️ and chai for Indian developers.",
                "About OruTeaDa", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void HideToTray()
        {
            Hide();
            WindowState = WindowState.Minimized;
        }

        // ─────────────────────────────────────────────────────────────────
        // UI event handlers
        // ─────────────────────────────────────────────────────────────────
        private void IdleSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (IdleMinutesLabel != null)
                IdleMinutesLabel.Text = $"{(int)e.NewValue} min";
        }

        private void SaveAndMinimize(object sender, RoutedEventArgs e)
        {
            try
            {
                var s = _settingsService!.GetSettings();
                s.IdleMinutes       = (int)IdleSlider.Value;
                s.PreferredLanguage = (LanguageCombo.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "Tamil";
                _settingsService.UpdateSettings(s);
                _idleDetector!.IdleThresholdMs = _settingsService.GetIdleThresholdMs();
                HideToTray();
                _trayService?.ShowNotification("Settings Saved", $"Idle break set to {s.IdleMinutes} minutes.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save: {ex.Message}");
            }
        }

        private async void UploadAudio_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var ofd = new OpenFileDialog
                {
                    Filter = "Audio files (*.mp3)|*.mp3",
                    Title  = "Select a short chai reminder (MP3)"
                };

                if (ofd.ShowDialog() != true) return;

                string filePath = ofd.FileName;
                var info = new FileInfo(filePath);

                // 1. Validate File Size (Max 1MB)
                if (info.Length > 1024 * 1024)
                {
                    AudioStatus.Text = "❌ Too large (>1MB)";
                    AudioStatus.Foreground = Brushes.Tomato;
                    return;
                }

                // 2. Validate Duration (Max 15s)
                AudioStatus.Text = "⌛ Checking length...";
                AudioStatus.Foreground = Brushes.SkyBlue;

                double durationSec = await GetAudioDuration(filePath);
                if (durationSec > 15 || durationSec <= 0)
                {
                    AudioStatus.Text = $"❌ Too long ({durationSec:F1}s)";
                    AudioStatus.Foreground = Brushes.Tomato;
                    return;
                }

                // 3. Copy to Assets
                string lang = (LanguageCombo.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "Tamil";
                string destDir = Path.Combine(AppContext.BaseDirectory, "Assets", "Audio", lang);
                
                if (!Directory.Exists(destDir)) Directory.CreateDirectory(destDir);

                string destFile = Path.Combine(destDir, $"custom_{DateTime.Now:yyyyMMdd_HHmmss}.mp3");
                File.Copy(filePath, destFile, true);

                AudioStatus.Text = $"✅ Uploaded ({durationSec:F1}s)";
                AudioStatus.Foreground = Brushes.LightGreen;
                Logger.Log($"Custom audio added to {lang}: {Path.GetFileName(destFile)}");
            }
            catch (Exception ex)
            {
                AudioStatus.Text = "❌ Upload failed";
                AudioStatus.Foreground = Brushes.Tomato;
                Logger.Error($"Upload error: {ex.Message}");
            }
        }

        private Task<double> GetAudioDuration(string path)
        {
            var tcs = new TaskCompletionSource<double>();
            var player = new MediaPlayer();

            player.MediaOpened += (s, e) =>
            {
                double sec = player.NaturalDuration.HasTimeSpan ? player.NaturalDuration.TimeSpan.TotalSeconds : 0;
                player.Close();
                tcs.SetResult(sec);
            };

            player.MediaFailed += (s, e) =>
            {
                player.Close();
                tcs.SetResult(-1);
            };

            player.Open(new Uri(path));
            return tcs.Task;
        }

        private void MinimizeToTray(object sender, RoutedEventArgs e) => HideToTray();

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                try { DragMove(); } catch { }
        }

        protected override void OnStateChanged(EventArgs e)
        {
            base.OnStateChanged(e);
            if (WindowState == WindowState.Minimized) HideToTray();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _idleDetector?.Stop();
            _trayService?.Dispose();
            _overlayWindow?.Close();
        }
    }
}
