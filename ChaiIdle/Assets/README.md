# ChaiIdle Assets Directory

This folder contains all media files for ChaiIdle/OruTeaDa. Files placed here are automatically copied to the build output folder.

## 📁 Required Files

### 1. `tea.mp4` - Tea Pouring Animation ☕
**Purpose:** Main animation showing tea/coffee/juice being poured

**Specifications:**
- Format: MP4 (H.264 video codec)
- Resolution: 1920x1080 or 1280x720 (min recommended)
- Duration: 10-15 seconds
- FPS: 30fps
- Background: Transparent OR solid white/black
- File size: 5-20MB

**Where to get:**
- Record your own tea pouring video
- AI video generation (Runway ML, Synthesia, etc.)
- Download from stock video sites (Pexels, Pixabay - search "tea pouring")
- Upscale with AI if needed

**Example filename:** `tea.mp4`

---

### 2. `pour.mp3` - Pouring Sound (ASMR Style) 🎵
**Purpose:** Background ASMR sound of tea/liquid pouring (glug-glug, sizzle)

**Specifications:**
- Format: MP3
- Bitrate: 192 kbps (or higher)
- Sample rate: 44.1 kHz
- Duration: 3-5 seconds
- Mono or Stereo: Stereo recommended for ASMR effect
- Content: Realistic liquid pouring sound (not music)
- File size: 500KB - 1MB

**Why separate from dialogue:**
- Loops continuously while overlay is visible
- Creates immersive ASMR experience
- Doesn't repeat dialogue annoyingly

**Where to get:**
- Record real tea pouring with good microphone
- Download from ASMR sound libraries (YouTube Audio Library, Pixabay Sounds)
- AI audio generation (Play.ht, Eleven Labs for sound effects)
- Freesound.org - search "pouring liquid" or "tea"

**Example filename:** `pour.mp3`

---

### 3. `tea_dialogue.mp3` - Tamil/Hinglish Trickster Voice 🎤
**Purpose:** Viral dialogue audio that plays exactly when overlay appears

**EXACT TIMING:** This plays IMMEDIATELY when overlay shows (0-0.5 sec delay max)

**Specifications:**
- Format: MP3
- Bitrate: 256 kbps (good quality for voice)
- Sample rate: 44.1 kHz or 48 kHz
- Duration: 2-4 seconds
- Voice: Deep, dramatic, "Trickster" tone
- Language: Tamil/Hinglish/Telugu mix
- Content: One of these phrases delivered dramatically:
  - "Oru tea potta dhan sariya irukkum da! 🍵"
  - "Boss, cutting chai time ba!"
  - "Da, chai break venam?"
  - "Infinity loop? Chai loop da!"
  - "Coding se tired? Chai lo!"
- File size: 300KB - 800KB

**Why this timing is IMPORTANT:**
- Glass placed on screen ➜ Dialogue plays ➜ Maximum meme dopamine
- Dialogue plays ONLY ONCE (not looped) so repeated idle isn't annoying
- Creates perfect sync: visual + audio + text

**How to record:**
1. Use voice acting software or AI voice generation
2. Record dramatic/funny tone (Trickster style)
3. Add slight reverb for "desktop speaker" effect
4. Normalize audio to -3dB (leave headroom)

**AI Voice Services:**
- Eleven Labs - Create custom cloned voices
- Play.ht - Natural sounding TTS with emotion
- Google Cloud Text-to-Speech - Indian English voices
- Microsoft Azure Speech Services

**Example filename:** `tea_dialogue.mp3`

---

### 4. `tea.ico` - Tray Icon ☕
**Purpose:** Icon shown in Windows system tray (bottom-right)

**Specifications:**
- Format: ICO (Windows icon format)
- Dimensions: 256x256 pixels (will auto-scale to 16x16 for tray)
- File size: 50-100KB
- Colors: 32-bit RGBA (with transparency)
- Content: Tea cup/coffee cup emoji or simple illustration

**How to create:**
1. Design in Figma, Adobe XD, or Photoshop
2. Export as PNG (256x256)
3. Convert PNG to ICO online:
   - icoconvert.com
   - convertio.co
   - cloudconvert.com
4. Select "256x256" option during conversion

**Or use online icon generator:**
- Generate simple tea icon at canva.com
- Export and convert to ICO

**Alternative: Use existing tea icon:**
- Search "tea icon 256x256" on Flaticon or Iconfinder
- Download as PNG
- Convert to ICO

**Example filename:** `tea.ico`

---

## 🎬 Optional/Alternative Assets

### `juice.mp4` - Different Tea Type Animation
**For variety:** Show juice/lassi being poured instead of tea
- Can have 2-3 animation variants
- App randomly picks one per idle trigger

### `juice.mp3` - Alternative Pour Sound
**For variety:** Different liquid sound (juice pouring differently than tea)

### `filter_coffee.mp4` - Filter Coffee Animation
**Regional flavor:** Show coffee filter pour-over for South Indian style

---

## 📋 Asset Setup Checklist

- [ ] `tea.mp4` placed in this folder
- [ ] `pour.mp3` placed in this folder  
- [ ] `tea_dialogue.mp3` placed in this folder
- [ ] `tea.ico` placed in this folder
- [ ] All files are in correct format (MP4, MP3, ICO)
- [ ] All files under 50MB total (keeps .exe fast)
- [ ] Tested locally - sounds play correctly
- [ ] No copyright issues (use royalty-free content)

---

## 🔄 What Happens If Files Are Missing?

ChaiIdle gracefully handles missing assets:

| Asset | Missing? | Fallback |
|-------|----------|----------|
| `tea.mp4` | ✓ | 🍵 Animated emoji bounces |
| `pour.mp3` | ✓ | Silent mode (no sound) |
| `tea_dialogue.mp3` | ✓ | Silent mode (no dialogue) |
| `tea.ico` | ✓ | Generic system shield icon |

**The app always works**, with or without media files. Audio-visual assets are bonuses for virality!

---

## 🎯 Audio Timing Flow

```
┌─────────────────────────────────────────────────┐
│ User is idle for 5 minutes (default)           │
└──────────────┬──────────────────────────────────┘
               │
               ▼
┌─────────────────────────────────────────────────┐
│ Overlay window appears on screen (T=0ms)        │
│ ✓ Tea animation starts                          │
│ ✓ Pour sound (loop) starts                      │
│ ✓ Dialogue sound plays ONCE (0-100ms delay)    │
│ ✓ Text fades in with dialogue                   │
└──────────────┬──────────────────────────────────┘
               │
               ▼ (Dialogue ends ~3-4 sec)
┌─────────────────────────────────────────────────┐
│ Pour sound continues looping                    │
│ Animation continues looping                     │
│ User can dismiss by:                            │
│   - Moving mouse                                │
│   - Pressing any key                            │
│   - After 10 seconds (auto-close)              │
└─────────────────────────────────────────────────┘
```

---

## 🚀 Building with Assets

**The project automatically:**
1. ✅ Copies all files from `Assets/` to build output (`bin/Release/...`)
2. ✅ Includes assets in single-file `.exe` 
3. ✅ Makes assets accessible at runtime

**Build command:**
```bash
dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true
```

Result: Single `ChaiIdle.exe` with all media embedded!

---

## 📞 Quick Links

- **Stock Videos:** Pexels.com, Pixabay.com, Coverr.co
- **ASMR Sounds:** Freesound.org, Zapsplat.com, YouTube Audio Library
- **Voice AI:** eleven-labs.io, play.ht, google.com/cloud/text-to-speech
- **Icon Creator:** canva.com, figma.com, inkscape.org
- **Format Converters:** ffmpeg (video), pandoc (audio), CloudConvert (any format)

---

## 🎉 Pro Tips for Viral Success

1. **Dialogue is KEY:** Best dialogue = most shares
   - Make it funny, relatable, desi
   - Record in fun Trickster voice
   - Keep it short (2-3 sec max)

2. **ASMR Pouring Sound:** Satisfying audio = viral potential
   - Real recordings beat AI sounds
   - High-quality microphone recommended
   - People love realistic ASMR

3. **Animation Polish:** Smooth animation = professional feel
   - Use high FPS (30fps+)
   - Clean transitions
   - Transparent background for sleek look

4. **Consistency:** Same files across all builds
   - Assets folder always copied to output
   - Users always get full experience
   - No "missing media" complaints

---

**Da, assets ready ba! Now time to build that viral .exe! 🍵✨**
