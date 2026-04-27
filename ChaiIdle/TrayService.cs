using System;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace ChaiIdle
{
    // Da, system tray ba! NotifyIcon with right-click menu
    public class TrayService
    {
        private NotifyIcon? _notifyIcon;
        private ContextMenuStrip? _contextMenu;

        public event EventHandler? SettingsClicked;
        public event EventHandler? PauseClicked;
        public event EventHandler? ResumeClicked;
        public event EventHandler? ExitClicked;
        public event EventHandler? AboutClicked;

        public bool IsPaused { get; set; } = false;

        public void Initialize()
        {
            try
            {
                _contextMenu = new ContextMenuStrip();

                var settingsItem = new ToolStripMenuItem("⚙️ Settings", null, (s, e) => SettingsClicked?.Invoke(this, EventArgs.Empty));
                var pauseItem = new ToolStripMenuItem("⏸️ Pause", null, (s, e) => HandlePauseClick());
                var aboutItem = new ToolStripMenuItem("ℹ️ About OruTeaDa", null, (s, e) => AboutClicked?.Invoke(this, EventArgs.Empty));
                var exitItem = new ToolStripMenuItem("❌ Exit", null, (s, e) => ExitClicked?.Invoke(this, EventArgs.Empty));

                _contextMenu.Items.Add(settingsItem);
                _contextMenu.Items.Add(pauseItem);
                _contextMenu.Items.Add(new ToolStripSeparator());
                _contextMenu.Items.Add(aboutItem);
                _contextMenu.Items.Add(exitItem);

                _notifyIcon = new NotifyIcon();
                
                // Try to load tea.ico, fallback to system icon
                try
                {
                    string[] possibleIconPaths = new[]
                    {
                        Path.Combine(AppContext.BaseDirectory, "Assets", "tea.ico"),
                        Path.Combine(AppContext.BaseDirectory, "tea.ico"),
                    };

                    bool iconLoaded = false;
                    foreach (var iconPath in possibleIconPaths)
                    {
                        if (File.Exists(iconPath))
                        {
                            _notifyIcon.Icon = new Icon(iconPath);
                            Console.WriteLine($"[ChaiIdle] Tray icon loaded from {iconPath} - Looking fancy ba!");
                            iconLoaded = true;
                            break;
                        }
                    }

                    if (!iconLoaded)
                    {
                        _notifyIcon.Icon = SystemIcons.Shield; // Fallback icon
                        Console.WriteLine("[ChaiIdle] Using system icon (tea.ico not found)");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ChaiIdle] Failed to load icon: {ex.Message}, using system icon");
                    _notifyIcon.Icon = SystemIcons.Shield;
                }

                _notifyIcon.Text = "ChaiIdle - OruTeaDa 🍵";
                _notifyIcon.ContextMenuStrip = _contextMenu;
                _notifyIcon.Visible = true;

                // Double-click to show about
                _notifyIcon.DoubleClick += (s, e) => AboutClicked?.Invoke(this, EventArgs.Empty);

                Console.WriteLine("[ChaiIdle] Tray service initialized - Ready da!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ChaiIdle Error] Failed to initialize tray: {ex.Message}");
            }
        }

        private void HandlePauseClick()
        {
            IsPaused = !IsPaused;
            if (IsPaused)
            {
                PauseClicked?.Invoke(this, EventArgs.Empty);
                ShowNotification("ChaiIdle Paused", "Chai breaks are paused. Click to resume!");
            }
            else
            {
                ResumeClicked?.Invoke(this, EventArgs.Empty);
                ShowNotification("ChaiIdle Resumed", "Chai breaks are active again!");
            }
        }

        public void UpdatePauseButtonText(bool isPaused)
        {
            if (_contextMenu?.Items[1] is ToolStripMenuItem pauseItem)
            {
                pauseItem.Text = isPaused ? "▶️ Resume" : "⏸️ Pause";
            }
        }

        public void ShowNotification(string title, string message, int durationMs = 5000)
        {
            _notifyIcon?.ShowBalloonTip(durationMs, title, message, ToolTipIcon.Info);
        }

        public void Dispose()
        {
            _notifyIcon?.Dispose();
            _contextMenu?.Dispose();
        }
    }
}
