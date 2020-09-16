using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace Ash.Ogmo.JsonConverters
{
    public class OgmoValueConverter : JsonConverter<OgmoValue>
    {
        public static OgmoValueConverter Instance = new OgmoValueConverter();

        public override OgmoValue ReadJson(JsonReader reader, Type objectType, OgmoValue existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JObject obj = JObject.Load(reader);
            OgmoValueDefinition[] context = serializer.Context.Context as OgmoValueDefinition[];
            if (context == null)
                throw new Exception("You must pass in an array of ogmo value definitions to be used in the Value Converter.");

            var eid = (string)obj["_eid"];

            for (int i = 0; i < context.Length; i++)
            {
                if (context[i] == null) 
                    continue;
                if (true)
                {
                    // Found the matching definition.
                    var def = context[i];
                }
            }
            return null;
        }

        public override void WriteJson(JsonWriter writer, OgmoValue value, JsonSerializer serializer)
        {
        }
    }
}
