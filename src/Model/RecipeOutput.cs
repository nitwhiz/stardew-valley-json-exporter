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

    public RecipeOutput(CraftingRecipe recipe)
    {
        ItemIds = recipe.itemToProduce.Select(itemId => recipe.bigCraftable ? ItemRegistry.ManuallyQualifyItemId(itemId, "(BC)") : ItemRegistry.QualifyItemId(itemId)).ToList();
        Amount = recipe.numberProducedPerCraft;
    }
}