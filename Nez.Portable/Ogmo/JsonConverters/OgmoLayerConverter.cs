using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace Nez.Ogmo.JsonConverters
{
    public class OgmoLayerConverter : JsonConverter<OgmoLayer>
    {
        public static OgmoLayerConverter Instance = new OgmoLayerConverter();

        public override OgmoLayer ReadJson(JsonReader reader, Type objectType, OgmoLayer existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JObject obj = JObject.Load(reader);
            OgmoProject project = serializer.Context.Context as OgmoProject;
            OgmoLayerDefinition[] context = project.LayerDefinitions;
            if(context == null)
                throw new Exception("You must pass the OgmoProject as a serializer context to deserialize this type.");

            var eid = (string)obj["_eid"];

            for(int i = 0; i < context.Length; i++)
            {
                if (context[i] == null) // Grid layers arent implemented, nor will they.
                    continue;
                if(eid == context[i].ExportID)
                {
                    // Found the matching definition.
                    var def = context[i];

                    switch (def)
                    {
                        case OgmoTileLayerDefinition tile:
                            OgmoTileLayer tileLayer = new OgmoTileLayer();
                            tileLayer.Target = tile;
                            tileLayer.TileSet = (string)obj["tileset"];
                            tileLayer.ExportID = eid;

                            // Todo: read the data array mode and map it to a 1d array always.
                            var dat = obj["data"].Children().ToArray();
                            tileLayer.Data = new int[dat.Length];
                            for(int j = 0; j < dat.Length; j++)
                            {
                                tileLayer.Data[j] = (int)dat[j];
                            }
                            
                            tileLayer.CellSize  = new Point((int)obj["gridCellWidth"], (int)obj["gridCellHeight"]);
                            tileLayer.CellCount = new Point((int)obj["gridCellsX"],    (int)obj["gridCellsY"]);
                            return tileLayer;
                        case OgmoEntityLayerDefinition entity:
                            OgmoEntityLayer entityLayer = new OgmoEntityLayer();
                            entityLayer.ExportID = eid;
                            entityLayer.Target = entity;
                            var valuelist = obj["entities"].Children().ToArray();
                            entityLayer.Entities = new OgmoEntity[valuelist.Length];
                            for (int x = 0; x < valuelist.Length; x++)
                            {
                                OgmoEntity ov = new OgmoEntity();
                                OgmoEntity result = (OgmoEntity)OgmoEntityConverter.Instance.ReadJson(valuelist[x].CreateReader(), typeof(OgmoEntity), ov, serializer);
                                entityLayer.Entities[x] = result;
                            }
                            return entityLayer;

                    }
                }
            }
            return null;
        }

        public override void WriteJson(JsonWriter writer, OgmoLayer value, JsonSerializer serializer)
        {
        }
    }
}
