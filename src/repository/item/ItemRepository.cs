using System.Collections.Generic;
using System.Linq;
using JsonExporter.data.wrapped.@object;
using StardewValley;

namespace JsonExporter.repository.item
{
    public class ItemRepository
    {
        private static ItemRepository _instance;

        private static readonly Dictionary<int, WrappedObject> Objects = new();

        public static ItemRepository GetInstance()
        {
            return _instance ??= new ItemRepository();
        }

        public List<WrappedObject> GetAll()
        {
            if (Objects.Count == 0)
                foreach (var itemId in Game1.objectInformation.Keys)
                    Objects.Add(itemId, new WrappedObject(new Object(itemId, 1)));

            return Objects.Values.ToList();
        }

        public WrappedObject GetById(int itemId)
        {
            if (Objects.ContainsKey(itemId)) return Objects[itemId];

            return null;
        }
    }
}