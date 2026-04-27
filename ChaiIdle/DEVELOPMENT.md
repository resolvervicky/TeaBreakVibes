# ChaiIdle Development Notes

## Architecture Overview

```
ChaiIdle (WPF .NET 8)
│
├── MainWindow (Hidden)
│   ├── IdleDetector (P/Invoke GetLastInputInfo)
│   │   └── Fires event on idle/active state change
│   ├── SettingsService (JSON AppData storage)
│   │   └── Manages config + dialogues
│   ├── TrayService (NotifyIcon)
│   │   └── System tray menu
│   └── OverlayWindow (Transparent, always-on-top)
│       ├── MediaElement (MP4/GIF animation)
│       ├── TextBlock (Tamil/Hinglish dialogue)
│       ├── MediaPlayer (ASMR sound)
│       └── Auto-close timer (10 seconds)
```

## Key Design Decisions

### 1. **P/Invoke Idle Detection**
- Uses Windows `GetLastInputInfo` for accurate idle time
- Lightweight: checks every 10 seconds
- No admin rights required
- Handles system tick count wraparound

### 2. **Transparent Overlay**
- `WindowStyle="None"` + `AllowsTransparency="True"`
- `Topmost="True"` for always-on-top
- Click-through except for close button
- Auto-hides after 10 seconds OR user activity

### 3. **Settings Storage**
- `%APPDATA%\ChaiIdle\settings.json` for portability
- `%APPDATA%\ChaiIdle\dialogues.json` for custom memes
- No registry modification (clean uninstall)
- JSON for easy editing

### 4. **Single-File Distribution**
- Publish with `--self-contained` (includes .NET runtime)
- `PublishSingleFile=true` (single .exe)
- No installation needed
- Can be deleted to uninstall

## Performance Optimization

### Memory Usage
- Idle detector: ~2MB
- Overlay window: ~10MB (when visible)
- Total: <50MB as required

### CPU Usage
- Idle check every 10 seconds (0.01% utilization)
- Overlay uses GPU-accelerated rendering
- Auto-cleanup on close

## Future Enhancements

### Phase 2: Social Integration
- [ ] Screenshot overlay
- [ ] Generate meme caption
- [ ] Share to X/Twitter (OAuth)
- [ ] Leaderboard system

### Phase 3: Gamification
- [ ] Achievement system
- [ ] Tea break streak tracker
- [ ] Custom badges
- [ ] Stats dashboard

### Phase 4: Team Features
- [ ] Send chai to coworker
- [ ] Team leaderboard
- [ ] Custom team dialogues
- [ ] Corporate license

### Phase 5: Animation Library
- [ ] Different tea types (chai, coffee, juice, lassi, kombucha)
- [ ] Seasonal animations (winter chai, summer juice)
- [ ] User-uploaded animations
- [ ] Voice-over instead of text

## Testing Checklist

- [ ] Idle detection triggers at correct time
- [ ] Overlay appears and disappears correctly
- [ ] Dialogue randomly selected
- [ ] Sound plays (if file exists)
- [ ] Tray menu works
- [ ] Settings persist after restart
- [ ] Single-file .exe runs without installation
- [ ] Works on clean Windows 10/11 without .NET pre-installed
- [ ] No admin rights required
- [ ] No registry modifications
- [ ] Clean removal (delete .exe)

## Known Issues & Workarounds

### Issue: No animation plays
**Cause:** `tea.mp4` not found in Assets folder
**Workaround:** Falls back to emoji animation 🍵

### Issue: High CPU usage
**Cause:** Animation codec or old GPU
**Workaround:** Use static image instead of video

### Issue: Sound not playing
**Cause:** `pour.mp3` not found
**Workaround:** Falls back to silent mode

## Build Commands Reference

```bash
# Development
dotnet run

# Release build
dotnet build -c Release

# Single-file exe (recommended for distribution)
dotnet publish -c Release -r win-x64 --self-contained /p:PublishSingleFile=true

# Framework-dependent (smaller, requires .NET 8 on target)
dotnet publish -c Release -r win-x64

# Trimmed (aggressive, may break reflection)
dotnet publish -c Release -r win-x64 --self-contained /p:PublishTrimmed=true
```

## Code Style Guidelines

- Use Tamil-English comments: `// Da, idle aayita da!`
- Console.WriteLine for debugging
- Try-catch everywhere (app shouldn't crash)
- Event-driven architecture
- No blocking operations on UI thread

## Dependencies

- **Newtonsoft.Json** - JSON serialization
- **WPF** - UI framework (built-in with .NET)
- **System.Windows.Forms** - NotifyIcon (for tray)

No heavy dependencies = lightweight & fast!

## Marketing Copy Ideas

> "Oru tea potta dhan sariya irukkum da!" - Your code doesn't need coffee, it needs chai. 🍵

> ChaiIdle: Because debugging is better with chai breaks.

> One notification. One tea. One dev. One chai. All that's needed.

> For developers, by developers. Pure Tamil energy. Zero bloat.

---

**Da, happy coding ba! Remember: Chai first, code second. 🍵**
