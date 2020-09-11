using Microsoft.Xna.Framework;

namespace Nez.Ogmo
{
    public abstract class OgmoValueDefinition
    {
        public string Name;
    }
    public class OgmoColorValueDefinition : OgmoValueDefinition
    {
        public Color Default;
        public bool IncludeAlpha;
    }
    public class OgmoStringValueDefinition : OgmoValueDefinition
    {
        public string Default;
        public int MaxLength;
        public bool TrimWhitespace;
    }
    public class OgmoTextValueDefinition : OgmoValueDefinition
    {
        public string Default;
    }
    public class OgmoFloatValueDefinition : OgmoValueDefinition
    {
        public float Default;
        public bool Bounded;
        public float Min;
        public float Max;
    }
    public class OgmoIntegerValueDefinition : OgmoValueDefinition
    {
        public int Default;
        public bool Bounded;
        public int Min;
        public int Max;
    }
    public class OgmoEnumValueDefinition : OgmoValueDefinition
    {
        public string[] Choices;
        public int Default;
    }
}
