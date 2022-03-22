using System;
using System.Collections.Generic;
using JsonExporter.data.wrapped.npc;
using JsonExporter.repository.item;
using JsonExporter.repository.npc;
using Newtonsoft.Json;
using StardewValley;

namespace JsonExporter.data.gift
{
    [JsonObject(MemberSerialization.OptIn)]
    public class GiftTaste
    {
        [JsonProperty("npcId")] public string NpcId;

        [JsonProperty("dislikeItems")] public readonly List<string> DislikeItems = new();

        [JsonProperty("hateItems")] public readonly List<string> HateItems = new();

        [JsonProperty("likeItems")] public readonly List<string> LikeItems = new();

        [JsonProperty("loveItems")] public readonly List<string> LoveItems = new();

        [JsonProperty("neutralItems")] public readonly List<string> NeutralItems = new();

        public GiftTaste(string npcId)
        {
            ItemRepository.GetInstance().GetAll().ForEach(item =>
            {
                int taste;
                WrappedNpc npc;

                try
                {
                    npc = NpcRepository.GetInstance().GetById(npcId);

                    NpcId = npc.Id;

                    taste = npc.Original
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