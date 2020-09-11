using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Nez.Ogmo.JsonConverters;

namespace Nez.Ogmo
{
    [JsonConverter(typeof(OgmoLayerDefinitionConverter))]
    public abstract class OgmoLayerDefinition
    {
        public OgmoProject ParentProject;
        public string Name;
        public Point GridSize;
        public string ExportID;
    }

    public class OgmoEntityLayerDefinition : OgmoLayerDefinition
    {
    }

    public class OgmoDecalLayerDefinition : OgmoLayerDefinition
    {
        public string Folder;
        public bool IncludeImageSequence;
        public bool Scalable;
        public bool Rotatable;
        public OgmoValueDefinition[] Values;
    }

    public class OgmoTileLayerDefinition : OgmoLayerDefinition
    {
        public enum ExportMode
        {
            ID,
            Coord
        }
        public enum ArrayMode
        {
            OneDimensional,
            TwoDimensional
        }
        public ExportMode ExportStyle;
        public ArrayMode ArrayStyle;
        public string DefaultTileset;
    }
}
