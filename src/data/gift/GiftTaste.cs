using System;
using System.Collections.Generic;
using JsonExporter.repository.item;
using JsonExporter.repository.npc;
using Newtonsoft.Json;
using StardewValley;

namespace JsonExporter.data.gift
{
    [JsonObject(MemberSerialization.OptIn)]
    public class GiftTaste
    {
        [JsonProperty("dislikeItems")] public readonly List<string> DislikeItems = new();

        [JsonProperty("hateItems")] public readonly List<string> HateItems = new();

        [JsonProperty("likeItems")] public readonly List<string> LikeItems = new();

        [JsonProperty("loveItems")] public readonly List<string> LoveItems = new();

        [JsonProperty("neutralItems")] public readonly List<string> NeutralItems = new();

        public GiftTaste(string npcName)
        {
            ItemRepository.GetInstance().GetAll().ForEach(item =>
            {
                int taste;

                try
                {
                    taste = NpcRepository.GetInstance().GetByName(npcName).Original
                        .getGiftTasteForThisItem(item.Original);
                }
                catch (Exception)
                {
                    return;
                }

                switch (taste)
                {
                    case NPC.gift_taste_love:
                        LoveItems.Add(item.Id);
                        break;
                    case NPC.gift_taste_like:
                        LikeItems.Add(item.Id);
                        break;
                    case NPC.gift_taste_dislike:
                        DislikeItems.Add(item.Id);
                        break;
                    case NPC.gift_taste_hate:
                        HateItems.Add(item.Id);
                        break;
                    case NPC.gift_taste_neutral:
                        NeutralItems.Add(item.Id);
                        break;
                }
            });
        }
    }
}