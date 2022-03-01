using System;
using System.Collections.Generic;
using System.Linq;
using JsonExporter.data.wrapped.npc;
using StardewValley;

namespace JsonExporter.repository.npc
{
    public class NpcRepository
    {
        private static NpcRepository _instance;

        private static readonly Dictionary<string, WrappedNpc> NPCs = new();

        public static NpcRepository GetInstance()
        {
            return _instance ??= new NpcRepository();
        }

        private void PopulateNPCs()
        {
            if (NPCs.Count != 0) return;

            foreach (var npcName in Game1.NPCGiftTastes.Keys)
            {
                if (npcName.StartsWith("Universal_")) continue;

                NPCs.Add(npcName, new WrappedNpc(new NPC
                {
                    Name = npcName
                }));
            }
        }

        public List<WrappedNpc> GetAll()
        {
            PopulateNPCs();

            return NPCs.Values.ToList();
        }

        public WrappedNpc GetByName(string name)
        {
            PopulateNPCs();

            return NPCs[name] ?? throw new ArgumentException($"no npc with name '{name}' found");
        }
    }
}