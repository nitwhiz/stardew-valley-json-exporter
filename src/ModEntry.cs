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
            helper.ConsoleCommands.Add("export", "export all data", (cmd, args) =>
            {
                Monitor.Log("exporting gift tastes", LogLevel.Info);

                var giftTastes = GiftTasteRepository.GetInstance().GetAll();
                helper.Data.WriteJsonFile("data/gift-tastes.json", giftTastes);

                Monitor.Log("exporting npcs", LogLevel.Info);

                var npcs = NpcRepository.GetInstance().GetAll();
                helper.Data.WriteJsonFile("data/npcs.json", npcs);

                npcs.ForEach(npc =>
                {
                    Monitor.Log($"exporting portrait for {npc.Name}", LogLevel.Info);

                    npc.SavePortrait(helper.DirectoryPath);
                });

                ItemRepository.GetInstance().GetAll().ForEach(item =>
                {
                    Monitor.Log($"exporting texture for {item.DisplayName}", LogLevel.Info);

                    item.SaveTexture(helper.DirectoryPath);
                });

                Monitor.Log("done!", LogLevel.Info);
            });
        }
    }
}