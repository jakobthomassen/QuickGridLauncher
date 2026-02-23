# Changelog

All notable changes to QuickGridLauncher will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [1.0.1] - 2025-02-23

### Changed
- Simplified release builds to single self-contained executable
- Removed framework-dependent build (was not working correctly)
- Optimized build process for better reliability
- Now uses `Environment.ProcessPath` for reliable executable path detection
- Improved error messages when restart fails

### Fixed
- Corrected build workflow to target .csproj instead of .sln
- Disabled trimming for Windows Forms compatibility
- Fixed executable size issues (now ~150MB for self-contained build)
- Restart application functionality now works correctly in single-file deployments
- Windows startup registry feature now works correctly in single-file deployments
- Resolved IL3000 warnings about Assembly.Location in single-file apps

### Technical
- Added explicit `/p:PublishTrimmed=false` flag
- Added `/p:IncludeNativeLibrariesForSelfExtract=true` for proper single-file packaging
- Pinned GitHub Actions to specific commit SHAs for security

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
