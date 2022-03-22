using System.Collections.Generic;
using System.Linq;
using JsonExporter.data.wrapped.npc;
using Newtonsoft.Json;
using StardewValley;

namespace JsonExporter.repository.npc
{
    public class NpcRepository : Repository<NpcRepository, WrappedNpc>
    {
        private static readonly Dictionary<string, WrappedNpc> Npcs = new();

        [JsonProperty("npcs")] private static WrappedNpc[] NpcsAsArray => Npcs.Values.ToArray();

        public override void Populate()
        {
            Npcs.Clear();

            foreach (var npcName in Game1.NPCGiftTastes.Keys)
            {
                if (npcName.StartsWith("Universal_"))
                {
                    continue;
                }

                var npc = new NPC
                {
                    Name = npcName
                };
                
                npc.reloadData();

                var wNpc = new WrappedNpc(npc);

                Npcs.Add(wNpc.Id, wNpc);
            }
        }

        public override List<WrappedNpc> GetAll()
        {
            return Npcs.Values.ToList();
        }

        public WrappedNpc GetById(string npcId)
        {
            return Npcs[npcId];
        }
    }
}