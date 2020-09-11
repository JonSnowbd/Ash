using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Nez.Ogmo.JsonConverters;
using System.Collections.Generic;

namespace Nez.Ogmo
{
    [JsonConverter(typeof(OgmoEntityConverter))]
    public class OgmoEntity
    {
        [JsonProperty("name")]
        public string Target;

        public string ExportID;
        public int WorldID;

        [JsonIgnore]
        public Point Position;
        [JsonIgnore]
        public Point Origin;
        
        [JsonIgnore]
        public Dictionary<string, OgmoValue> Values;
    }
}
