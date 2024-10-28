using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JsonExporter.Model;
using JsonExporter.Util;
using Newtonsoft.Json;

namespace JsonExporter.Repository;

public class CategoryRepository : Repository<CategoryRepository, Category>
{
    private static readonly Dictionary<int, Category> Categories = new();

    [JsonProperty("categories")] private static Category[] ObjectsAsArray => Categories.Values.ToArray();

    [JsonProperty("version")] private static string _version = DateTime.Now.ToString("u");

    public override void Populate()
    {
        Categories.Clear();

        var fields = typeof(StardewValley.Object)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(field => field.IsLiteral && !field.IsInitOnly && field.FieldType == typeof(int))
            .Where(field => (int)field.GetValue(null) < 0)
            .ToList();

        foreach (var f in fields)
        {
            var val = (int)f.GetValue(null);
            Categories.Add(val, new Category(val));
        }

        TranslationHelper.TranslateAll(Categories.Values);
    }

    public override List<Category> GetAll()
    {
        return Categories.Values.ToList();
    }
}