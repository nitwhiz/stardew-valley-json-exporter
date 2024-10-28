using System;
using System.Collections.Generic;
using System.Linq;
using JsonExporter.Model;
using JsonExporter.Util;
using Newtonsoft.Json;
using StardewValley;

namespace JsonExporter.Repository;

public class NpcRepository : Repository<NpcRepository, WrappedNpc>
{
    private static readonly Dictionary<string, WrappedNpc> Npcs = new();

    [JsonProperty("npcs")] private static WrappedNpc[] NpcsAsArray => Npcs.Values.ToArray();

    [JsonProperty("version")] private static string _version = DateTime.Now.ToString("u");

    public override void Populate()
    {
        Npcs.Clear();

        foreach (var npcName in Game1.NPCGiftTastes.Keys)
        {
            if (npcName.StartsWith("Universal_")) continue;

            var npc = new NPC
            {
                Name = npcName
            };

            npc.reloadData();

            var wNpc = new WrappedNpc(npc);

            Npcs[wNpc.Id] = wNpc;
        }

        TranslationHelper.TranslateAll(Npcs.Values);
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