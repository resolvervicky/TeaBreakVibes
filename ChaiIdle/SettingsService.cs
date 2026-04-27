using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ChaiIdle
{
    // Da, settings service ba! Store config in AppData
    public class SettingsService
    {
        private readonly string _settingsPath;
        private readonly string _dialoguesPath;
        private Settings _currentSettings = new();

        public class Settings
        {
            [JsonProperty("idleMinutes")]
            public int IdleMinutes { get; set; } = 5;

            [JsonProperty("preferredLanguage")]
            public string PreferredLanguage { get; set; } = "Tamil";

            [JsonProperty("enabledLanguages")]
            public List<string> EnabledLanguages { get; set; } = new() { "Tamil", "English", "Hinglish" };

            [JsonProperty("teaTypes")]
            public List<string> TeaTypes { get; set; } = new() { "chai", "filter_coffee", "juice" };

            [JsonProperty("customDialogues")]
            public Dictionary<string, List<string>> CustomDialogues { get; set; } = new();

            [JsonProperty("soundEnabled")]
            public bool SoundEnabled { get; set; } = true;

            [JsonProperty("autoShareEnabled")]
            public bool AutoShareEnabled { get; set; } = false;

            [JsonProperty("isPaused")]
            public bool IsPaused { get; set; } = false;
        }

        public class Dialogues
        {
            [JsonProperty("Tamil")]
            public List<string> Tamil { get; set; } = new();

            [JsonProperty("English")]
            public List<string> English { get; set; } = new();

            [JsonProperty("Hinglish")]
            public List<string> Hinglish { get; set; } = new();

            [JsonProperty("Telugu")]
            public List<string> Telugu { get; set; } = new();
        }

        public SettingsService()
        {
            string appDataPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "ChaiIdle"
            );

            if (!Directory.Exists(appDataPath))
                Directory.CreateDirectory(appDataPath);

            _settingsPath = Path.Combine(appDataPath, "settings.json");
            _dialoguesPath = Path.Combine(appDataPath, "dialogues.json");

            LoadSettings();
        }

        public void LoadSettings()
        {
            try
            {
                if (File.Exists(_settingsPath))
                {
                    string json = File.ReadAllText(_settingsPath);
                    _currentSettings = JsonConvert.DeserializeObject<Settings>(json) ?? new Settings();
                }
                else
                {
                    _currentSettings = new Settings();
                    SaveSettings();
                }

                Console.WriteLine($"[ChaiIdle] Settings loaded - Idle after {_currentSettings.IdleMinutes} minutes");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ChaiIdle] Failed to load settings: {ex.Message}");
                _currentSettings = new Settings();
            }
        }

        public void SaveSettings()
        {
            try
            {
                string json = JsonConvert.SerializeObject(_currentSettings, Formatting.Indented);
                File.WriteAllText(_settingsPath, json);
                Console.WriteLine("[ChaiIdle] Settings saved successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ChaiIdle] Failed to save settings: {ex.Message}");
            }
        }

        public Settings GetSettings() => _currentSettings;

        public void UpdateSettings(Settings newSettings)
        {
            _currentSettings = newSettings;
            SaveSettings();
        }

        public int GetIdleThresholdMs()
        {
            return _currentSettings.IdleMinutes * 60 * 1000;
        }

        public List<string> GetEnabledDialogues()
        {
            var dialogues = new List<string>();
            var allDialogues = GetDefaultDialogues();
            var prefLang = _currentSettings.PreferredLanguage;

            if (allDialogues.TryGetValue(prefLang, out var langDialogues))
            {
                dialogues.AddRange(langDialogues);
            }

            // Add custom dialogues for this language if any
            if (_currentSettings.CustomDialogues.TryGetValue(prefLang, out var customDialogues))
            {
                dialogues.AddRange(customDialogues);
            }

            return dialogues.Count > 0 ? dialogues : allDialogues["Tamil"];
        }

        public string GetRandomDialogue()
        {
            var dialogues = GetEnabledDialogues();
            if (dialogues.Count == 0)
                return "Oru tea potta dhan sariya irukkum da 🍵";

            Random random = new Random();
            return dialogues[random.Next(dialogues.Count)];
        }

        public string GetRandomTeaType()
        {
            Random random = new Random();
            return _currentSettings.TeaTypes[random.Next(_currentSettings.TeaTypes.Count)];
        }

        // Da, default meme dialogues ba!
        private Dictionary<string, List<string>> GetDefaultDialogues()
        {
            return new Dictionary<string, List<string>>
            {
                {
                    "Tamil", new List<string>
                    {
                        "Oru tea potta dhan sariya irukkum da 🍵",
                        "Boss, oru cutting chai venum da",
                        "Da, system-ku rest koduthutiya?",
                        "Chai break time ba!",
                        "Enna machan, code-ku tired-ah?",
                        "Filter coffee-la irundhu start pannalam da",
                        "Oru tea, oru life da!",
                        "Code bug-la irundhu break pottu chai kudi da",
                        "Chai vandha peace-ah irukum da",
                        "Seri da, oru tea potti vaippom da!",
                        "Debug-ku chai necessary da",
                        "Stack overflow? Chai overflow da!",
                        "Infinite loop? Chai loop vaippom da!",
                        "Oru tea vandha sari ayidum da",
                        "Boss, production server chai-ah kudhikkudha?",
                        "Chai-ku priority highest ba!",
                        "Merge conflict kuraikka chai kudi da",
                        "PR review-ku chai mandatory da",
                        "Deployment-la irundhu back off, chai kudi da",
                        "Memory leak-la irundhu chai heal pannum ba!"
                    }
                },
                {
                    "English", new List<string>
                    {
                        "Time for a chai break! 🍵",
                        "Boss, take a tea break.",
                        "Is your system as tired as you are?",
                        "Chai break time, everyone!",
                        "Your code needs a break, and so do you.",
                        "Caffeine fix incoming!",
                        "One tea, one life.",
                        "Bugs are better debugged with chai.",
                        "Peace comes with a cup.",
                        "Let's brew a solution.",
                        "Debug mode: Chai Edition.",
                        "Stack overflow? Chai overflow!",
                        "Infinite loops need chai loops.",
                        "A cup of chai fixes everything.",
                        "Production running? Sip this chai.",
                        "Chai is the top priority.",
                        "Merge conflicts taste better with chai.",
                        "PR reviews require chai.",
                        "Deployment needs a break, just like you.",
                        "Memory leaks? Chai heals!"
                    }
                },
                {
                    "Hinglish", new List<string>
                    {
                        "Bhai, chai break time hai 🍵",
                        "Code se break lo, chai lo.",
                        "Ek chai, sab theek ho jayega.",
                        "System ko rest do, chai lo.",
                        "Chai pee lo, stress kam ho jayega.",
                        "Coding se thak gaye? Chai time!",
                        "Bug fix hota hai chai se.",
                        "Caffeine + Coding = Success.",
                        "Chai ke bina life incomplete hai.",
                        "Break time, chai time!",
                        "Deadline se pehle chai zaroor.",
                        "Meeting ke baad chai zaroori hai.",
                        "Logic chalti hai chai se.",
                        "Documentation + Chai = Perfection.",
                        "Deployment se pehle chai mandatory.",
                        "Testing ke time chai zaroori.",
                        "Refactoring bada aasan hai chai se.",
                        "Performance tuning + Chai = Magic.",
                        "Security patches need chai support.",
                        "DevOps pipeline + Chai = Smooth!"
                    }
                },
                {
                    "Telugu", new List<string>
                    {
                        "Chai break time ra! 🍵",
                        "Boss, chhay teesuko.",
                        "Code-to tired-ah? Chai-ki ocheyandi.",
                        "System-ku rest ichey, chai kudu.",
                        "Chai unte peace untundi ra.",
                        "Bug fix avutundi chai unte.",
                        "Stress poddi chai-tho.",
                        "Chai pee, back to work!",
                        "Break time, chai time kada!",
                        "Oka chhay, sab theek!"
                    }
                }
            };
        }
    }
}
