using System.Collections.Generic;
using System.Linq;
using JsonExporter.data.gift;
using JsonExporter.repository.npc;

namespace JsonExporter.repository.gift
{
    public class GiftTasteRepository
    {
        private static readonly Dictionary<string, GiftTaste> Tastes = new();

        private static GiftTasteRepository _instance;

        public static GiftTasteRepository GetInstance()
        {
            return _instance ??= new GiftTasteRepository();
        }

        public List<GiftTaste> GetAll()
        {
            if (Tastes.Count == 0)
                foreach (var npc in NpcRepository.GetInstance().GetAll())
                    Tastes.Add(npc.Name, new GiftTaste(npc.Name));

            return Tastes.Values.ToList();
        }
    }
}