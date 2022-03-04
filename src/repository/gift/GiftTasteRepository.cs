using System.Collections.Generic;
using System.Linq;
using JsonExporter.data.gift;
using JsonExporter.repository.npc;
using Newtonsoft.Json;

namespace JsonExporter.repository.gift
{
    public class GiftTasteRepository : Repository<GiftTasteRepository, GiftTaste>
    {
        [JsonProperty("tastesByNpc")] private static readonly Dictionary<string, GiftTaste> TastesByNpc = new();

        public override void Populate()
        {
            TastesByNpc.Clear();
            
            foreach (var npc in NpcRepository.GetInstance().GetAll())
            {
                TastesByNpc.Add(npc.Id, new GiftTaste(npc.Name));
            }
        }

        public override List<GiftTaste> GetAll()
        {
            return TastesByNpc.Values.ToList();
        }
    }
}