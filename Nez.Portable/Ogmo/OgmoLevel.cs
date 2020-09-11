using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Nez.Ogmo.JsonConverters;
using System.Collections.Generic;

namespace Nez.Ogmo
{
    [JsonConverter(typeof(OgmoLevelConverter))]
    public class OgmoLevel
    {
        public OgmoProject Project;
        public string Name;
        public string FullPath;
        public string OgmoVersion;

        /// <summary>
        /// In pixels, how large the level is.
        /// </summary>
        public Point LevelSize;
        public Dictionary<string, OgmoValue> Values;
        public OgmoLayer[] Layers;
    }
}
