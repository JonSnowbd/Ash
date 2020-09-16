using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace Ash.Ogmo.JsonConverters
{
    public class OgmoLayerDefinitionConverter : JsonConverter<OgmoLayerDefinition>
    {
        public static OgmoLayerDefinitionConverter Instance = new OgmoLayerDefinitionConverter();

        public override OgmoLayerDefinition ReadJson(JsonReader reader, Type objectType, OgmoLayerDefinition existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JObject obj = JObject.Load(reader);
            switch ((string)obj["definition"])
            {
                case "entity":
                    var entLayer = new OgmoEntityLayerDefinition();
                    entLayer.Name = (string)obj["name"];
                    entLayer.ExportID = (string)obj["exportID"];

                    var entsize = new Point();
                    entsize.X = (int)obj["gridSize"]["x"];
                    entsize.Y = (int)obj["gridSize"]["y"];
                    entLayer.GridSize = entsize;
                    
                    return entLayer;
                case "decal":
                    var decLayer = new OgmoDecalLayerDefinition();
                    decLayer.Name = (string)obj["name"];
                    decLayer.ExportID = (string)obj["exportID"];

                    var decsize = new Point();
                    decsize.X = (int)obj["gridSize"]["x"];
                    decsize.Y = (int)obj["gridSize"]["y"];
                    decLayer.GridSize = decsize;

                    decLayer.Folder = (string)obj["folder"];

                    decLayer.Scalable = (bool)obj["scaleable"];
                    decLayer.Rotatable = (bool)obj["rotatable"];

                    var vallist = obj["values"].Children().ToArray();
                    decLayer.Values = new OgmoValueDefinition[vallist.Length];
                    for(int i = 0; i < vallist.Length; i++)
                    {
                        OgmoValueDefinition ov = new OgmoEnumValueDefinition();
                        OgmoValueDefinition result = (OgmoValueDefinition)OgmoValueDefinitionConverter.Instance.ReadJson(vallist[i].CreateReader(), typeof(OgmoValueDefinition), ov, serializer);
                        decLayer.Values[i] = result;
                    }

                    return decLayer;
                case "tile":
                    var tileLayer = new OgmoTileLayerDefinition();
                    tileLayer.Name = (string)obj["name"];
                    tileLayer.ExportID = (string)obj["exportID"];

                    var tilesize = new Point();
                    tilesize.X = (int)obj["gridSize"]["x"];
                    tilesize.Y = (int)obj["gridSize"]["y"];
                    tileLayer.GridSize = tilesize;

                    tileLayer.ExportStyle = (OgmoTileLayerDefinition.ExportMode)((int)obj["exportMode"]);
                    tileLayer.ArrayStyle = (OgmoTileLayerDefinition.ArrayMode)((int)obj["arrayMode"]);

                    tileLayer.DefaultTileset = (string)obj["defaultTileset"];

                    return tileLayer;
            }
            return null;
        }

        public override void WriteJson(JsonWriter writer, OgmoLayerDefinition value, JsonSerializer serializer)
        {
        }
    }
}
