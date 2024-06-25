using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using StardewValley;

namespace JsonExporter.data;

[JsonObject(MemberSerialization.OptIn)]
public class WrappedNpc
{
    [JsonProperty("birthdayDay")] public readonly int BirthdayDay;

    [JsonProperty("birthdaySeason")] public readonly string BirthdaySeason;

    [JsonProperty("displayNames")] public readonly Dictionary<string, string> DisplayNames = new();

    [JsonProperty("id")] public readonly string Id;

    public readonly NPC InnerNpc;

    public WrappedNpc(NPC npc)
    {
        InnerNpc = npc;

        Id = npc.id.ToString();
        BirthdaySeason = npc.Birthday_Season;
        BirthdayDay = npc.Birthday_Day;

        var languageCodes =
            (LocalizedContentManager.LanguageCode[])Enum.GetValues(typeof(LocalizedContentManager.LanguageCode));

        foreach (var languageCode in languageCodes)
        {
            if (
                    languageCode is LocalizedContentManager.LanguageCode.zh or LocalizedContentManager.LanguageCode.th
                    or LocalizedContentManager.LanguageCode.ko or LocalizedContentManager.LanguageCode.mod
                )
                // skip some languages
                continue;

            try
            {
                LocalizedContentManager.CurrentLanguageCode = languageCode;
                Game1.game1.TranslateFields();

                DisplayNames.Add(Enum.GetName(typeof(LocalizedContentManager.LanguageCode), languageCode) ?? "none",
                    NPC.GetDisplayName(npc.Name));
            }
            catch
            {
            }
        }
    }

    public void SaveTexture(string basePath)
    {
        InnerNpc.TryLoadPortraits("Portraits\\" + InnerNpc.getTextureName(), out var error);

        var portrait = InnerNpc.Portrait;

        if (portrait == null) return;

        Directory.CreateDirectory(Path.Combine(basePath, "textures/portraits"));

        var rTarget = new RenderTarget2D(Game1.graphics.GraphicsDevice, 64, 64, false,
            SurfaceFormat.Color, DepthFormat.Depth24, 0, RenderTargetUsage.DiscardContents);

        var gd = Game1.graphics.GraphicsDevice;

        gd.SetRenderTarget(rTarget);

        var s = new SpriteBatch(gd);

        s.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
        gd.Clear(Color.Transparent);
        s.Draw(InnerNpc.Portrait, Vector2.Zero, new Rectangle(0, 0, 64, 64), Color.White);
        s.End();

        Stream stream = File.Create(Path.Combine(basePath, "textures/portraits", Id + ".png"));
        rTarget.SaveAsPng(stream, rTarget.Width, rTarget.Height);

        stream.Dispose();
    }
}