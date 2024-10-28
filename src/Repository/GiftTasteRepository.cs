using System;
using System.Collections.Generic;
using System.Linq;
using JsonExporter.Model;
using Newtonsoft.Json;

namespace JsonExporter.Repository;

public class GiftTasteRepository : Repository<GiftTasteRepository, GiftTaste>
{
    private static readonly Dictionary<string, GiftTaste> TastesByNpc = new();

    [JsonProperty("tastesByNpc")] private static GiftTaste[] TastesAsArray => TastesByNpc.Values.ToArray();

    [JsonProperty("version")] private static string _version = DateTime.Now.ToString("u");

    public override void Populate()
    {
        TastesByNpc.Clear();

        foreach (var npc in NpcRepository.GetInstance().GetAll()) TastesByNpc.Add(npc.Id, new GiftTaste(npc.Id));
    }

    public override List<GiftTaste> GetAll()
    {
        return TastesByNpc.Values.ToList();
    }
}