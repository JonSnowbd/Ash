using Microsoft.Xna.Framework;
using Nez.Ogmo.Components;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Nez.Ogmo
{
    public class OgmoScene : ECScene
    {
        public OgmoLevel CurrentLevel;
        public OgmoRenderer LevelRenderer;
        public List<ECEntity> EmittedEntities;

        /// <summary>
        /// If true, the camera will be bound within the
        /// level's size, preventing it from viewing outside of it.
        /// </summary>
        public bool LockCameraToLevelBounds;
        /// <summary>
        /// If true, when the camera is zoomed, partial pixels will be rounded. Note if you
        /// are using mouse drag to move the camera it will be buggy. Use the mouse drag to modify
        /// an external vector2, then set the cameras position to that vector2 to workaround.
        /// </summary>
        public bool LockCameraToZoomPixels;

        public Assembly OverrideAssembly;

        /// <param name="autoEmitEntities">
        /// If true, each entity layer will have its entities mapped 
        /// to Nez Entities and instantiated if they implement <c>IOgmoEmittable</c>
        /// </param>
        /// <param name="lockCamera">
        /// If true, the camera will be bound within the
        /// level's size, preventing it from viewing outside of it.
        /// </param>
        public OgmoScene(OgmoLevel level, bool autoEmitEntities = true, Assembly entityAssembly = null)
        {
            CurrentLevel = level;
            EmittedEntities = ListPool<ECEntity>.Obtain();

            var map = CreateEntity("Map");
            LevelRenderer = map.AddComponent(new OgmoRenderer(CurrentLevel));
            LockCameraToLevelBounds = false;
            LockCameraToZoomPixels = false;
            OverrideAssembly = entityAssembly;

            if (autoEmitEntities)
            {
                EmitEntities();
            }
        }

        public override void Unload()
        {
            base.Unload();
            ListPool<ECEntity>.Free(EmittedEntities);
        }

        public override void Update()
        {
            base.Update();

            if (LockCameraToLevelBounds)
            {
                if (
                    Camera.Bounds.Left < LevelRenderer.Bounds.Left || Camera.Bounds.Top < LevelRenderer.Bounds.Top
                    || Camera.Bounds.Right > LevelRenderer.Bounds.Right || Camera.Bounds.Bottom > LevelRenderer.Bounds.Bottom
                    )
                {
                    Camera.Position = Vector2.Clamp(Camera.Bounds.Location, LevelRenderer.Bounds.Location, LevelRenderer.Bounds.Location + LevelRenderer.Bounds.Size - Camera.Bounds.Size) + (Camera.Bounds.Size * 0.5f);
                }
            }

            if (LockCameraToZoomPixels)
            {
                Camera.Position = Vector2.Floor(Camera.Position);
            }
        }

        /// <summary>
        /// Loops through all the layers that are of <c>EntityLayer</c> and uses reflection
        /// to dynamically instantiate any Nez entity of the same name that implements 
        /// <c>IOgmoEmittable</c>
        /// </summary>
        public void EmitEntities()
        {
            for(int i = 0; i < CurrentLevel.Layers.Length; i++)
            {
                if(CurrentLevel.Layers[i] is OgmoEntityLayer entLayer)
                {
                    EmitEntityLayer(entLayer);
                }
            }
            PostEntitiesAdded(EmittedEntities);
        }

        public void EmitEntityLayer(OgmoEntityLayer layer)
        {
            Assembly assembly = OverrideAssembly ?? GetType().Assembly;
            for (int i = 0; i < layer.Entities.Length; i++)
            {
                BeforeEntitySubmitted(layer.Entities[i]);

                Type targetType = null;
                foreach (var type in assembly.ExportedTypes)
                {
                    if(type.Name == layer.Entities[i].Target)
                    {
                        targetType = type;
                        break;
                    }
                }

                if (targetType == null)
                {
                    Debug.Warn($"Couldnt find type <{layer.Entities[i].Target}> in <{assembly.FullName}>");
                    continue;
                }

                object ent = Activator.CreateInstance(targetType);
                if(ent != null && ent is ECEntity && ent is IOgmoEmittable)
                {
                    var ogmo = ent as IOgmoEmittable;
                    var entity = ent as ECEntity;
                    ogmo.Absorb(layer.Entities[i]);

                    AddEntity(entity);
                    EmittedEntities?.Add(entity);
                    OnEntitySubmitted(layer.Entities[i], entity);
                }
                else
                {
                    Debug.Error($"Failed to emit entity {layer.Entities[i].Target} from Type {targetType.FullName}");
                }
            }
        }

        public virtual void OnEntitySubmitted(OgmoEntity entity, ECEntity output)
        {

        }

        public virtual void BeforeEntitySubmitted(OgmoEntity entity)
        {

        }

        public virtual void PostEntitiesAdded(List<ECEntity> EntitiesAdded)
        {

        }
    }
}
