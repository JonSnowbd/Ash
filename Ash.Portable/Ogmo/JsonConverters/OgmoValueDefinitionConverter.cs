using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace Ash.Ogmo.JsonConverters
{
    public class OgmoValueDefinitionConverter : JsonConverter<OgmoValueDefinition>
    {
        public static OgmoValueDefinitionConverter Instance = new OgmoValueDefinitionConverter();

        public override OgmoValueDefinition ReadJson(JsonReader reader, Type objectType, OgmoValueDefinition existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JObject obj = JObject.Load(reader);
            switch ((string)obj["definition"])
            {
                case "String":
                    var strValue = new OgmoStringValueDefinition();
                    strValue.Name = (string)obj["name"];
                    strValue.Default = (string)obj["defaults"];
                    strValue.MaxLength = (int)obj["maxLength"];
                    strValue.TrimWhitespace = (bool)obj["trimWhitespace"];
                    return strValue;
                case "Text":
                    var textValue = new OgmoTextValueDefinition();
                    textValue.Name = (string)obj["name"];
                    textValue.Default = (string)obj["defaults"];
                    return textValue;
                case "Enum":
                    var enumValue = new OgmoEnumValueDefinition();
                    enumValue.Name = (string)obj["name"];
                    var arr = obj["choices"].Children().ToArray();
                    var finalChoices = new string[arr.Length];
                    for(int i = 0; i < arr.Length; i++)
                    {
                        finalChoices[i] = (string)arr[i];
                    }
                    enumValue.Choices = finalChoices;
                    enumValue.Default = (int)obj["defaults"];
                    return enumValue;
                case "Integer":
                    var intValue = new OgmoIntegerValueDefinition();
                    intValue.Name = (string)obj["name"];
                    intValue.Default = (int)obj["defaults"];
                    intValue.Bounded = (bool)obj["bounded"];
                    intValue.Min = (int)obj["min"];
                    intValue.Max = (int)obj["max"];
                    return intValue;
                case "Float":
                    var floatValue = new OgmoFloatValueDefinition();
                    floatValue.Name = (string)obj["name"];
                    floatValue.Default = (float)obj["defaults"];
                    floatValue.Bounded = (bool)obj["bounded"];
                    floatValue.Min = (float)obj["min"];
                    floatValue.Max = (float)obj["max"];
                    return floatValue;
                case "Color":
                    var colValue = new OgmoColorValueDefinition();
                    colValue.Name = (string)obj["name"];
                    colValue.Default = ColorExt.HexToColor((string)obj["defaults"]);
                    colValue.IncludeAlpha = (bool)obj["includeAlpha"];
                    return colValue;
                    
            }
            return null;
        }

        public override void WriteJson(JsonWriter writer, OgmoValueDefinition value, JsonSerializer serializer)
        {
        }
    }
}
