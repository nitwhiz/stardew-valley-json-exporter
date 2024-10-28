using System;
using System.Collections.Generic;
using System.Linq;
using JsonExporter.Model;
using JsonExporter.Util;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using StardewValley;
using Object = StardewValley.Object;

namespace JsonExporter.Repository;

public class ItemRepository : Repository<ItemRepository, WrappedObject>
{
    private static readonly Dictionary<string, WrappedObject> Objects = new();

    [JsonProperty("objects")] private static WrappedObject[] ObjectsAsArray => Objects.Values.ToArray();

    [JsonProperty("version")] private static string _version = DateTime.Now.ToString("u");

    public override void Populate()
    {
        Objects.Clear();

        foreach (var itemId in Game1.objectData.Keys)
        {
            var sObject = new Object(itemId, 1);
            var wItem = new WrappedObject(sObject);

            Objects[wItem.Id] = wItem;
        }

        foreach (var itemId in Game1.bigCraftableData.Keys)
        {
            var sObject = new Object(new Vector2(0, 0), itemId);
            var wItem = new WrappedObject(sObject);

            Objects[wItem.Id] = wItem;
        }

        TranslationHelper.TranslateAll(Objects.Values);
    }

    public WrappedObject GetById(string id)
    {
        return Objects.GetValueOrDefault(id, null);
    }

    public override List<WrappedObject> GetAll()
    {
        return Objects.Values.ToList();
    }
}