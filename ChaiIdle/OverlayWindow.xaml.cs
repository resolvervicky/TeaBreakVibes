using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.IO;

namespace ChaiIdle
{
    /// <summary>
    /// Interaction logic for OverlayWindow.xaml
    /// Da, overlay ba! Transparent, always-on-top, click-through except close button
    /// </summary>
    public partial class OverlayWindow : Window
    {
        private SettingsService _settingsService;
        private MediaPlayer? _teaVideoPlayer;   // Main video player (VideoDrawing)
        private MediaPlayer? _soundPlayer;      // Pour/pouring sound (looping)
        private MediaPlayer? _dialoguePlayer;   // Dialogue sound (plays once)
        private bool _isClickThrough = true;
        private bool _dialoguePlayed = false;   // Da, track if dialogue already played ba!

        public event EventHandler? UserActivityDetected;

        public OverlayWindow(SettingsService settingsService)
        {
            InitializeComponent();
            _settingsService = settingsService;

            Setup();
        }

        private void Setup()
        {
            try
            {
                // Position window at center-bottom of screen
                var screenWidth = SystemParameters.PrimaryScreenWidth;
                var screenHeight = SystemParameters.PrimaryScreenHeight;
                
                this.Left = (screenWidth - 800) / 2;
                this.Top = screenHeight * 0.6;

                // Load and play animation
                LoadAnimation();

                // Show dialogue
                ShowDialogue();

                // Setup sounds (pour + dialogue)
                SetupSound();

                // Da, play dialogue at overlay start (0-0.5 sec delay for max meme timing!)
                var dialogueTimer = new System.Windows.Threading.DispatcherTimer();
                dialogueTimer.Interval = TimeSpan.FromMilliseconds(100);
                dialogueTimer.Tick += (s, e) =>
                {
                    dialogueTimer.Stop();
                    PlayDialogueSound();
                };
                dialogueTimer.Start();

                // Auto-close after 10 seconds
                var closeTimer = new System.Windows.Threading.DispatcherTimer();
                closeTimer.Interval = TimeSpan.FromSeconds(10);
                closeTimer.Tick += (s, e) =>
                {
                    closeTimer.Stop();
                    Close();
                };
                closeTimer.Start();

                Console.WriteLine("[ChaiIdle] Overlay window created - Time for chai!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ChaiIdle Error] Overlay setup failed: {ex.Message}");
            }
        }

        private void LoadAnimation()
        {
            try
            {
                // Try to load tea animation - fallback to placeholder
                string? teaAnimPath = null;

                // Da, log directories ba!
                Logger.Log($"Base Dir: {AppContext.BaseDirectory}");
                Logger.Log($"Current Dir: {Directory.GetCurrentDirectory()}");

                // Look for tea.gif (preferred), tea.mp4, or tea_pour.png (generated)
                string[] possiblePaths = new[]
                {
                    Path.Combine(AppContext.BaseDirectory, "Assets", "tea.gif"),
                    Path.Combine(AppContext.BaseDirectory, "tea.gif")
                };

                foreach (var path in possiblePaths)
                {
                    Logger.Log($"Checking path: {path}");
                    if (File.Exists(path))
                    {
                        teaAnimPath = path;
                        Logger.Success($"Found animation at: {path}");
                        break;
                    }
                }

                if (!string.IsNullOrEmpty(teaAnimPath))
                {
                    if (teaAnimPath.EndsWith(".png") || teaAnimPath.EndsWith(".jpg"))
                    {
                        // Use Image control for static assets with "breathing" animation
                        ShowStaticImageWithAnimation(teaAnimPath);
                    }
                    else
                    {
                        // Use WpfAnimatedGif for reliable GIF playback
                        AnimationImage.Visibility = Visibility.Visible;
                        var image = new BitmapImage(new Uri(teaAnimPath, UriKind.Absolute));
                        WpfAnimatedGif.ImageBehavior.SetAnimatedSource(AnimationImage, image);
                        Logger.Log($"Animation source set via WpfAnimatedGif: {teaAnimPath}");
                    }
                }
                else
                {
                    Logger.Error("Video file not found anywhere!");
                    ShowEmojiAnimation();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ChaiIdle Error] Failed to load animation: {ex.Message}");
                ShowEmojiAnimation();
            }
        }

        private void ShowStaticImageWithAnimation(string imagePath)
        {
            // Create Image control
            var img = new Image
            {
                Source = new BitmapImage(new Uri(imagePath, UriKind.Absolute)),
                Width = 800,
                Height = 450,
                Stretch = Stretch.UniformToFill,
                RenderTransformOrigin = new Point(0.5, 0.5)
            };

            var scale = new ScaleTransform(1, 1);
            img.RenderTransform = scale;

            // Breathing animation (slow zoom)
            var zoom = new DoubleAnimation
            {
                From = 1.0,
                To = 1.05,
                Duration = TimeSpan.FromSeconds(3),
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever
            };
            scale.BeginAnimation(ScaleTransform.ScaleXProperty, zoom);
            scale.BeginAnimation(ScaleTransform.ScaleYProperty, zoom);

            // Replace Image control with the new one if needed (though we usually just update the source)
            if (AnimationImage.Parent is Border border)
            {
                border.Child = img;
            }

            Logger.Log($"Showing static asset with breathing animation: {imagePath}");
        }

        private void ShowEmojiAnimation()
        {
            // Create animated emoji fallback
            var animationTextBlock = new TextBlock
            {
                Text = "🍵",
                FontSize = 120,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            var scaleTransform = new ScaleTransform(1, 1);
            animationTextBlock.RenderTransform = scaleTransform;

            // Create bounce animation
            var animation = new DoubleAnimationUsingKeyFrames();
            animation.KeyFrames.Add(new LinearDoubleKeyFrame(1.0, KeyTime.FromTimeSpan(TimeSpan.Zero)));
            animation.KeyFrames.Add(new LinearDoubleKeyFrame(1.1, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(300))));
            animation.KeyFrames.Add(new LinearDoubleKeyFrame(1.0, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(600))));
            animation.RepeatBehavior = RepeatBehavior.Forever;

            scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, animation);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, animation);

            // Find the parent container of AnimationImage
            if (AnimationImage.Parent is Border border)
            {
                border.Child = animationTextBlock;
            }
            else if (this.Content is Grid grid)
            {
                // Backup plan: add to main grid
                grid.Children.Add(animationTextBlock);
            }

            Console.WriteLine("[ChaiIdle] Using emoji animation fallback");
        }

        private void ShowDialogue()
        {
            try
            {
                var dialogue = _settingsService.GetRandomDialogue();
                DialogueText.Text = dialogue;

                // Fade in animation
                var fadeIn = new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    Duration = TimeSpan.FromSeconds(1),
                    BeginTime = TimeSpan.FromSeconds(1)
                };

                DialogueText.BeginAnimation(OpacityProperty, fadeIn);

                // Fade in close button after 2 seconds
                var fadeInButton = new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    Duration = TimeSpan.FromSeconds(0.5),
                    BeginTime = TimeSpan.FromSeconds(2)
                };

                CloseButton.BeginAnimation(OpacityProperty, fadeInButton);

                Console.WriteLine($"[ChaiIdle] Dialogue shown: {dialogue}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ChaiIdle Error] Failed to show dialogue: {ex.Message}");
            }
        }

        private void SetupSound()
        {
            try
            {
                if (!_settingsService.GetSettings().SoundEnabled)
                    return;

                // Setup pour/pouring sound (looping)
                string[] possiblePourSounds = new[]
                {
                    Path.Combine(AppContext.BaseDirectory, "Assets", "pour.mp3"),
                    Path.Combine(AppContext.BaseDirectory, "pour.mp3"),
                };

                foreach (var path in possiblePourSounds)
                {
                    if (File.Exists(path))
                    {
                        _soundPlayer = new MediaPlayer();
                        _soundPlayer.Open(new Uri(path));
                        _soundPlayer.MediaEnded += (s, e) => _soundPlayer.Position = TimeSpan.Zero; // Loop
                        _soundPlayer.Play();
                        Console.WriteLine($"[ChaiIdle] Pour sound playing from {path}");
                        break;
                    }
                }

                Console.WriteLine("[ChaiIdle] Sound setup complete");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ChaiIdle Error] Failed to setup sound: {ex.Message}");
            }
        }

        private void PlayDialogueSound()
        {
            try
            {
                if (_dialoguePlayed || !_settingsService.GetSettings().SoundEnabled)
                    return;

                // Look for dialogue audio files
                string[] possibleDialogueSounds = new[]
                {
                    Path.Combine(AppContext.BaseDirectory, "Assets", "tea_dialogue.mp3"),
                    Path.Combine(AppContext.BaseDirectory, "Assets", "dialogue.mp3"),
                    Path.Combine(AppContext.BaseDirectory, "tea_dialogue.mp3"),
                    Path.Combine(AppContext.BaseDirectory, "dialogue.mp3"),
                };

                foreach (var path in possibleDialogueSounds)
                {
                    if (File.Exists(path))
                    {
                        // Da, dialogue plays once ba! Not looped!
                        _dialoguePlayer = new MediaPlayer();
                        _dialoguePlayer.Open(new Uri(path, UriKind.Absolute));
                        _dialoguePlayer.Play();
                        _dialoguePlayed = true;

                        Console.WriteLine($"[ChaiIdle] Dialogue sound playing from {path} - ASMR + Trickster voice time!");
                        return;
                    }
                }

                Console.WriteLine("[ChaiIdle] No dialogue sound found - silent mode (text only)");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ChaiIdle Error] Failed to play dialogue: {ex.Message}");
            }
        }

        private void AnimationImage_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            Logger.Error($"Animation playback failed: {e.ErrorException.Message}");
            ShowEmojiAnimation();
        }

        // Mouse events to detect user activity
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (_isClickThrough)
            {
                e.Handled = true;
                try { this.DragMove(); } catch { }
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.Key != Key.None)
            {
                UserActivityDetected?.Invoke(this, EventArgs.Empty);
                Close();
            }
        }

        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDown(e);
            if (e.Source is Button)
            {
                _isClickThrough = false;
            }
            else
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    UserActivityDetected?.Invoke(this, EventArgs.Empty);
                    Close();
                }
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            UserActivityDetected?.Invoke(this, EventArgs.Empty);
            Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _teaVideoPlayer?.Stop();
            _teaVideoPlayer?.Close();
            _soundPlayer?.Stop();
            _soundPlayer?.Close();
            _dialoguePlayer?.Stop();
            _dialoguePlayer?.Close();
        }
    }
}
