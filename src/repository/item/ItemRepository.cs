using System.Collections.Generic;
using System.Linq;
using JsonExporter.data.wrapped.@object;
using Newtonsoft.Json;
using StardewValley;

namespace JsonExporter.repository.item
{
    public class ItemRepository : Repository<ItemRepository, WrappedObject>
    {
        private static readonly Dictionary<string, WrappedObject> Objects = new();
        
        [JsonProperty("objects")]  private static WrappedObject[] ObjectsAsArray => Objects.Values.ToArray();

        public override void Populate()
        {
            Objects.Clear();

            foreach (var itemId in Game1.objectInformation.Keys)
            {
                var sObject = new Object(itemId, 1); 
                var wItem = new WrappedObject(itemId, sObject);

                Objects.Add(wItem.Id, wItem);
            }
        }
        
        public override List<WrappedObject> GetAll()
        {
            return Objects.Values.ToList();
        }
    }
}