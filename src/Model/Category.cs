using System.Collections.Generic;
using JsonExporter.Contract;
using Newtonsoft.Json;

namespace JsonExporter.Model;

[JsonObject(MemberSerialization.OptIn)]
public class Category : ITranslatable
{
    [JsonProperty("id")] public readonly int Id;

    [JsonProperty("displayNames")] public readonly Dictionary<string, string> DisplayNames = new();

    public Category(int id)
    {
        Id = id;
    }

    public void PopulateDisplayName(string code)
    {
        DisplayNames[code] = StardewValley.Object.GetCategoryDisplayName(Id);
    }
}