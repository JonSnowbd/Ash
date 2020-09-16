using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Ash.Ogmo.JsonConverters;

namespace Ash.Ogmo
{
    [JsonConverter(typeof(OgmoEntityDefinitionConverter))]
    public class OgmoEntityDefinition
    {
        public string ExportID;
        public string Name;
        public int Limit;
        public Point Origin;
        public Point Size;
        public Point TileSize;

        public string[] Tags;

        public OgmoValueDefinition[] ValueDefinitions;
    }
}
