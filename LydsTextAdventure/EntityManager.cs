using System;
using System.Collections.Generic;
using System.Linq;

namespace LydsTextAdventure
{

    public class EntityManager
    {

        private static Dictionary<int, Entity> Entities = new Dictionary<int, Entity>();
        private static List<Entity> VisibleEntities = new List<Entity>();
        private static List<Entity> AliveEntities = new List<Entity>();

        private static int EntityCount = 0;

        public static void RegisterEntity(Entity entity)
        {
            entity.SetIndex(EntityManager.EntityCount);
            entity.World = WorldManager.CurrentWorld;

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

            EntityManager.VisibleEntities.Clear();
            EntityManager.AliveEntities.Clear();
        }

        public static List<Entity> GetEntitiesByType(Type type)
        {

            List<Entity> result = new List<Entity>();
            for (int i = 0; i < Entities.Count; i++)
            {

                if (!EntityManager.Entities.TryGetValue(i, out Entity entity))
                    continue;

                if (entity.GetType() == type && !entity.isDestroyed)
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

                if (entity.Value.GetType() == type && !entity.Value.isDestroyed)
                    return entity.Value;
            }

            return null;
        }

        public static List<Entity> GetEntitiesByName(string name)
        {

            List<Entity> result = new List<Entity>();
            foreach (KeyValuePair<int, Entity> entity in EntityManager.Entities)
            {

                if (entity.Value.Name == name && !entity.Value.isDestroyed)
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
                entity.Value.isVisible = false;
                entity.Value.SetDisabled(true);
                entity.Value.isMarkedForDeletion = true;
                break;
            }
        }

        public static void UpdateEntities()
        {

            //updates disabled entities not seen by the camera last frame
            Camera.UpdateDisabled();
            //then get the visible entities
            List<Entity> list = EntityManager.GetVisibleEntities();

            //then update them
            for (int i = 0; i < list.Count; i++)
            {
                Entity entity = list[i];
                if (entity.isDisabled)
                    continue;

                if (entity.GetType() != typeof(Camera))
                    if (Entity.IsMouseOver(ConsoleManager.GetMousePosition(), entity))
                    {

                        entity.isHovering = true;
                        entity.OnHover();
                    }
                    else
                        entity.isHovering = false;

                if (!entity.isWaiting && !entity.isStatic && !entity.isMarkedForDeletion)
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

        public static List<Entity> GetVisibleEntitiesAroundPosition(Position position, int range = 1, bool solidsOnly = true)
        {

            return (EntityManager.GetEntities(position, range, solidsOnly, EntityManager.GetVisibleEntities()));
        }


        public static List<Entity> GetEntitiesAroundPosition(Position position, int range = 1, bool solidsOnly = true)
        {

            return (EntityManager.GetEntities(position, range, solidsOnly, EntityManager.GetAliveEntities()));
        }

        public static List<Entity> GetEntities(Position position, int range, bool solidsOnly, List<Entity> list)
        {

            if (range <= 0)
                range = 1;

            List<Entity> result = new List<Entity>();
            for (int i = 0; i < list.Count; i++)
            {
                Entity entity = list[i];
                if (entity.position.x > position.x - range && entity.position.x < position.x + range)
                    if (entity.position.y > position.y - range && entity.position.y < position.y + range)
                    {

                        if (solidsOnly && !entity.isSolid)
                            continue;

                        result.Add(entity);
                    }

            }

            return result;
        }

        public static List<Entity> GetVisibleEntities(bool ignoreCache = false)
        {

            if (!ignoreCache && EntityManager.VisibleEntities.Count != 0)
                return EntityManager.VisibleEntities;

            List<Entity> result = new List<Entity>();

            for (int i = 0; i < EntityCount; i++)
            {

                if (!EntityManager.Entities.TryGetValue(i, out Entity entity))
                {
                    Program.DebugLog("failed to read entity at index [" + i + "]", "entity_manager");
                    continue;
                }


                if (entity.isVisible && !entity.isDestroyed && !entity.isMarkedForDeletion)
                    result.Add(entity);
            }

            EntityManager.VisibleEntities = result;
            return result;
        }

        public static void CacheEntities()
        {

            EntityManager.VisibleEntities.Clear();
            EntityManager.AliveEntities.Clear();

            for (int i = 0; i < EntityCount; i++)
            {

                if (!EntityManager.Entities.TryGetValue(i, out Entity entity))
                {
                    Program.DebugLog("failed to read entity at index [" + i + "]", "entity_manager");
                    continue;
                }

                if (!entity.isDestroyed && !entity.isMarkedForDeletion)
                    AliveEntities.Add(entity);

                if (entity.isVisible && !entity.isDestroyed && !entity.isMarkedForDeletion)
                    VisibleEntities.Add(entity);
            }
        }

        public static List<Entity> GetAliveEntities(bool ignoreCache = false)
        {

            if (!ignoreCache && EntityManager.AliveEntities.Count != 0)
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

                if (!entity.isDestroyed && !entity.isMarkedForDeletion)
                    result.Add(entity);
            }

            EntityManager.AliveEntities = result;
            return result;
        }
    }
}