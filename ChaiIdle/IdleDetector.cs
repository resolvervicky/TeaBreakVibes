using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Threading;

namespace ChaiIdle
{
    // Da, idle detection ba! P/Invoke magic with Windows API
    internal class IdleDetector
    {
        [DllImport("user32.dll")]
        private static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

        [StructLayout(LayoutKind.Sequential)]
        private struct LASTINPUTINFO
        {
            public static readonly int SizeOf = Marshal.SizeOf(typeof(LASTINPUTINFO));
            [MarshalAs(UnmanagedType.U4)]
            public int cbSize;
            [MarshalAs(UnmanagedType.U4)]
            public uint dwTime;
        }

        private DispatcherTimer? _idleCheckTimer;
        private bool _wasIdle = false;

        public int CurrentIdleTimeMs { get; private set; } = 0;

        public event EventHandler<IdleStateChangedEventArgs>? IdleStateChanged;

        // Idle time in milliseconds (default 5 minutes)
        public int IdleThresholdMs { get; set; } = 1 * 60 * 1000;

        public bool IsEnabled { get; set; } = true;

        public void Start()
        {
            if (_idleCheckTimer != null)
                return;

            // Da, 2 second timer ba! Super fast check for testing
            _idleCheckTimer = new DispatcherTimer();
            _idleCheckTimer.Interval = TimeSpan.FromSeconds(2);
            _idleCheckTimer.Tick += CheckIdleStatus;
            _idleCheckTimer.Start();

            Logger.Log("Idle detector started - watching for chai break time 🍵");
        }

        public void Stop()
        {
            if (_idleCheckTimer != null)
            {
                _idleCheckTimer.Stop();
                _idleCheckTimer = null;
            }
        }

        private void CheckIdleStatus(object? sender, EventArgs e)
        {
            if (!IsEnabled)
                return;

            try
            {
                LASTINPUTINFO lii = new LASTINPUTINFO();
                lii.cbSize = LASTINPUTINFO.SizeOf;

                if (!GetLastInputInfo(ref lii))
                    return;

                uint currentTickCount = (uint)Environment.TickCount;
                uint idleTime = currentTickCount - lii.dwTime;
                CurrentIdleTimeMs = (int)idleTime;

                // Da, direct uint comparison ba! No rollover issues here.
                bool isNowIdle = idleTime >= (uint)IdleThresholdMs;

                if (isNowIdle && !_wasIdle)
                {
                    _wasIdle = true;
                    Logger.Log($"Idle detected! {idleTime}ms >= {IdleThresholdMs}ms - Oru chai venum da!");
                    IdleStateChanged?.Invoke(this, new IdleStateChangedEventArgs { IsIdle = true, IdleTimeMs = (int)idleTime });
                }
                else if (!isNowIdle && _wasIdle)
                {
                    _wasIdle = false;
                    Logger.Log("User active again! Chat la irundutiya da!");
                    IdleStateChanged?.Invoke(this, new IdleStateChangedEventArgs { IsIdle = false, IdleTimeMs = (int)idleTime });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ChaiIdle Error] IdleDetector crashed: {ex.Message}");
            }
        }

        public void ResetIdleTimer()
        {
            // User activity detected - bring back to work mode
            _wasIdle = false;
        }
    }

    public class IdleStateChangedEventArgs : EventArgs
    {
        public bool IsIdle { get; set; }
        public int IdleTimeMs { get; set; }
    }
}
