using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ash.Ogmo.Components
{
    public class OgmoRenderer : ECRenderable
    {
        public override RectangleF Bounds
        {
            get
            {
                if (_areBoundsDirty)
                {
                    _bounds.X = Entity.Position.X;
                    _bounds.Y = Entity.Position.Y;
                    _bounds.Width = TargetLevel.LevelSize.X;
                    _bounds.Height = TargetLevel.LevelSize.Y;
                    _areBoundsDirty = false;
                }
                return _bounds;
            }
        }

        OgmoLevel TargetLevel;
        public List<OgmoTileLayer> RenderedLayers;
        Dictionary<string, OgmoTileset> TilesetLookup;

        /// <param name="level">The level intended to be rendered. Get this by calling <c>.LoadLevel(string)</c> on an <c>OgmoProject</c></param>
        public OgmoRenderer(OgmoLevel level)
        {
            RenderedLayers = new List<OgmoTileLayer>();
            TilesetLookup = new Dictionary<string, OgmoTileset>();

            TargetLevel = level;
            for(int i = 0; i < TargetLevel.Layers.Length; i++)
            {
                if(TargetLevel.Layers[i] is OgmoTileLayer tileLayer)
                {
                    RenderedLayers.Add(tileLayer);
                }
            }

            _areBoundsDirty = true;
        }

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
            var Content = Entity.Scene.Content;
            foreach(var layer in RenderedLayers)
            {
                if (TilesetLookup.ContainsKey(layer.TileSet))
                    continue;
                var tileset = TargetLevel.Project.GetTileset(layer.TileSet);
                TilesetLookup.Add(layer.TileSet, tileset);
            }
            RenderedLayers.Sort((a, b) => 
            {
                var av = Array.IndexOf(TargetLevel.Layers, a);
                var bv = Array.IndexOf(TargetLevel.Layers, b);
                return bv.CompareTo(av);
            });
        }
        public override void Render(Batcher batcher, Camera camera)
        {
#if DEBUG
            Tilerenders = 0;
#endif
            Rectangle dst = new Rectangle();
            for(int i = 0; i < RenderedLayers.Count; i++)
            {
                var layer = RenderedLayers[i];
                var tile = TilesetLookup[layer.TileSet];
                dst.Width = tile.TileSize.X;
                dst.Height = tile.TileSize.Y;

                var start = layer.WorldToTile(camera.Bounds.Location);
                var end = layer.WorldToTile(camera.Bounds.Location + camera.Bounds.Size);

                for(int y = start.Y; y <= end.Y; y++)
                {
                    for(int x = start.X; x <= end.X; x++)
                    {
                        var realIndex = Utils.DimensionIndex(x, y, layer.CellCount.X);
                        if (realIndex >= layer.Data.Length)
                            continue;
                        var dataTile = layer.Data[realIndex];
                        if (dataTile == -1)
                            continue;

                        var worldIndex = Utils.DimensionIndex(realIndex, layer.CellCount.X);

                        dst.X = (int)Entity.Position.X + (int)LocalOffset.X + worldIndex.X * layer.CellSize.X;
                        dst.Y = (int)Entity.Position.Y + (int)LocalOffset.Y + worldIndex.Y * layer.CellSize.Y;

                        var src = tile.TileSources[dataTile];

                        batcher.Draw(tile.LoadedTexture, dst, src, Color, SpriteEffects.None);
#if DEBUG
                        Tilerenders++;
#endif
                    }
                }
            }
        }

#if DEBUG
        int Tilerenders;
        StringBuilder debugBuilder = new StringBuilder();
        readonly Vector2 pad = new Vector2(4, 4);
        public override void DebugRender(Batcher batcher)
        {
            base.DebugRender(batcher);
            debugBuilder.Clear();
            debugBuilder.AppendLine($"{TargetLevel.Project.Name} - {TargetLevel.Name}");
            debugBuilder.Append($"{Tilerenders} Tiles Rendered");

            var str = debugBuilder.ToString();
            var measurement = Graphics.Instance.DevFont.MeasureString(str);
            var end = Bounds.Location + Bounds.Size - measurement - pad;
            var desiredLocation = Entity.Scene.Camera.Bounds.Location + (Entity.Scene.Camera.Bounds.Size * 0.5f);
            desiredLocation.X -= measurement.X * 0.5f;
            desiredLocation.Y = Entity.Position.Y + pad.Y;
            var final = Vector2.Clamp(Vector2.Max(desiredLocation,Entity.Scene.Camera.Bounds.Location+pad), Entity.Position+pad, end);
            
            batcher.DrawString(Graphics.Instance.DevFont, str, final, ECDebug.Colors.DebugText);
        }
#endif
    }
}
