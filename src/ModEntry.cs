using System.IO;
using JsonExporter.Repository;
using JsonExporter.Util;
using StardewModdingAPI;

namespace JsonExporter;

public class ModEntry : Mod
{
    public override void Entry(IModHelper helper)
    {
        var basePath = Path.Combine(helper.DirectoryPath, "export");

        if (Directory.Exists(basePath)) Directory.Delete(basePath, true);

        Directory.CreateDirectory(basePath);

        helper.ConsoleCommands.Add("export_textures", "export all textures", (cmd, args) =>
        {
            Monitor.Log("exporting textures...", LogLevel.Info);

            Monitor.Log("portraits ...", LogLevel.Info);
            foreach (var npc in NpcRepository.GetInstance().GetAll()) npc.SaveTexture(basePath);

            Monitor.Log("items ...", LogLevel.Info);
            foreach (var item in ItemRepository.GetInstance().GetAll()) item.SaveTexture(basePath);

            Monitor.Log("done!", LogLevel.Info);
        });

        helper.ConsoleCommands.Add("export_data", "export all data", (cmd, args) =>
        {
            Monitor.Log("exporting data...", LogLevel.Info);

            Monitor.Log("npcs ...", LogLevel.Info);
            NpcRepository.GetInstance().ExportJson(basePath, "npcs");

            Monitor.Log("items ...", LogLevel.Info);
            ItemRepository.GetInstance().ExportJson(basePath, "items");

            Monitor.Log("categories ...", LogLevel.Info);
            CategoryRepository.GetInstance().ExportJson(basePath, "categories");

            Monitor.Log("gift tastes ...", LogLevel.Info);
            GiftTasteRepository.GetInstance().ExportJson(basePath, "gift-tastes-by-npc");

            Monitor.Log("recipes ...", LogLevel.Info);
            RecipeRepository.GetInstance().ExportJson(basePath, "recipes");

            TranslationHelper.Reset();

            Monitor.Log("done!", LogLevel.Info);
        });
    }
}