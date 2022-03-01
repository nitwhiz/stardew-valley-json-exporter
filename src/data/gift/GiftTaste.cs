using System;
using System.Collections.Generic;
using JsonExporter.data.wrapped.@object;
using JsonExporter.repository.item;
using Newtonsoft.Json;
using StardewValley;

namespace JsonExporter.data.gift
{
    [JsonObject(MemberSerialization.OptIn)]
    public class GiftTaste
    {
        [JsonProperty] public readonly List<WrappedObject> DislikeItems = new();

        [JsonProperty] public readonly List<WrappedObject> HateItems = new();

        [JsonProperty] public readonly List<WrappedObject> LikeItems = new();

        [JsonProperty] public readonly List<WrappedObject> LoveItems = new();

        [JsonProperty] public readonly List<WrappedObject> NeutralItems = new();

        [JsonProperty] public readonly string NPCName;

        public GiftTaste(string npcName)
        {
            NPCName = npcName;

            var npc = new NPC
            {
                Name = npcName
            };

            ItemRepository.GetInstance().GetAll().ForEach(item =>
            {
                int taste;

                try
                {
                    taste = npc.getGiftTasteForThisItem(item.Original);
                }
                catch (Exception)
                {
                    return;
                }

                switch (taste)
                {
                    case NPC.gift_taste_love:
                        LoveItems.Add(item);
                        break;
                    case NPC.gift_taste_like:
                        LikeItems.Add(item);
                        break;
                    case NPC.gift_taste_dislike:
                        DislikeItems.Add(item);
                        break;
                    case NPC.gift_taste_hate:
                        HateItems.Add(item);
                        break;
                    case NPC.gift_taste_neutral:
                        NeutralItems.Add(item);
                        break;
                }
            });
        }
    }
}