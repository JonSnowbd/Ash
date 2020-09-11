using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace Nez.Ogmo.JsonConverters
{
    public class OgmoEntityDefinitionConverter : JsonConverter<OgmoEntityDefinition>
    {
        public static OgmoEntityDefinitionConverter Instance = new OgmoEntityDefinitionConverter();

        public override OgmoEntityDefinition ReadJson(JsonReader reader, Type objectType, OgmoEntityDefinition existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JObject obj = JObject.Load(reader);
            var def = new OgmoEntityDefinition();

            def.Name = (string)obj["name"];
            def.ExportID = (string)obj["_eid"];

            def.Size = new Point();
            def.Size.X = (int)obj["size"]["x"];
            def.Size.Y = (int)obj["size"]["y"];

            def.Origin = new Point();
            def.Origin.X = (int)obj["origin"]["x"];
            def.Origin.Y = (int)obj["origin"]["y"];

            def.TileSize = new Point();
            def.TileSize.X = (int)obj["tileSize"]["x"];
            def.TileSize.Y = (int)obj["tileSize"]["y"];

            def.Limit = (int)obj["limit"];

            var taglist = obj["tags"].Children().ToArray();
            def.Tags = new string[taglist.Length];
            for(int i = 0; i < taglist.Length; i++)
            {
                def.Tags[i] = (string)taglist[i];
            }

            var valuelist = obj["values"].Children().ToArray();
            def.ValueDefinitions = new OgmoValueDefinition[valuelist.Length];
            for(int i = 0; i < valuelist.Length; i++)
            {
                OgmoValueDefinition ov = new OgmoEnumValueDefinition();
                OgmoValueDefinition result = (OgmoValueDefinition)OgmoValueDefinitionConverter.Instance.ReadJson(valuelist[i].CreateReader(), typeof(OgmoValueDefinition), ov, serializer);
                def.ValueDefinitions[i] = result;
            }

            return def;
        }

        public override void WriteJson(JsonWriter writer, OgmoEntityDefinition value, JsonSerializer serializer)
        {
        }
    }
}
