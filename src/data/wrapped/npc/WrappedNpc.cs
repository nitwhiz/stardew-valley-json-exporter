using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using StardewValley;

namespace JsonExporter.data.wrapped.npc
{
    public class WrappedNpc : WrappedInstance<NPC>
    {
        [JsonProperty("displayNames")] public readonly Dictionary<string, string> DisplayNames = new();

        [JsonProperty("birthdaySeason")] public readonly string BirthdaySeason;
        
        [JsonProperty("birthdayDay")] public readonly int BirthdayDay;

        public readonly string Name;

        public readonly string TextureName;

        public WrappedNpc(NPC npc) : base(npc)
        {
            Name = npc.displayName;
            BirthdaySeason = npc.Birthday_Season;
            BirthdayDay = npc.Birthday_Day;

            var languageCodes =
                (LocalizedContentManager.LanguageCode[]) Enum.GetValues(typeof(LocalizedContentManager.LanguageCode));

            foreach (var languageCode in languageCodes)
            {
                try
                {
                    LocalizedContentManager.localizedAssetNames.Clear();

                    DisplayNames.Add(Enum.GetName(typeof(LocalizedContentManager.LanguageCode), languageCode) ?? "none",
                        translateName(npc.Name, languageCode));
                }
                catch { }
            }

            LocalizedContentManager.CurrentLanguageCode = LocalizedContentManager.LanguageCode.en;

            TextureName = npc.getTextureName();
        }
        
        private void parseStringPath(string path, out string assetName, out string key)
        {
            int length = path.IndexOf(':');
            assetName = length != -1
                ? path.Substring(0, length)
                : throw new Exception("Unable to parse string path: " + path);
            key = path.Substring(length + 1, path.Length - length - 1);
        }

        private string GetString(Dictionary<string, string> strings, string key)
        {
            string str;
            return strings.TryGetValue(key + ".desktop", out str) ? str : strings[key];
        }

        private string LoadString(string path, LocalizedContentManager.LanguageCode languageCode)
        {
            string assetName;
            string key;

            parseStringPath(path, out assetName, out key);

            Dictionary<string, string>
                strings = Game1.content.Load<Dictionary<string, string>>(assetName, languageCode);

            if (strings == null || !strings.ContainsKey(key))
                return Game1.content.LoadBaseString(path);

            string str = GetString(strings, key);

            if (str.Contains("¦"))
                str = !Game1.player.IsMale ? str.Substring(str.IndexOf("¦") + 1) : str.Substring(0, str.IndexOf("¦"));

            return str;
        }

        private string translateName(string name, LocalizedContentManager.LanguageCode languageCode)
        {
            switch (name)
            {
                case "Bear":
                    return LoadString("Strings\\NPCNames:Bear", languageCode);
                case "Birdie":
                    return LoadString("Strings\\NPCNames:Birdie", languageCode);
                case "Bouncer":
                    return LoadString("Strings\\NPCNames:Bouncer", languageCode);
                case "Gil":
                    return LoadString("Strings\\NPCNames:Gil", languageCode);
                case "Governor":
                    return LoadString("Strings\\NPCNames:Governor", languageCode);
                case "Grandpa":
                    return LoadString("Strings\\NPCNames:Grandpa", languageCode);
                case "Gunther":
                    return LoadString("Strings\\NPCNames:Gunther", languageCode);
                case "Henchman":
                    return LoadString("Strings\\NPCNames:Henchman", languageCode);
                case "Kel":
                    return LoadString("Strings\\NPCNames:Kel", languageCode);
                case "Marlon":
                    return LoadString("Strings\\NPCNames:Marlon", languageCode);
                case "Mister Qi":
                    return LoadString("Strings\\NPCNames:MisterQi", languageCode);
                case "Morris":
                    return LoadString("Strings\\NPCNames:Morris", languageCode);
                case "Old Mariner":
                    return LoadString("Strings\\NPCNames:OldMariner", languageCode);
                case "Welwick":
                    return LoadString("Strings\\NPCNames:Welwick", languageCode);
                default:
                    Dictionary<string, string> dictionary =
                        Game1.content.Load<Dictionary<string, string>>("Data\\NPCDispositions", languageCode);
                    if (!dictionary.ContainsKey(name))
                        return name;
                    string[] strArray = dictionary[name].Split('/');
                    return strArray[strArray.Length - 1];
            }
        }

        public void SaveTexture(string basePath)
        {
            Directory.CreateDirectory(Path.Combine(basePath, "textures/portraits"));

            var rTarget = new RenderTarget2D(Game1.graphics.GraphicsDevice, 64, 64, false,
                SurfaceFormat.Color, DepthFormat.Depth24, 0, RenderTargetUsage.DiscardContents);

            var gd = Game1.graphics.GraphicsDevice;

            gd.SetRenderTarget(rTarget);

            var s = new SpriteBatch(gd);

            s.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
            gd.Clear(Color.Transparent);
            s.Draw(Original.Portrait, Vector2.Zero, new Rectangle(0, 0, 64, 64), Color.White);
            s.End();

            Stream stream = File.Create(Path.Combine(basePath, "textures/portraits", Id + ".png"));
            rTarget.SaveAsPng(stream, rTarget.Width, rTarget.Height);

            stream.Dispose();
        }
    }
}