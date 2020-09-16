using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Ash.Ogmo.JsonConverters;

namespace Ash.Ogmo
{
    [JsonConverter(typeof(OgmoLayerConverter))]
    public class OgmoLayer
    {
        [JsonIgnore]
        public OgmoLayerDefinition Target;

        [JsonProperty("_eid")]
        public string ExportID;
    }

    public class OgmoEntityLayer : OgmoLayer
    {
        [JsonProperty("entities")]
        public OgmoEntity[] Entities;
    }

    public class OgmoTileLayer : OgmoLayer
    {
        public int[] Data;
        public string TileSet;
        public Point CellSize;
        public Point CellCount;
    }
}
