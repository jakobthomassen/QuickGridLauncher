# QuickGridLauncher

A lightweight, keyboard-driven application launcher for Windows with zero dependencies.

[![Downloads](https://img.shields.io/github/downloads/[YOUR-USERNAME]/QuickGridLauncher/total)](https://github.com/[YOUR-USERNAME]/QuickGridLauncher/releases)
[![Latest Release](https://img.shields.io/github/v/release/[YOUR-USERNAME]/QuickGridLauncher)](https://github.com/[YOUR-USERNAME]/QuickGridLauncher/releases/latest)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![OpenSSF Scorecard](https://api.securityscorecards.dev/projects/github.com/[YOUR-USERNAME]/QuickGridLauncher/badge)](https://securityscorecards.dev/viewer/?uri=github.com/[YOUR-USERNAME]/QuickGridLauncher)

![QuickGridLauncher Demo](https://via.placeholder.com/800x400.png?text=Add+Demo+Screenshot+Here)

## ✨ Features

- ⌨️ **Global Hotkey** - Launch instantly from anywhere (Default: Alt+Shift+Space)
- 🎯 **WASD Navigation** - Fast keyboard-driven grid navigation with wrap-around
- 🎨 **Full Customization** - Colors, opacity, columns, and hotkey combinations
- 🖱️ **Drag-and-Drop** - Reorder apps easily in settings
- 💾 **Persistent Config** - All settings saved automatically
- 🖥️ **Multi-Monitor** - Opens on your active monitor
- 🚀 **Zero Dependencies** - No external libraries, pure WPF
- 🔄 **Auto-Cleanup** - Missing apps removed automatically
- ⚡ **Lightweight** - ~70MB self-contained, ~25-30MB RAM usage

## 📥 Installation

### Option 1: Download Release (Recommended)

1. Go to [Releases](https://github.com/[YOUR-USERNAME]/QuickGridLauncher/releases/latest)
2. Download **QuickGridLauncher-win-x64.exe** (recommended - works on any Windows 10+)
   - _Or_ **QuickGridLauncher-fd.exe** if you have [.NET 10](https://dotnet.microsoft.com/download) installed
3. Run the executable
4. If Windows SmartScreen appears, click **"More info"** → **"Run anyway"**

### Option 2: Build from Source

```bash
git clone https://github.com/[YOUR-USERNAME]/QuickGridLauncher.git
cd QuickGridLauncher
dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true /p:PublishTrimmed=true
```

The executable will be at: `bin\Release\net10.0-windows\win-x64\publish\QuickGridLauncher.exe`

## 🎮 Usage

### Quick Start

1. **Launch:** Press your hotkey (default: **Alt+Shift+Space**)
2. **Navigate:** Use **WASD** keys to move between apps
3. **Launch App:** Press **Enter** or **Space**
4. **Settings:** Press **Q** to configure
5. **Close:** Press **Esc** or click anywhere outside

### Keyboard Controls

| Key               | Action              |
| ----------------- | ------------------- |
| `W`               | Move up             |
| `A`               | Move left           |
| `S`               | Move down           |
| `D`               | Move right          |
| `Enter` / `Space` | Launch selected app |
| `Q`               | Open settings       |
| `Esc`             | Close overlay       |

### Settings

- **Appearance:** Customize grid columns, background color/opacity, highlight color
- **Hotkey:** Change modifier keys (Alt/Ctrl/Shift) and trigger key
- **Startup:** Enable/disable run on Windows startup
- **Apps:** Add, remove, and reorder applications via drag-and-drop

## 🛡️ About SmartScreen Warnings

Windows SmartScreen may warn that this app is from an "unknown publisher" because:

1. **Code signing certificates cost $100-400/year** - Not sustainable for free open source
2. **This is normal for open-source apps** - Thousands of projects work this way
3. **The code is 100% transparent** - Every line is reviewable in this repository
4. **Built by GitHub Actions** - All builds are automated and auditable

**To bypass the warning:**

- Click **"More info"** → **"Run anyway"** (one-time action)
- Or right-click the .exe → **Properties** → Check **"Unblock"** → **Apply**

**For maximum security:**

- Review the source code yourself
- Build from source using the instructions above
- Check the [GitHub Actions build logs](https://github.com/[YOUR-USERNAME]/QuickGridLauncher/actions)

## 🏗️ Tech Stack

- **.NET 10** - Latest .NET framework
- **WPF** - Windows Presentation Foundation for UI
- **Win32 API** - Global hotkey registration and multi-monitor support
- **JSON** - Simple configuration persistence

## 🤝 Contributing

Contributions welcome! Please:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📝 Changelog

See [CHANGELOG.md](CHANGELOG.md) for version history.

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments

- Inspired by launchers like [Wox](https://github.com/Wox-launcher/Wox), [Flow Launcher](https://github.com/Flow-Launcher/Flow.Launcher), and [PowerToys Run](https://github.com/microsoft/PowerToys)
- Built with ❤️ for keyboard-driven productivity

## 📬 Support

- 🐛 **Bug Reports:** [Open an issue](https://github.com/[YOUR-USERNAME]/QuickGridLauncher/issues/new?template=bug_report.md)
- 💡 **Feature Requests:** [Open an issue](https://github.com/[YOUR-USERNAME]/QuickGridLauncher/issues/new?template=feature_request.md)
- 💬 **Discussions:** [GitHub Discussions](https://github.com/[YOUR-USERNAME]/QuickGridLauncher/discussions)

---

**Note:** Replace `[YOUR-USERNAME]` with your GitHub username throughout this file.
