using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Nez.Ogmo.JsonConverters;

namespace Nez.Ogmo
{
    [JsonConverter(typeof(OgmoValueConverter))]
    public abstract class OgmoValue
    {
        [JsonIgnore]
        public OgmoValueDefinition Definition;
        public string Name;
    }

    public class OgmoColorValue : OgmoValue
    {
        public Color Value;
    }
    public class OgmoStringValue : OgmoValue
    {
        public string Value;
    }
    public class OgmoTextValue : OgmoValue
    {
        public string Value;
    }
    public class OgmoFloatValue : OgmoValue
    {
        public float Value;
    }
    public class OgmoIntegerValue : OgmoValue
    {
        public int Value;
    }
    public class OgmoEnumValue : OgmoValue
    {
        public int Value;
    }
}
