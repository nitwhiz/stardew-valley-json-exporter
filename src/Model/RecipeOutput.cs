using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using StardewValley;

namespace JsonExporter.Model;

[JsonObject(MemberSerialization.OptIn)]
public class RecipeOutput
{
    [JsonProperty("itemIds")] public List<string> ItemIds;

    [JsonProperty("amount")] public int Amount;

    public RecipeOutput(List<string> itemIds, int amount)
    {
        ItemIds = itemIds.Select(ItemRegistry.QualifyItemId).ToList();
        Amount = amount;
    }
}