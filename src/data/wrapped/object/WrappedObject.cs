using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using StardewValley;
using StardewObject = StardewValley.Object;

namespace JsonExporter.data.wrapped.@object
{
    [JsonObject(MemberSerialization.OptIn)]
    public class WrappedObject : WrappedInstance<StardewObject>
    {
        [JsonProperty] public readonly string DisplayName;

        [JsonProperty] public readonly string NormalizedName;

        public WrappedObject(StardewObject originalStardewObject) : base(originalStardewObject)
        {
            DisplayName = Original.DisplayName;
            NormalizedName = Normalize(DisplayName);
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
            Original.drawInMenu(s, Vector2.Zero, 1f, 1f, 1f, StackDrawType.Hide, Color.White, false);
            s.End();

            Directory.CreateDirectory(Path.Combine(basePath, "items"));

            var stream = File.Create(Path.Combine(basePath, "items", NormalizedName + ".png"));

            rTarget.SaveAsPng(stream, rTarget.Width, rTarget.Height);

            stream.Dispose();
        }
    }
}