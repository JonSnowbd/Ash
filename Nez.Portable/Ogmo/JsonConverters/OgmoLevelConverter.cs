using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nez.Ogmo.JsonConverters
{
    public class OgmoLevelConverter : JsonConverter<OgmoLevel>
    {
        public static OgmoLevelConverter Instance = new OgmoLevelConverter();

        public override OgmoLevel ReadJson(JsonReader reader, Type objectType, OgmoLevel existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JObject obj = JObject.Load(reader);

            OgmoProject project = serializer.Context.Context as OgmoProject;
            OgmoValueDefinition[] defs = project.LevelValueDefinitions;

            OgmoLevel level = new OgmoLevel();
            level.LevelSize = new Point((int)obj["width"], (int)obj["height"]);
            level.OgmoVersion = (string)obj["ogmoVersion"];

            // Layers.
            var layerList = obj["layers"].Children().ToArray();
            level.Layers = new OgmoLayer[layerList.Length];
            for (int x = 0; x < layerList.Length; x++)
            {
                OgmoLayer result = (OgmoLayer)OgmoLayerConverter.Instance.ReadJson(layerList[x].CreateReader(), typeof(OgmoLayer), null, serializer);
                level.Layers[x] = result;
            }

            // Values.
            var valArray = obj["values"] as JObject;
            level.Values = new Dictionary<string, OgmoValue>();

            if(valArray == null)
            {
                return level;
            }

            foreach (var pair in valArray)
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
                                level.Values.Add(pair.Key, colorVal);
                                break;
                            case OgmoStringValueDefinition str:
                                var stringVal = new OgmoStringValue();
                                stringVal.Name = pair.Key;
                                stringVal.Definition = str;
                                stringVal.Value = (string)pair.Value;
                                level.Values.Add(pair.Key, stringVal);
                                break;
                            case OgmoEnumValueDefinition enu:
                                var enumVal = new OgmoEnumValue();
                                enumVal.Name = pair.Key;
                                enumVal.Definition = enu;
                                var enumstring = (string)pair.Value;
                                enumVal.Value = Array.IndexOf(enu.Choices, enumstring);
                                level.Values.Add(pair.Key, enumVal);
                                break;
                            case OgmoFloatValueDefinition flo:
                                var floatVal = new OgmoFloatValue();
                                floatVal.Name = pair.Key;
                                floatVal.Definition = flo;
                                floatVal.Value = (float)pair.Value;
                                level.Values.Add(pair.Key, floatVal);
                                break;
                            case OgmoTextValueDefinition text:
                                var textValue = new OgmoTextValue();
                                textValue.Name = pair.Key;
                                textValue.Definition = text;
                                textValue.Value = (string)pair.Value;
                                level.Values.Add(pair.Key, textValue);
                                break;
                        }
                    }
                }
            }


            return level;
        }

        public override void WriteJson(JsonWriter writer, OgmoLevel value, JsonSerializer serializer)
        {
        }
    }
}
