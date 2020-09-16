using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Ash.Ogmo.JsonConverters;
using Ash.Systems;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace Ash.Ogmo
{
    public class OgmoProject
    {
        [JsonIgnore]
        public NezContentManager ParentContentManager;
        [JsonIgnore]
        public string FullContentPath;

        [JsonProperty("name")]
        public string Name;
        [JsonProperty("ogmoVersion")]
        public string OgmoVersion;
        [JsonProperty("levelPaths")]
        public string[] LevelPaths;
        [JsonProperty("backgroundColor")]
        public string BackgroundColorHex;
        [JsonProperty("gridColor")]
        public string GridColor;
        [JsonProperty("anglesRadians")]
        public bool UseAngleRadians;
        [JsonProperty("directoryDepth")]
        public int DirectoryDepth;
        [JsonProperty("layerGridDefaultSize")]
        public Point DefaultLayerGridSize;
        [JsonProperty("levelDefaultSize")]
        public Point DefaultLevelSize;
        [JsonProperty("levelMinSize")]
        public Point MinimumLevelSize;
        [JsonProperty("levelMaxSize")]
        public Point MaximumLevelSize;
        [JsonProperty("levelValues", ItemConverterType = typeof(OgmoValueDefinitionConverter))]
        public OgmoValueDefinition[] LevelValueDefinitions;
        [JsonProperty("defaultExportMode")]
        public string DefaultExportMode;
        [JsonProperty("compactExport")]
        public bool UsesCompactExport;
        [JsonProperty("layers", ItemConverterType = typeof(OgmoLayerDefinitionConverter))]
        public OgmoLayerDefinition[] LayerDefinitions;
        [JsonProperty("entities", ItemConverterType = typeof(OgmoEntityDefinitionConverter))]
        public OgmoEntityDefinition[] EntityDefinitions;
        [JsonProperty("tilesets")]
        public OgmoTileset[] Tilesets;

        /// <summary>
        /// This is a list of every `level.json` file available to this project.
        /// </summary>
        [JsonIgnore]
        public List<string> Levels;

        /// <summary>
        /// This is the list of every `level.json` that has been request to be loaded.
        /// </summary>
        [JsonIgnore]
        public Dictionary<string, OgmoLevel> LoadedLevels;

        /// <summary>
        /// This is a dictionary that maps a level's name to its full content path in monogame.
        /// </summary>
        [JsonIgnore]
        public Dictionary<string, string> LevelNameToFullPath;

        [OnDeserialized]
        void PostInit(StreamingContext ctx)
        {
            Levels = new List<string>();
            LoadedLevels = new Dictionary<string, OgmoLevel>();
            LevelNameToFullPath = new Dictionary<string, string>();
            FullContentPath = ctx.Context as string;

            // Link all relationships
            foreach(var layerDef in LayerDefinitions)
            {
                if(layerDef != null)
                    layerDef.ParentProject = this;
            }

            // Get list of all levels.
            Levels = ListPool<string>.Obtain();

            for (int i = 0; i < LevelPaths.Length; i++)
            {
                var path = Path.Combine(Path.GetDirectoryName(FullContentPath), LevelPaths[i]);
                var all = Directory.GetFiles(path);
                foreach (var filePath in all)
                {
                    if (Path.GetExtension(filePath).Contains("json"))
                    {
                        var fileName = Path.GetFileName(filePath);
                        Levels.Add(fileName);

                        if (!LevelNameToFullPath.ContainsKey(fileName))
                        {
                            LevelNameToFullPath.Add(fileName, filePath);
                        }
                    }
                }
            }
        }

        public OgmoLevel LoadLevel(string level)
        {
            if(!Path.HasExtension(level))
                level = Path.ChangeExtension(level, "json");

            if (LoadedLevels.ContainsKey(level))
            {
                Debug.Warn("You're loading a level that was already loaded. Be careful as its likely to have been disposed if the parent NezContentManager has been disposed inbetween calls.");
                return LoadedLevels[level];
            }
                
            if(LevelNameToFullPath.ContainsKey(level) == false)
                throw new System.Exception("You cannot load a level that is not registed to this ogmo project. Try checking the file paths in your project and json files in content.");

            var targetFilePath = LevelNameToFullPath[level];
            OgmoLevel GameLevel = ParentContentManager.LoadJson(targetFilePath, this, OgmoLevelConverter.Instance);
            GameLevel.Project = this;
            GameLevel.FullPath = Path.GetDirectoryName(targetFilePath);
            GameLevel.Name = level;

            LoadedLevels.Add(level, GameLevel);

            return GameLevel;
        }

        public OgmoTileset GetTileset(string tilesetName)
        {
            for(int i = 0; i < Tilesets.Length; i++)
            {
                if (tilesetName == Tilesets[i].Name)
                {
                    if(Tilesets[i].LoadedTexture == null)
                    {
                        var tex = ParentContentManager.LoadTexture(Path.Combine(Path.GetDirectoryName(FullContentPath), Tilesets[i].Path));
                        Tilesets[i].Initialize(tex);
                    }
                    return Tilesets[i];
                }
                    
            }
            throw new System.Exception($"Tileset '{tilesetName}' does not exist on project {Name}");
        }
    }
}
