# ☕ OruTeaDa (ChaiIdle)

**OruTeaDa** is a premium, meme-inspired idle reminder for developers. It monitors your system activity and gently (or humorously) reminds you to take a chai/tea break when you've been inactive for too long. 

Built with **WPF (.NET 8)**, it features a modern glassmorphism UI, smooth animations, and language-aware audio dialogues.

---

## ✨ Features

- **💎 Modern UI**: Frosted glass (glassmorphism) design with a dark, premium aesthetic.
- **🎨 Custom Animations**: Hand-drawn SVG tea cup with animated steam and ripple effects.
- **🌍 Multi-language Dialogues**: Supports Tamil, English, and Hinglish memes and reminders.
- **🎵 Smart Audio**: Intelligent audio rotation that picks random clips based on your preferred language.
- **⏰ Customizable Idle Timer**: Set your own break threshold from 1 to 30 minutes.
- **🚀 Single-File Portability**: Runs as a self-contained executable with no installation required.

---

## 🛠️ Installation & Usage

1. **Download**: Grab the latest version from the [Releases](https://github.com/ResolverVicky/ChaiIdle/releases) page.
2. **Run**: Extract the ZIP and double-click `OruTeaDa.exe`.
3. **Tray Menu**: The app lives in your system tray. Right-click the ☕ icon to:
   - Open **Settings** to adjust idle time and language.
   - **Pause/Resume** the idle detector.
   - **Exit** the application.

---

## 🎵 Customizing Audio

You can add your own audio clips to make the reminders even more personal!

- **Folder Structure**:
  ```
  Assets/
  └── Audio/
      ├── Tamil/    <-- Add Tamil .mp3 files here
      ├── English/  <-- Add English .mp3 files here
      └── Hinglish/ <-- Add Hinglish .mp3 files here
  ```
- **How it works**: The app will randomly pick an MP3 from the folder matching your selected language.

---

## 🏗️ Development

### Prerequisites
- .NET 8.0 SDK

### Build & Run
```powershell
# Clone the repo
git clone https://github.com/ResolverVicky/ChaiIdle.git
cd ChaiIdle

# Run the app
dotnet run
```

### Create a Release
Use the provided script to generate a single-file ZIP package:
```powershell
.\release.ps1 -Version "1.0.0"
```

---

## 📜 License
MIT License. Feel free to use, modify, and share!

---
Made with ❤️ and plenty of ☕ by **ResolverVicky**.
