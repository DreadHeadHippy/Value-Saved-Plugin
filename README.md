# Value Saved Plugin for Playnite

This plugin allows you to input the original price for each game you added for free, and it tallies the total value saved across all games.

## Features
- Set original price for the currently selected game.
- View total value saved (sum of all original prices).
- Prices are saved persistently.

## Build Instructions
1. Open the folder in Visual Studio or VS Code.
2. Make sure Playnite.SDK.dll is referenced (copy from your Playnite install folder if needed).
3. Build the project in Release mode.
4. Copy the built DLL (`ValueSavedPlugin.dll`) to Playnite's `Extensions` folder (e.g., `%AppData%\Playnite\Extensions\ValueSavedPlugin\`).

## Usage
- In Playnite, select a game.
- Go to the main menu > @Value Saved > Set Original Price for Selected Game.
- Enter the original price.
- To view total saved, go to main menu > @Value Saved > Show Total Value Saved.

---
This is a simple plugin. Customize as needed.