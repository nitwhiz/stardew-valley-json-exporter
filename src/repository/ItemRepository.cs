using System.Collections.Generic;
using System.Linq;
using JsonExporter.data;
using Newtonsoft.Json;
using StardewValley;
using Object = StardewValley.Object;

namespace JsonExporter.repository
{
    public class ItemRepository : Repository<ItemRepository, WrappedObject>
    {
        private static readonly Dictionary<string, WrappedObject> Objects = new();
        
        [JsonProperty("objects")]  private static WrappedObject[] ObjectsAsArray => Objects.Values.ToArray();

        public override void Populate()
        {
            Objects.Clear();
            
            foreach (string itemId in Game1.objectData.Keys)
            {
                var obj = Game1.objectData[itemId];

                if (obj.CanBeGivenAsGift)
                {
                    var sObject = new Object(itemId, 1);
                    var wItem = new WrappedObject(sObject);
                    
                    Objects.Add(wItem.Id, wItem);
                }
            }
        }
        
        public override List<WrappedObject> GetAll()
        {
            return Objects.Values.ToList();
        }
    }
}