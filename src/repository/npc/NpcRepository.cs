using System.Collections.Generic;
using System.Linq;
using JsonExporter.data.wrapped.npc;
using Newtonsoft.Json;
using StardewValley;

namespace JsonExporter.repository.npc
{
    public class NpcRepository : Repository<NpcRepository, WrappedNpc>
    {
        [JsonProperty("npcs")] private static readonly Dictionary<string, WrappedNpc> Npcs = new();

        private static readonly Dictionary<string, string> NameToId = new();
        
        public override void Populate()
        {
            Npcs.Clear();

            foreach (var npcName in Game1.NPCGiftTastes.Keys)
            {
                if (npcName.StartsWith("Universal_"))
                {
                    continue;
                }

                var wNpc = new WrappedNpc(new NPC
                {
                    Name = npcName
                });

                NameToId.Add(wNpc.Name, wNpc.Id);
                Npcs.Add(wNpc.Id, wNpc);
            }
        }

        public override List<WrappedNpc> GetAll()
        {
            return Npcs.Values.ToList();
        }

        public WrappedNpc GetByName(string npcName)
        {
            return Npcs[NameToId[npcName]];
        }
    }
}