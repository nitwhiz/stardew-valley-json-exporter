using System.Linq;
using JsonExporter.Repository;
using Newtonsoft.Json;
using StardewValley;

namespace JsonExporter.Model;

[JsonObject(MemberSerialization.OptIn)]
public class Ingredient
{
    [JsonProperty("itemId")] public string ItemId;

    [JsonProperty("quantity")] public int Quantity;

    public Ingredient(string itemId, int quantity)
    {
        if (itemId.StartsWith("-"))
            ItemId = itemId;
        else
            ItemId = ItemRegistry.QualifyItemId(itemId);

        Quantity = quantity;
    }
}