using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using JsonExporter.Contract;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using StardewValley;

namespace JsonExporter.Model;

[JsonObject(MemberSerialization.OptIn)]
public class WrappedNpc : ITranslatable
{
    [JsonProperty("birthdayDay")] public readonly int BirthdayDay;

    [JsonProperty("birthdaySeason")] public readonly string BirthdaySeason;

    [JsonProperty("displayNames")] public readonly Dictionary<string, string> DisplayNames = new();

    [JsonProperty("id")] public readonly string Id;

    [JsonProperty("textureName")] public readonly string TexureName;

    public readonly NPC InnerNpc;

    public WrappedNpc(NPC npc)
    {
        InnerNpc = npc;

        Id = npc.id.ToString();
        BirthdaySeason = npc.Birthday_Season;
        BirthdayDay = npc.Birthday_Day;

        TexureName = Convert.ToHexString(SHA256.Create().ComputeHash(Encoding.ASCII.GetBytes(Id))).ToLower();
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

        Directory.CreateDirectory(Path.Combine(basePath, "textures/portraits"));
        Directory.CreateDirectory(Path.Combine(basePath, "textures/portraits", TexureName[0].ToString()));

        var stream = File.Create(Path.Combine(basePath, "textures/portraits", TexureName[0].ToString(),
            TexureName + ".png"));

        rTarget.SaveAsPng(stream, rTarget.Width, rTarget.Height);

        stream.Dispose();
    }

    public void PopulateDisplayName(string code)
    {
        DisplayNames[code] = NPC.GetDisplayName(InnerNpc.Name);
    }
}