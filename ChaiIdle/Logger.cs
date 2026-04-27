using System;
using System.Collections.Generic;

namespace ChaiIdle
{
    public static class Logger
    {
        public static event EventHandler<string>? LogAdded;
        private static readonly List<string> _logs = new();

        public static void Log(string message)
        {
            string log = $"[{DateTime.Now:HH:mm:ss}] {message}";
            _logs.Add(log);
            Console.WriteLine(log);
            LogAdded?.Invoke(null, log);
        }

        public static void Error(string message)
        {
            Log($"❌ ERROR: {message}");
        }

        public static void Success(string message)
        {
            Log($"✅ {message}");
        }

        public static string GetAllLogs() => string.Join(Environment.NewLine, _logs);
    }
}
