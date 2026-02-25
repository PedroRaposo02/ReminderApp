# ReminderApp

ReminderApp is a modern, lightweight, and perfectly-styled Windows desktop utility application built with C# and WPF. It lives quietly in your system tray and helps you manage recurring reminders with a beautiful, dark-themed Fluent UI.

![ReminderApp Screenshot](/Images/Screenshot.png)

## Features

- **System Tray Integration**: Quietly runs in the background and is accessible directly from your taskbar or system tray.
- **Modern WPF Tray Menu**: Features a sleek, custom-built dark tray context menu with drop shadows and rounded corners.
- **Fluent Dark Theme UI**: A beautifully crafted modal for managing your reminders, complete with:
  - Custom borderless title bar.
  - Hover states, perfectly aligned data columns, and visually pleasing square inputs.
  - Integration with high-quality Windows Segoe MDL2 iconography.
- **Smart Sizing Support**: Supports high-resolution system icons and dynamically scales to fit your System Tray, Taskbar, and File Explorer effortlessly.
- **Persistent Storage**: Survives application restarts through local JSON persistence.

## Getting Started

### Prerequisites
- Windows 10 or later.
- .NET 10.0 runtime (if fully-standalone executable is not provided)

### Installation

1. Go to the [Releases](#) tab of this repository.
2. Download the latest `ReminderApp.zip` file.
3. Extract the contents of the ZIP file to any folder on your computer.
4. Execute `ReminderApp.exe` inside the extracted folder. There is no formal installation required. You can simply create a desktop shortcut to this file or add the shortcut to your Windows Startup folder.

### Building from Source

If you prefer to compile from source:

1. Clone the repository.
2. Open Powershell or CMD in the repository directory.
3. Run the following command:
```powershell
dotnet run --project ReminderApp
```

To build a release package:
```powershell
dotnet publish -c Release
```
This will compile the application in `bin/Release/net10.x/publish/`. You can copy the contents of that folder anywhere to use the app!

## Setup & Usage

- **Adding Reminders**: Simply click on the "New Reminder" button in the app, type in your description, and specify the interval for when you would like to be reminded.
- **Managing Reminders**: Use the play/skip icons to restart the reminder or the edit icon to change its interval and description. 

## License

MIT License. See `LICENSE.txt` for more details.
