using System;
using System.Collections.Generic;
using System.Linq;

namespace LydsTextAdventure
{

    public class EntityManager
    {

        private static Dictionary<int, Entity> Entities = new Dictionary<int, Entity>();
        private static List<Entity> VisibleEntities;
        private static List<Entity> AliveEntities;

        private static int EntityCount = 0;

        public static void RegisterEntity(Entity entity)
        {
            entity.SetIndex(EntityManager.EntityCount);
            entity.SetWorld(WorldManager.CurrentWorld);

            if (!Entities.TryAdd(EntityManager.EntityCount, entity))
                Program.DebugLog("entity " + entity.ToString() + " FAILED TO CREATE!", "entity_manager");

            EntityManager.EntityCount = EntityCount + 1;

        }

        public static void DestroyAllEntities()
        {

            foreach (KeyValuePair<int, Entity> entity in EntityManager.Entities)
            {
                entity.Value.Destroy();
            }

            EntityManager.Entities = new Dictionary<int, Entity>();
            EntityManager.EntityCount = 0;
            EntityManager.VisibleEntities = null;
            EntityManager.AliveEntities = null;
        }

        public static List<Entity> GetEntitiesByType(Type type)
        {

            List<Entity> result = new List<Entity>();
            for (int i = 0; i < Entities.Count; i++)
            {

                if (!EntityManager.Entities.TryGetValue(i, out Entity entity))
                    continue;

                if (entity.GetType() == type && !entity.IsDestroyed())
                    result.Add(entity);
            }

            return result;
        }

        public static Camera GetMainCamera()
        {

            List<Entity> entities = EntityManager.GetEntitiesByType(typeof(Camera));

            foreach (Entity ent in entities)
            {

                Camera cam = (Camera)ent;

                if (cam.IsMainCamera())
                    return cam;
            }

            return (Camera)EntityManager.GetEntityByType(typeof(Camera));
        }

        public static Entity GetEntityByType(Type type)
        {

            foreach (KeyValuePair<int, Entity> entity in EntityManager.Entities)
            {

                if (entity.Value.GetType() == type && !entity.Value.IsDestroyed())
                    return entity.Value;
            }

            return null;
        }

        public static List<Entity> GetEntitiesByName(string name)
        {

            List<Entity> result = new List<Entity>();
            foreach (KeyValuePair<int, Entity> entity in EntityManager.Entities)
            {

                if (entity.Value.GetName() == name && !entity.Value.IsDestroyed())
                    result.Add(entity.Value);
            }

            return result;
        }


        public static void RemoveEntity(string id)
        {

            foreach (KeyValuePair<int, Entity> entity in EntityManager.Entities)
            {

                if (entity.Value.id != id)
                    continue;

                entity.Value.Destroy();
                entity.Value.SetVisible(false);
                entity.Value.SetDisabled(true);
                entity.Value.isMarkedForDeletion = true;
                break;
            }
        }

        public static void UpdateEntities()
        {

            //updates disabled entities not seen by the camera last frame
            Camera.UpdateDisabled();

            List<Entity> list = EntityManager.GetVisibleEntities();
            for (int i = 0; i < list.Count; i++)
            {
                Entity entity = list[i];
                if (entity.IsDisabled())
                    continue;

                if (entity.GetType() != typeof(Camera))
                    if (Entity.IsMouseOver(ConsoleManager.GetMousePosition(), entity))
                    {

                        entity.isHovering = true;
                        entity.OnHover();
                    }
                    else
                        entity.isHovering = false;

                if (!entity.isWaiting && !entity.isStatic)
                {
                    entity.Update(Program.GetTick());
                }
            }
        }

        public static List<Entity> GetEntities()
        {

            return EntityManager.Entities.Values.ToList();
        }

        public static Entity GetClosestTo(Entity entity, int range = 1)
        {

            List<Entity> entities = EntityManager.GetEntitiesAroundPosition(entity.position, range); //this will naturally get the longest distance first

            if (entities.Count == 0)
                return null;

            entities.Remove(entity);

            return entities.LastOrDefault(); //so get the last element aka the first
        }


        public static Entity GetFurthestFrom(Entity entity, int range = 1)
        {

            List<Entity> entities = EntityManager.GetEntitiesAroundPosition(entity.position, range);

            if (entities.Count == 0)
                return null;

            entities.Remove(entity);

            return entities.FirstOrDefault();
        }

        public static List<Entity> GetEntitiesAroundPosition(Position position, int range = 1, bool solidsOnly = true)
        {

            if (range <= 0)
                range = 1;

            List<Entity> result = new List<Entity>();
            List<Entity> list = EntityManager.GetAliveEntities();
            for (int i = 0; i < list.Count; i++)
            {
                Entity entity = list[i];
                if (entity.position.x > position.x - range && entity.position.x < position.x + range)
                    if (entity.position.y > position.y - range && entity.position.y < position.y + range)
                    {

                        if (solidsOnly && !entity.IsSolid())
                            continue;

                        result.Add(entity);
                    }

            }

            return result;
        }

        public static List<Entity> GetVisibleEntities(bool ignoreCache = false)
        {

            if (!ignoreCache && EntityManager.VisibleEntities != null)
                return EntityManager.VisibleEntities;

            List<Entity> result = new List<Entity>();

            for (int i = 0; i < EntityCount; i++)
            {

                if (!EntityManager.Entities.TryGetValue(i, out Entity entity))
                {
                    Program.DebugLog("failed to read entity at index [" + i + "]", "entity_manager");
                    continue;
                }


                if (entity.IsVisible() && !entity.IsDestroyed())
                    result.Add(entity);
            }

            EntityManager.VisibleEntities = result;
            return result;
        }

        public static List<Entity> GetAliveEntities(bool ignoreCache = false)
        {

            if (!ignoreCache && EntityManager.AliveEntities != null)
                return EntityManager.AliveEntities;

            List<Entity> result = new List<Entity>();

            //uses a for loop since the Entities dictionary could be updated from a different thread
            for (int i = 0; i < EntityCount; i++)
            {

                if (!EntityManager.Entities.TryGetValue(i, out Entity entity))
                {
                    Program.DebugLog("failed to read entity at index [" + i + "]", "entity_manager");
                    continue;
                }

                if (!entity.IsDestroyed() && !entity.isMarkedForDeletion)
                    result.Add(entity);

            }

            EntityManager.AliveEntities = result;
            return result;
        }
    }
}