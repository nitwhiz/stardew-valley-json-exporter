using System.Collections.Generic;
using System.Linq;
using JsonExporter.data;
using Newtonsoft.Json;

namespace JsonExporter.repository;

public class GiftTasteRepository : Repository<GiftTasteRepository, GiftTaste>
{
    private static readonly Dictionary<string, GiftTaste> TastesByNpc = new();

    [JsonProperty("tastesByNpc")] private static GiftTaste[] TastesAsArray => TastesByNpc.Values.ToArray();

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