# ChaiIdle (OruTeaDa) - Indian IT Engineer's Chai Break Desktop Toy

```
 ☕ OruTeaDa ☕
 Da, chai break time ba!
```

## 🍵 What is ChaiIdle?

**ChaiIdle** is a fun, viral desktop toy for Indian IT engineers. After you've been coding for 5 minutes without moving the mouse or keyboard, a cute animated tea break appears on your screen with:

- ✨ Animated tea being poured (ASMR style)
- 🎵 Realistic tea-pouring sound
- 💬 Viral Tamil/Tanglish/Hinglish dialogue ("Oru tea potta dhan sariya irukkum da!")
- 🍵 Multiple tea types (chai, filter coffee, juice)
- 🔕 System tray control (Pause, Resume, Settings, Exit)
- ⚡ Super lightweight (<50MB RAM)
- 🎬 One-click "Share Chai Moment" to X/Instagram

## 🚀 Installation

### Option 1: Download .exe (Recommended - No Installation)
1. Download `ChaiIdle.exe` from Releases
2. Double-click to run
3. It hides to system tray automatically
4. Right-click tray icon to manage

### Option 2: Build from Source

**Requirements:**
- .NET 8 SDK or later ([download](https://dotnet.microsoft.com/download/dotnet/8.0))
- Visual Studio Code or Visual Studio (optional)

**Build:**
```bash
cd ChaiIdle
dotnet build -c Release
```

**Run:**
```bash
dotnet run
```

**Publish as Single Exe:**
```bash
dotnet publish -c Release -r win-x64 \
  --self-contained true \
  /p:PublishSingleFile=true \
  /p:IncludeNativeLibrariesForSelfExtract=true \
  /p:DebugType=embedded
```

The single `.exe` will be in `bin/Release/net8.0-windows/win-x64/publish/`

## 🎮 How to Use

1. **Run ChaiIdle** - It minimizes to system tray immediately
2. **Code normally** - Keep working, take a break, whatever!
3. **Idle for 5 minutes** - After 5 min of no mouse/keyboard activity:
   - Transparent overlay appears
   - Cute tea animation plays
   - Random dialogue shows (Tamil/English/Hinglish)
   - ASMR sound plays
4. **Click anywhere or press any key** - Overlay closes, back to work!

## ⚙️ Configuration

Settings are stored in: `%APPDATA%\ChaiIdle\settings.json`

```json
{
  "idleMinutes": 5,
  "enabledLanguages": ["Tamil", "English", "Hinglish"],
  "teaTypes": ["chai", "filter_coffee", "juice"],
  "soundEnabled": true,
  "autoShareEnabled": false,
  "isPaused": false,
  "customDialogues": {}
}
```

### Add Custom Dialogues

Edit `%APPDATA%\ChaiIdle\dialogues.json` and add your own memes:

```json
{
  "Tamil": [
    "Boss, oru cutting chai venum da!",
    "Enna machan, code la stuck ah?"
  ],
  "Hinglish": [
    "Chai time, debugging time!",
    "Loop se break, chai se peace!"
  ]
}
```

## 🔧 System Tray Menu

Right-click the tea icon in system tray:

- **⚙️ Settings** - Open preferences (coming soon: full UI)
- **⏸️ Pause** - Temporarily disable chai breaks
- **▶️ Resume** - Re-enable chai breaks
- **ℹ️ About** - View version info
- **❌ Exit** - Close ChaiIdle

## 📊 Features

| Feature | Status |
|---------|--------|
| Idle Detection (P/Invoke) | ✅ Done |
| Transparent Overlay | ✅ Done |
| Multi-language Dialogues | ✅ Done |
| ASMR Sound | ✅ Done (awaiting MP3) |
| System Tray | ✅ Done |
| JSON Settings | ✅ Done |
| Single-file .exe | ✅ Ready |
| Settings UI | 🔄 Coming Soon |
| Share to Social Media | 🔄 Coming Soon |
| Custom Tea Animations | 🔄 Coming Soon |

## 🐛 Troubleshooting

**Overlay not showing?**
- Check if paused (right-click tray > Resume)
- Verify idle time setting (default: 5 min)
- Check console: `ChaiIdle.exe 2>error.log`

**No sound?**
- Ensure `pour.mp3` is in Assets folder
- Check `settings.json` → `"soundEnabled": true`
- System volume isn't muted

**High CPU usage?**
- Idle detector checks every 10 seconds (normal)
- Report CPU issues on GitHub

## 📝 License

Free to use, share, and fork! Made with chai for Indian devs. ☕

## 🙏 Contributing

Found a bug? Have a funny dialogue idea? Fork it, fix it, PR it!

**Ideas:**
- More tea animations
- Voice-over instead of text
- Achievement system ("You saved 100 hours of work this week!")
- Leaderboard with friends
- Dark mode
- Dock/taskbar notifications

## 🎬 Viral Ideas

- **#ChaiIdle Challenge** - Screenshot your most hilarious dialogue and tweet it
- **Custom Dialogues** - Add your team's inside jokes
- **Share Button** - Auto-generate meme images for Twitter
- **Stats Mode** - Track how many chai breaks you've had

---

**Made by Chennai devs, for Chennai devs. 🍵 Oru tea vadikka sollrindhu thiyanum ba!**

Questions? Issues? Chai recommendations? Open an issue!
