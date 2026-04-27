# 🎵 ChaiIdle Audio Setup - Quick Start

This guide helps you quickly create or obtain the audio files needed for ChaiIdle's viral moment!

## ⚡ The 5-Minute Setup

### Step 1: Get or Create `tea_dialogue.mp3` (MOST IMPORTANT!)

**Fastest way (AI Voice):**
1. Go to https://play.ht or https://eleven-labs.io
2. Paste one of these scripts:

```
Oru tea potta dhan sariya irukkum da!
```
or
```
Boss, oru cutting chai venum da
```

3. Choose Indian English or Tamil voice
4. Download as MP3
5. Save as `tea_dialogue.mp3` in `Assets/` folder

**DIY Voice Recording:**
1. Open Windows Voice Recorder (or Audacity - free)
2. Record yourself saying the line dramatically
3. Export as MP3
4. Save to `Assets/tea_dialogue.mp3`

**Why this matters:** This plays the INSTANT your overlay appears = viral moment!

---

### Step 2: Get or Create `pour.mp3` (ASMR Sound)

**Fastest way (Find online):**
1. Go to https://freesound.org
2. Search: "pouring liquid" or "tea pouring"
3. Download highest-rated MP3
4. Save as `pour.mp3` in `Assets/` folder

**DIY ASMR Recording:**
1. Get a cup, water, microphone
2. Pour water into cup slowly (close to mic for ASMR effect)
3. Record 3-5 second clip
4. Edit in Audacity (trim silence, normalize)
5. Export as MP3 → `pour.mp3`

**Why this matters:** Creates immersive experience, loops continuously

---

### Step 3: Get or Create `tea.mp4` (Animation)

**Fastest way (Find online):**
1. Go to https://pexels.com or https://pixabay.com
2. Search: "tea pouring" or "coffee pouring"
3. Download video (1920x1080 preferred)
4. Save as `tea.mp4` in `Assets/` folder

**DIY Video:**
1. Film yourself pouring tea into a clear glass (side angle)
2. Keep it steady, 10-15 seconds
3. Convert to MP4 using ffmpeg:
   ```
   ffmpeg -i input.mov -c:v libx264 -preset fast -crf 23 -c:a aac output.mp4
   ```
4. Save as `tea.mp4`

**Why this matters:** Eye candy = users will share!

---

### Step 4: Get or Create `tea.ico` (Tray Icon)

**Fastest way (Online generator):**
1. Go to https://canva.com
2. Create 1:1 square design (tea cup emoji or simple tea cup icon)
3. Download as PNG
4. Go to https://convertio.co
5. Convert PNG → ICO (256x256)
6. Download and save as `tea.ico` in `Assets/` folder

**Shortcut:** Use existing icon
1. Search Google: "tea cup icon 256x256"
2. Download any PNG
3. Convert to ICO using online tool
4. Save as `tea.ico`

---

## ✅ Verification Checklist

```bash
# Navigate to Assets folder
cd Assets

# Check files exist:
ls -la
```

You should see:
```
tea_dialogue.mp3   (300KB - 1MB)
pour.mp3          (500KB - 1MB)
tea.mp4           (5-20MB)
tea.ico           (50-100KB)
```

---

## 🏗️ Build with Assets

Once files are in `Assets/` folder:

```bash
cd ChaiIdle

# Build for testing
dotnet build -c Release

# Publish as single .exe (includes assets!)
dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true
```

**Result:** `bin/Release/net8.0-windows/win-x64/publish/ChaiIdle.exe`

This .exe has ALL audio files embedded! 🎉

---

## 🎯 Exact Timing When User Runs App

```
T=0ms     → User idles for 5 min (configurable)
T=5min    → Overlay appears instantly
T=5min+0ms  → Tea animation starts
T=5min+0ms  → Pour sound (loop) starts  
T=5min+100ms → Tea dialogue plays ONCE (perfect timing!)
T=5min+3500ms → Dialogue ends, pour sound continues looping
T=5min+any → User moves mouse/presses key → instant close
T=5min+10s → Auto-close if user doesn't interact
```

**KEY:** Dialogue timing is everything! It plays at exact moment overlay appears.

---

## 📁 File Locations After Build

```
ChaiIdle/
├── Assets/
│   ├── tea_dialogue.mp3 ✓
│   ├── pour.mp3 ✓
│   ├── tea.mp4 ✓
│   └── tea.ico ✓
│
└── bin/Release/net8.0-windows/win-x64/publish/
    ├── ChaiIdle.exe (includes all assets!)
    └── Assets/
        ├── tea_dialogue.mp3
        ├── pour.mp3
        ├── tea.mp4
        └── tea.ico
```

---

## 🚀 Testing Audio Timing

After build, run the app:

```bash
# From publish folder
./ChaiIdle.exe
```

1. App hides to tray
2. Don't touch mouse for 5 minutes (or change idle time to 1 min in settings)
3. Overlay appears
4. You should hear:
   - ✓ Tea dialogue instantly ("Oru tea potta dhan sariya irukkum!")
   - ✓ Pour sound looping in background
   - ✓ Text fading in with dialogue
5. Click overlay or press key → closes silently

**If no audio:** Check if audio files exist in output folder

---

## 🎙️ Pro Audio Recording Tips

### For Dialogue (Most Important):
- Record in quiet room (minimize background noise)
- Speak dramatically, like a Trickster character
- Don't rush - let words hang for effect
- Good phrases:
  - "Oru tea potta dhan sariya irukkum da" (Tamil)
  - "Boss, oru cutting chai venum da" (Tanglish)
  - "Chai break time ba!" (Hinglish)

### For Pour Sound (ASMR):
- Use high-quality microphone (USB condenser mic ~$50)
- Position mic 15-20cm from cup
- Pour slowly for satisfying glug-glug effect
- Record multiple takes, pick the best
- Trim to 3-5 seconds in Audacity

### For Tea Video:
- Use phone camera (modern phones = great quality)
- Stable tripod or phone stand
- Side angle (so you see pouring action)
- Good lighting (natural light best)
- 10-15 second duration = perfect

---

## 🆘 Troubleshooting

**Q: Audio not playing in app?**
A: Check if files exist in:
```
%APPDATA%\ChaiIdle\settings.json
Check: "soundEnabled": true
```

**Q: Wrong audio playing?**
A: App looks for files in this order:
1. `Assets/tea_dialogue.mp3` → plays once at start
2. `Assets/dialogue.mp3` → fallback  
3. `Assets/pour.mp3` → loops during overlay
4. `Assets/tea.mp4` → main animation

**Q: File size too large?**
A: Reduce:
- Video: Compress with handbrake (target 10-15MB)
- Audio: Reduce bitrate to 128kbps MP3 (192 if possible)

**Q: Want to test without audio?**
A: In settings.json, set:
```json
"soundEnabled": false
```

---

## 🎉 You're Ready!

1. ✅ Add 4 audio/icon files to `Assets/`
2. ✅ Build with `dotnet publish`
3. ✅ Run the single .exe
4. ✅ Go viral! 🍵

---

## 📞 Need More Details?

See `Assets/README.md` for:
- Exact file specifications
- Where to find content
- Alternative assets
- Pro tips for virality

---

**Da, chai time ba! 🍵✨**
