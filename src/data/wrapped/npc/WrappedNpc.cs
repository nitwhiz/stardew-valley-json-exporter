using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using StardewValley;

namespace JsonExporter.data.wrapped.npc
{
    public class WrappedNpc : WrappedInstance<NPC>
    {
        [JsonProperty] public readonly string Name;

        [JsonProperty] public readonly string NormalizedTextureName;

        public readonly string TextureName;

        public WrappedNpc(NPC npc) : base(npc)
        {
            Name = npc.displayName;
            TextureName = npc.getTextureName();

            NormalizedTextureName = Normalize(TextureName);
        }

        public void SavePortrait(string basePath)
        {
            Directory.CreateDirectory(Path.Combine(basePath, "portraits"));

            var rTarget = new RenderTarget2D(Game1.graphics.GraphicsDevice, 64, 64, false,
                SurfaceFormat.Color, DepthFormat.Depth24, 0, RenderTargetUsage.DiscardContents);

            var gd = Game1.graphics.GraphicsDevice;

            gd.SetRenderTarget(rTarget);

            var s = new SpriteBatch(gd);

            s.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
            gd.Clear(Color.Transparent);
            s.Draw(Original.Portrait, Vector2.Zero, new Rectangle(0, 0, 64, 64), Color.White);
            s.End();

            Stream stream = File.Create(Path.Combine(basePath, "portraits", NormalizedTextureName + ".png"));
            rTarget.SaveAsPng(stream, rTarget.Width, rTarget.Height);

            stream.Dispose();
        }
    }
}