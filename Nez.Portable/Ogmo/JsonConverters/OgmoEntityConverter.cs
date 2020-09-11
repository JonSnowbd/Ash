using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Nez.Ogmo.JsonConverters
{
    public class OgmoEntityConverter : JsonConverter<OgmoEntity>
    {
        public static OgmoEntityConverter Instance = new OgmoEntityConverter();

        public override OgmoEntity ReadJson(JsonReader reader, Type objectType, OgmoEntity existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JObject obj = JObject.Load(reader);

            OgmoProject project = serializer.Context.Context as OgmoProject;
            OgmoValueDefinition[] defs = null;

            var ent = new OgmoEntity();
            ent.Target = (string)obj["name"];

            for (int i = 0; i < project.EntityDefinitions.Length; i++)
            {
                if (project.EntityDefinitions[i].Name == ent.Target)
                {
                    defs = project.EntityDefinitions[i].ValueDefinitions;
                    break;
                }
            }

            ent.ExportID = (string)obj["_eid"];
            ent.WorldID = (int)obj["id"];
            ent.Position = new Point((int)obj["x"], (int)obj["y"]);
            ent.Origin = new Point((int)obj["originX"], (int)obj["originY"]);

            var valArray = obj["values"] as JObject;
            ent.Values = new Dictionary<string, OgmoValue>();

            if (defs == null || valArray == null)
            {
                if(defs == null)
                    Debug.Warn($"Definition member was not found for type {ent.Target}");
                return ent;
            }

            foreach(var pair in valArray)
            {
                for (int x = 0; x < defs.Length; x++)
                {
                    if (defs[x].Name == pair.Key)
                    {
                        var definition = defs[x];
                        switch (definition)
                        {
                            case OgmoColorValueDefinition color:
                                var colorVal = new OgmoColorValue();
                                colorVal.Name = pair.Key;
                                colorVal.Definition = color;
                                colorVal.Value = ColorExt.HexToColor((string)pair.Value);
                                ent.Values.Add(pair.Key, colorVal);
                                break;
                            case OgmoStringValueDefinition str:
                                var stringVal = new OgmoStringValue();
                                stringVal.Name = pair.Key;
                                stringVal.Definition = str;
                                stringVal.Value = (string)pair.Value;
                                ent.Values.Add(pair.Key, stringVal);
                                break;
                            case OgmoEnumValueDefinition enu:
                                var enumVal = new OgmoEnumValue();
                                enumVal.Name = pair.Key;
                                enumVal.Definition = enu;
                                enumVal.Value = (int)pair.Value;
                                ent.Values.Add(pair.Key, enumVal);
                                break;
                            case OgmoFloatValueDefinition flo:
                                var floatVal = new OgmoFloatValue();
                                floatVal.Name = pair.Key;
                                floatVal.Definition = flo;
                                floatVal.Value = (float)pair.Value;
                                ent.Values.Add(pair.Key, floatVal);
                                break;
                            case OgmoTextValueDefinition text:
                                var textValue = new OgmoTextValue();
                                textValue.Name = pair.Key;
                                textValue.Definition = text;
                                textValue.Value = (string)pair.Value;
                                ent.Values.Add(pair.Key, textValue);
                                break;
                        }
                    }
                }
            }

            return ent;
        }

        public override void WriteJson(JsonWriter writer, OgmoEntity value, JsonSerializer serializer)
        {
        }
    }
}
