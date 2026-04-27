using System;
using System.Windows;
using System.Windows.Input;

namespace ChaiIdle
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// Da, main window ba! Hidden mostly, shown for settings
    /// </summary>
    public partial class MainWindow : Window
    {
        private IdleDetector? _idleDetector;
        private SettingsService? _settingsService;
        private TrayService? _trayService;
        private OverlayWindow? _overlayWindow;

        public MainWindow()
        {
            InitializeComponent();
            
            // Da, initialization ba!
            _settingsService = new SettingsService();
            _trayService = new TrayService();
            _idleDetector = new IdleDetector();

            Setup();

            // Subscribe to logs
            Logger.LogAdded += (s, msg) => {
                Dispatcher.Invoke(() => {
                    LogText.AppendText(msg + Environment.NewLine);
                    LogScrollViewer.ScrollToEnd();
                });
            };
            Logger.Log("App Started - Oru tea sollu ba!");

            // Da, delay hide ba! Let them see the diagnostics first for 5s
            var hideTimer = new System.Windows.Threading.DispatcherTimer();
            hideTimer.Interval = TimeSpan.FromSeconds(5);
            hideTimer.Tick += (s, e) => { hideTimer.Stop(); HideToTray(); };
            hideTimer.Start();
        }

        private void Setup()
        {
            try
            {
                // Tray menu events
                _trayService!.SettingsClicked += (s, e) => ShowSettings();
                _trayService.PauseClicked += (s, e) => _idleDetector!.IsEnabled = false;
                _trayService.ResumeClicked += (s, e) => _idleDetector!.IsEnabled = true;
                _trayService.ExitClicked += (s, e) => Application.Current.Shutdown();
                _trayService.AboutClicked += (s, e) => ShowAbout();

                _trayService.Initialize();

                // Idle detection events
                _idleDetector!.IdleStateChanged += (s, e) =>
                {
                    if (e.IsIdle && !_settingsService!.GetSettings().IsPaused)
                    {
                        ShowOverlay();
                    }
                    else if (!e.IsIdle && _overlayWindow?.IsVisible == true)
                    {
                        HideOverlay();
                    }
                };

                _idleDetector.IdleThresholdMs = _settingsService!.GetIdleThresholdMs();
                _idleDetector.Start();

                // Initialize UI from settings
                var settings = _settingsService.GetSettings();
                IdleSlider.Value = settings.IdleMinutes;
                IdleMinutesLabel.Text = settings.IdleMinutes.ToString();

                // Da, diagnostic timer ba! Update labels every 1s
                var diagTimer = new System.Windows.Threading.DispatcherTimer();
                diagTimer.Interval = TimeSpan.FromSeconds(1);
                diagTimer.Tick += (s, e) =>
                {
                    StatusText.Text = $"Status: {(_idleDetector.IsEnabled ? "Watching" : "Paused")}";
                    DiagnosticText.Text = $"Idle: {_idleDetector.CurrentIdleTimeMs / 1000}s / Threshold: {_idleDetector.IdleThresholdMs / 1000}s";
                };
                diagTimer.Start();

                diagTimer.Start();

                Logger.Success("Setup complete - Chai potta da!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ChaiIdle Error] Setup failed: {ex.Message}");
            }
        }

        private void ShowOverlay()
        {
            try
            {
                // Create new window if it doesn't exist (it gets set to null when closed)
                if (_overlayWindow == null)
                {
                    _overlayWindow = new OverlayWindow(_settingsService!);
                    _overlayWindow.UserActivityDetected += (s, e) =>
                    {
                        HideOverlay();
                        _idleDetector?.ResetIdleTimer();
                    };
                    
                    // Set to null when closed so we know to recreate it next time
                    _overlayWindow.Closed += (s, e) => _overlayWindow = null;
                }

                _overlayWindow.Show();
                _overlayWindow.Activate(); // Bring to front
                Logger.Log("Overlay shown - Chai time ba!");
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to show overlay: {ex.Message}");
                _overlayWindow = null; // Reset on failure
            }
        }

        private void HideOverlay()
        {
            try
            {
                if (_overlayWindow?.IsVisible == true)
                {
                    _overlayWindow.Hide();
                    Console.WriteLine("[ChaiIdle] Overlay hidden - Back to work!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ChaiIdle Error] Failed to hide overlay: {ex.Message}");
            }
        }

        private void ShowSettings()
        {
            try
            {
                this.WindowState = WindowState.Normal;
                this.Show();
                this.Activate();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ChaiIdle Error] Failed to show settings: {ex.Message}");
            }
        }

        private void ShowAbout()
        {
            MessageBox.Show(
                "ChaiIdle (OruTeaDa) v1.0\n\n" +
                "A fun chai break reminder for Indian IT engineers.\n\n" +
                "Idle for 5 minutes and get a viral chai moment!\n\n" +
                "Made with ❤️ and chai for Tamil developers.\n\n" +
                "Share your chai break: Right-click overlay > Share Chai Moment",
                "About OruTeaDa",
                MessageBoxButton.OK,
                MessageBoxImage.Information
            );
        }

        private void HideToTray()
        {
            this.Hide();
            this.WindowState = WindowState.Minimized;
        }

        private void MinimizeToTray(object sender, RoutedEventArgs e)
        {
            HideToTray();
        }

        private void SaveAndMinimize(object sender, RoutedEventArgs e)
        {
            try
            {
                var settings = _settingsService!.GetSettings();
                settings.IdleMinutes = (int)IdleSlider.Value;
                _settingsService.UpdateSettings(settings);

                _idleDetector!.IdleThresholdMs = _settingsService.GetIdleThresholdMs();
                
                HideToTray();
                _trayService?.ShowNotification("Settings Saved", $"Idle break set to {settings.IdleMinutes} minutes.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save: {ex.Message}");
            }
        }

        private void IdleSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (IdleMinutesLabel != null)
            {
                IdleMinutesLabel.Text = ((int)e.NewValue).ToString();
            }
        }

        private void TestOverlay_Click(object sender, RoutedEventArgs e)
        {
            ShowOverlay();
        }

        protected override void OnStateChanged(EventArgs e)
        {
            base.OnStateChanged(e);
            
            if (WindowState == WindowState.Minimized)
            {
                HideToTray();
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            
            _idleDetector?.Stop();
            _trayService?.Dispose();
            _overlayWindow?.Close();
            
            Console.WriteLine("[ChaiIdle] App closed - See you next chai break!");
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }
    }
}
