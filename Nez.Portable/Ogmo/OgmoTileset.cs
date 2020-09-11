using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Nez.Ogmo
{
    public class OgmoTileset
    {
        [JsonProperty("label")]
        public string Name;
        [JsonProperty("path")]
        public string Path;

        // Internal Sizes.
        [JsonProperty("tileWidth")]
        internal int tileWidth;
        [JsonProperty("tileHeight")]
        internal int tileHeight;

        // Internal Spacings.
        [JsonProperty("tileSeparationX")]
        internal int tileSpacingX;
        [JsonProperty("tileSeparationY")]
        internal int tileSpacingY;

        [JsonIgnore]
        public Point TileSize;

        [JsonIgnore]
        public Point TileSpacing;

        [JsonIgnore]
        public Texture2D LoadedTexture;

        [JsonIgnore]
        public Rectangle[] TileSources;

        [OnDeserialized]
        internal void PostInit(StreamingContext ctx)
        {
            TileSize = new Point(tileWidth, tileHeight);
            TileSpacing = new Point(tileSpacingX, tileSpacingY);
        }

        /// <summary>
        /// Pre-calculates all the tile sources.
        /// </summary>
        internal void Initialize(Texture2D texture)
        {
            LoadedTexture = texture;
            int totalX = LoadedTexture.Width / (TileSize.X + TileSpacing.X);
            int totalY = LoadedTexture.Height / (TileSize.Y + TileSpacing.Y);
            int totalTiles = totalX * totalY;
            TileSources = new Rectangle[totalTiles];
            for(int i = 0; i < TileSources.Length; i++)
            {
                var point = Utils.DimensionIndex(i, totalX);
                var rect = new Rectangle(point.X * TileSize.X, point.Y * TileSize.Y, TileSize.X, TileSize.Y);
                TileSources[i] = rect;
            }
        }
    }
}
