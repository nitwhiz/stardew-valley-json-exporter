using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using JsonExporter.Contract;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using StardewValley;
using StardewObject = StardewValley.Object;

namespace JsonExporter.Model;

[JsonObject(MemberSerialization.OptIn)]
public class WrappedObject : ITranslatable
{
    public readonly StardewObject InnerObject;

    [JsonProperty("category")] public readonly int Category;

    [JsonProperty("displayNames")] public readonly Dictionary<string, string> DisplayNames = new();

    [JsonProperty("id")] public readonly string Id;

    [JsonProperty("textureName")] public readonly string TextureName;

    [JsonProperty("type")] public readonly string Type;

    [JsonProperty("isGiftable")] public readonly bool IsGiftable;

    [JsonProperty("isBigCraftable")] public readonly bool IsBigCraftable;

    public WrappedObject(StardewObject obj)
    {
        InnerObject = obj;

        Id = InnerObject.QualifiedItemId;
        Type = InnerObject.Type.ToLower();
        Category = InnerObject.Category;
        IsGiftable = InnerObject.canBeGivenAsGift();
        IsBigCraftable = InnerObject.bigCraftable.Value;

        TextureName = Convert.ToHexString(SHA256.Create().ComputeHash(Encoding.ASCII.GetBytes(Id))).ToLower();
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
        InnerObject.drawInMenu(s, Vector2.Zero, 1f, 1f, 1f, StackDrawType.Hide, Color.White, false);
        s.End();

        Directory.CreateDirectory(Path.Combine(basePath, "textures/items"));
        Directory.CreateDirectory(Path.Combine(basePath, "textures/items", TextureName[0].ToString()));

        var stream =
            File.Create(Path.Combine(basePath, "textures/items", TextureName[0].ToString(), TextureName + ".png"));

        rTarget.SaveAsPng(stream, rTarget.Width, rTarget.Height);

        stream.Dispose();
    }

    public void PopulateDisplayName(string code)
    {
        DisplayNames[code] = InnerObject.DisplayName;
    }
}