# Days Counter Overlay

A minimal, low-resource Windows overlay that displays the number of days since a target date (e.g., Dec 18, 2025).

## Features
- **Auto-Overlay**: Starts full-screen for 5 minutes, then shrinks to a mini-widget.
- **Low Resource**: Native .NET 8 WPF application with minimal RAM/CPU usage.
- **Customizable**:
  - Drag and move the widget anywhere.
  - Change Text Color, Font Size, and Overlay Size.
  - Set Custom Target Date.
  - Toggle detailed Time Mode (Days + HH:mm:ss).
- **System Tray**: Access settings or exit via the tray icon.

## Installation
1. Go to the [Releases](#) page (not yet published).
2. Download `DaysCounter.exe`.
3. Run it.

## Usage
- **Right-Click Tray Icon**: Open **Settings** or **Exit**.
- **Drag**: Click and hold the text in mini-mode to move it.

## Build from Source
1. Install .NET 8 SDK.
2. Clone repository:
   ```bash
   git clone https://github.com/ramaseshanms/days_counter.git
   ```
3. Build:
   ```bash
   dotnet build -c Release
   ```
