using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using StardewValley;
using StardewObject = StardewValley.Object;

namespace JsonExporter.data
{
    [JsonObject(MemberSerialization.OptIn)]
    public class WrappedObject
    {
        [JsonProperty("id")] public readonly string Id;
        
        [JsonProperty("displayNames")] public readonly Dictionary<string, string> DisplayNames = new();

        [JsonProperty("type")] public readonly string Type;

        [JsonProperty("category")] public readonly int Category;

        public readonly StardewObject wrappedObject;

        public WrappedObject(StardewObject obj)
        {
            wrappedObject = obj;
            
            Id = wrappedObject.QualifiedItemId;
            Type = wrappedObject.Type.ToLower();
            Category = wrappedObject.Category;

            var languageCodes =
                (LocalizedContentManager.LanguageCode[]) Enum.GetValues(typeof(LocalizedContentManager.LanguageCode));

            foreach (var languageCode in languageCodes)
            {
                if (
                    languageCode is LocalizedContentManager.LanguageCode.zh or LocalizedContentManager.LanguageCode.th
                    or LocalizedContentManager.LanguageCode.ko or LocalizedContentManager.LanguageCode.mod
                )
                {
                    // skip some languages
                    continue;
                }

                try
                {
                    LocalizedContentManager.CurrentLanguageCode = languageCode;
                    Game1.game1.TranslateFields();

                    DisplayNames.Add(Enum.GetName(typeof(LocalizedContentManager.LanguageCode), languageCode) ?? "none",
                        wrappedObject.DisplayName);
                }
                catch { }
            }
        }

        public void SaveTexture(string basePath)
        {
            var rTarget = new RenderTarget2D(Game1.graphics.GraphicsDevice, 64, 64, false,
                SurfaceFormat.Color, DepthFormat.Depth24, 0, RenderTargetUsage.DiscardContents);

            var gd = Game1.graphics.GraphicsDevice;

            gd.SetRenderTarget(rTarget);

            var s = new SpriteBatch(gd);

            s.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
            gd.Clear(Color.Transparent);
            wrappedObject.drawInMenu(s, Vector2.Zero, 1f, 1f, 1f, StackDrawType.Hide, Color.White, false);
            s.End();

            Directory.CreateDirectory(Path.Combine(basePath, "textures/items"));

            var stream = File.Create(Path.Combine(basePath, "textures/items", Id + ".png"));

            rTarget.SaveAsPng(stream, rTarget.Width, rTarget.Height);

            stream.Dispose();
        }
    }
}