using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace ChaiIdle
{
    public partial class OverlayWindow : Window
    {
        private readonly SettingsService _settingsService;
        private MediaPlayer? _soundPlayer;
        private MediaPlayer? _dialoguePlayer;
        private DispatcherTimer? _progressTimer;
        private double _progressWidth = 0;
        private const double TotalSeconds = 15;
        private double _elapsed = 0;

        public event EventHandler? UserActivityDetected;

        public OverlayWindow(SettingsService settingsService)
        {
            InitializeComponent();
            _settingsService = settingsService;
            Loaded += (_, _) => BeginEntrance();
        }

        // ─────────────────────────────────────────────────────────────────
        // Entrance
        // ─────────────────────────────────────────────────────────────────
        private void BeginEntrance()
        {
            try
            {
                CenterOnScreen();
                ShowDialogue();
                StartSteamAnimations();
                StartRippleAnimation();
                StartGlowPulse();
                StartProgressCountdown();
                SetupSound();
                PlayDialogueSound();
            }
            catch (Exception ex)
            {
                Logger.Error($"Overlay entrance failed: {ex.Message}");
            }
        }

        private void CenterOnScreen()
        {
            var w = SystemParameters.PrimaryScreenWidth;
            var h = SystemParameters.PrimaryScreenHeight;
            Left = (w - ActualWidth) / 2;
            Top  = (h - ActualHeight) / 2;
            if (Left < 0) Left = 0;
            if (Top  < 0) Top  = 0;
        }

        // ─────────────────────────────────────────────────────────────────
        // Fade-in card
        // ─────────────────────────────────────────────────────────────────
        private void ShowDialogue()
        {
            DialogueText.Text = _settingsService.GetRandomDialogue();

            // Card + root card entrance
            var cardIn = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromMilliseconds(500)));
            var rootIn = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromMilliseconds(600)));
            var btnIn  = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromMilliseconds(400)))
            {
                BeginTime = TimeSpan.FromMilliseconds(700)
            };
            var progIn = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromMilliseconds(400)))
            {
                BeginTime = TimeSpan.FromMilliseconds(600)
            };

            RootCard.BeginAnimation(OpacityProperty, rootIn);
            DialogueCard.BeginAnimation(OpacityProperty, cardIn);
            CloseButton.BeginAnimation(OpacityProperty, btnIn);
            ProgressRow.BeginAnimation(OpacityProperty, progIn);
        }

        // ─────────────────────────────────────────────────────────────────
        // Steam particle animations
        // ─────────────────────────────────────────────────────────────────
        private void StartSteamAnimations()
        {
            AnimateSteam(Steam1, 0, 700, -30);
            AnimateSteam(Steam2, 200, 900, -25);
            AnimateSteam(Steam3, 400, 600, -35);
        }

        private void AnimateSteam(UIElement el, int delayMs, int durationMs, double yOffset)
        {
            var up = new DoubleAnimation(0, yOffset,
                new Duration(TimeSpan.FromMilliseconds(durationMs)))
            {
                BeginTime       = TimeSpan.FromMilliseconds(delayMs),
                AutoReverse     = true,
                RepeatBehavior  = RepeatBehavior.Forever,
                EasingFunction  = new SineEase { EasingMode = EasingMode.EaseInOut }
            };
            var fade = new DoubleAnimation(0, 1,
                new Duration(TimeSpan.FromMilliseconds(durationMs / 2)))
            {
                BeginTime       = TimeSpan.FromMilliseconds(delayMs),
                AutoReverse     = true,
                RepeatBehavior  = RepeatBehavior.Forever
            };

            var tt = new TranslateTransform();
            el.RenderTransform = tt;
            tt.BeginAnimation(TranslateTransform.YProperty, up);
            el.BeginAnimation(OpacityProperty, fade);
        }

        // ─────────────────────────────────────────────────────────────────
        // Ripple on tea surface
        // ─────────────────────────────────────────────────────────────────
        private void StartGlowPulse()
        {
            var pulse = new DoubleAnimation(0.3, 1.0,
                new Duration(TimeSpan.FromSeconds(1.8)))
            {
                AutoReverse    = true,
                RepeatBehavior = RepeatBehavior.Forever,
                EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut }
            };
            GlowRing.BeginAnimation(OpacityProperty, pulse);
        }

        private void StartRippleAnimation()
        {
            var grow = new DoubleAnimation(20, 60,
                new Duration(TimeSpan.FromSeconds(1.2)))
            {
                AutoReverse     = true,
                RepeatBehavior  = RepeatBehavior.Forever,
                EasingFunction  = new SineEase { EasingMode = EasingMode.EaseInOut }
            };
            var pulse = new DoubleAnimation(0, 0.6,
                new Duration(TimeSpan.FromSeconds(0.6)))
            {
                AutoReverse     = true,
                RepeatBehavior  = RepeatBehavior.Forever
            };

            RippleEllipse.BeginAnimation(WidthProperty, grow);
            RippleEllipse.BeginAnimation(OpacityProperty, pulse);
        }

        // ─────────────────────────────────────────────────────────────────
        // Progress countdown bar
        // ─────────────────────────────────────────────────────────────────
        private void StartProgressCountdown()
        {
            // Measure after layout pass
            Dispatcher.InvokeAsync(() =>
            {
                _progressWidth = ProgressRow.ActualWidth;
                if (_progressWidth <= 0) _progressWidth = 520;
                ProgressBar.Width = _progressWidth;

                _progressTimer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromMilliseconds(100)
                };
                _progressTimer.Tick += (_, _) =>
                {
                    _elapsed += 0.1;
                    double ratio = Math.Min(_elapsed / TotalSeconds, 1.0);
                    ProgressBar.Width = _progressWidth * (1 - ratio);

                    if (_elapsed >= TotalSeconds)
                    {
                        _progressTimer.Stop();
                        CloseWithFade();
                    }
                };
                _progressTimer.Start();
            }, System.Windows.Threading.DispatcherPriority.Loaded);
        }

        private void CloseWithFade()
        {
            var fade = new DoubleAnimation(1, 0, new Duration(TimeSpan.FromMilliseconds(400)));
            fade.Completed += (_, _) => Close();
            RootCard.BeginAnimation(OpacityProperty, fade);
        }

        // ─────────────────────────────────────────────────────────────────
        // Sound
        // ─────────────────────────────────────────────────────────────────
        private void SetupSound()
        {
            if (!_settingsService.GetSettings().SoundEnabled) return;
            try
            {
                string[] paths =
                {
                    Path.Combine(AppContext.BaseDirectory, "Assets", "pour.mp3"),
                    Path.Combine(AppContext.BaseDirectory, "pour.mp3")
                };
                foreach (var p in paths)
                {
                    if (!File.Exists(p)) continue;
                    _soundPlayer = new MediaPlayer();
                    _soundPlayer.Open(new Uri(p, UriKind.Absolute));
                    _soundPlayer.MediaEnded += (_, _) => _soundPlayer.Position = TimeSpan.Zero;
                    _soundPlayer.Play();
                    break;
                }
            }
            catch (Exception ex) { Logger.Error($"Sound error: {ex.Message}"); }
        }

        private void PlayDialogueSound()
        {
            if (!_settingsService.GetSettings().SoundEnabled) return;
            try
            {
                string prefLang = _settingsService.GetSettings().PreferredLanguage;
                string langDir  = Path.Combine(AppContext.BaseDirectory, "Assets", "Audio", prefLang);

                // 1. Try language-specific folder first
                if (Directory.Exists(langDir))
                {
                    var files = Directory.GetFiles(langDir, "*.mp3");
                    if (files.Length > 0)
                    {
                        PlayFile(files[new Random().Next(files.Length)]);
                        return;
                    }
                }

                // 2. Fall back to legacy named files in Assets/
                string[] legacyPaths =
                {
                    Path.Combine(AppContext.BaseDirectory, "Assets", "tea_dialogue.mp3"),
                    Path.Combine(AppContext.BaseDirectory, "Assets", "dialogue.mp3"),
                    Path.Combine(AppContext.BaseDirectory, "tea_dialogue.mp3"),
                };
                foreach (var p in legacyPaths)
                {
                    if (File.Exists(p)) { PlayFile(p); return; }
                }

                Logger.Log("No dialogue audio found — text-only mode.");
            }
            catch (Exception ex) { Logger.Error($"Dialogue audio error: {ex.Message}"); }
        }

        private void PlayFile(string path)
        {
            _dialoguePlayer = new MediaPlayer();
            _dialoguePlayer.Open(new Uri(path, UriKind.Absolute));
            _dialoguePlayer.Play();
            Logger.Success($"Playing: {Path.GetFileName(path)}");
        }

        // ─────────────────────────────────────────────────────────────────
        // Interaction
        // ─────────────────────────────────────────────────────────────────
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            UserActivityDetected?.Invoke(this, EventArgs.Empty);
            _progressTimer?.Stop();
            CloseWithFade();
        }

        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDown(e);
            // Allow click-to-drag but not close (unless on button)
            if (e.Source is not Button && e.LeftButton == MouseButtonState.Pressed)
            {
                try { DragMove(); } catch { }
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _progressTimer?.Stop();
            _soundPlayer?.Stop();   _soundPlayer?.Close();
            _dialoguePlayer?.Stop();_dialoguePlayer?.Close();
        }
    }
}
