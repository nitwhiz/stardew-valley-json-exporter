using System.IO;
using JsonExporter.repository.gift;
using JsonExporter.repository.item;
using JsonExporter.repository.npc;
using StardewModdingAPI;

namespace JsonExporter
{
    public class ModEntry : Mod
    {
        public override void Entry(IModHelper helper)
        {
            var basePath = Path.Combine(helper.DirectoryPath, "export");

            if (Directory.Exists(basePath))
            {
                Directory.Delete(basePath, true);
            }

            Directory.CreateDirectory(basePath);

            helper.ConsoleCommands.Add("export", "export all data", (cmd, args) =>
            {
                Monitor.Log("exporting...", LogLevel.Info);

                Monitor.Log("npcs", LogLevel.Info);
                NpcRepository.GetInstance().ExportJson(basePath, "npcs");

                Monitor.Log("items", LogLevel.Info);
                ItemRepository.GetInstance().ExportJson(basePath, "items");

                Monitor.Log("gift tastes", LogLevel.Info);
                GiftTasteRepository.GetInstance().ExportJson(basePath, "gift-tastes-by-npc");

                Monitor.Log("portrait textures", LogLevel.Info);
                foreach (var npc in NpcRepository.GetInstance().GetAll())
                {
                    npc.SaveTexture(basePath);
                }

                Monitor.Log("item textures");
                foreach (var item in ItemRepository.GetInstance().GetAll())
                {
                    item.SaveTexture(basePath);
                }

                Monitor.Log("done!", LogLevel.Info);
            });
        }
    }
}