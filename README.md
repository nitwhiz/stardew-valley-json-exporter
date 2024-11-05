# Stardew Valley JSON Exporter

Export everything about the game as JSON!

## Usage

Download the mod zip from the releases page and extract it into your mods folder.

Start the game and enter `export_data` or `export_textures` in the SMAPI console.
You can do that before the menu screen is fully loaded.

## Commands

* `export_data` will export in-game data as JSON. The following data is supported:
  * Categories
  * Items
  * Item Names in every language available but Chinese, Thai, Korean or Modded
  * Npcs
  * Npc Names in every language available but Chinese, Thai, Korean or Modded
  * Gift Tastes
  * Recipes
* `export_textures` will export in-game textures as png files. The following textures are supported:
  * Items
  * Npcs

## Output

All exported data will be saved in a directory at `<Stardew Valley Path>/Mods/<JsonExporter Folder>/export/`.
There will be a folder for the data and one for the textures.

