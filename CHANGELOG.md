# Changelog

All notable changes to QuickGridLauncher will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [1.0.0] - 2025-02-23

### Added

- Initial release
- Global hotkey activation (Alt+Shift+Space)
- WASD keyboard navigation with wrap-around
- Grid-based application launcher overlay
- Customizable appearance (colors, opacity, columns)
- Customizable hotkey combinations
- Drag-and-drop app reordering in settings
- Windows startup option
- Multi-monitor support with active monitor detection
- Automatic cleanup of missing executables
- Persistent JSON configuration
- Scale animation on overlay entrance/exit
- Empty state message when no apps configured
- Application restart option from settings

### Technical

- Built on .NET 10 with WPF
- Zero external dependencies
- Self-contained deployment option
- Framework-dependent deployment option
- ~25-30MB RAM usage
- Icon caching for performance
