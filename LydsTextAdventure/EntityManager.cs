using System;
using System.Collections.Generic;
using System.Linq;

namespace LydsTextAdventure
{

    public class EntityManager
   {

        private static List<Entity> entities = new List<Entity>();
        private static List<Entity> visibleEntities;
        private static List<Entity> aliveEntities;

        private static int sceneCount = 0;

        public static void RegisterEntity(Entity entity)
        {

            entity.SetIndex(EntityManager.sceneCount++);
            entity.SetWorld(WorldManager.CurrentWorld);

            entities.Add(entity);
            Program.DebugLog("entity " + entity.ToString() + " created", "entity_manager");
        }

        public static void DestroyAllEntities()
        {

            foreach (Entity entity in EntityManager.entities)
            {
                entity.Destroy();
            }

            EntityManager.entities = new List<Entity>();
            EntityManager.sceneCount = 0;
            EntityManager.visibleEntities = null;
            EntityManager.aliveEntities = null;
        }

        public static List<Entity> GetEntitiesByType(Type type)
        {

            List<Entity> result = new List<Entity>();
            foreach (Entity entity in EntityManager.entities)
            {

                if (entity.GetType() == type && !entity.IsDestroyed())
                    result.Add(entity);
            }

            return result;
        }

        public static Camera GetMainCamera()
        {

            List<Entity> entities = EntityManager.GetEntitiesByType(typeof(Camera));

            foreach(Entity ent in entities)
            {

                Camera cam = (Camera)ent;

                if (cam.IsMainCamera())
                    return cam;
            }

            return (Camera)EntityManager.GetEntityByType(typeof(Camera));
        }

        public static Entity GetEntityByType(Type type)
        {

            foreach (Entity entity in EntityManager.entities)
            {

                if (entity.GetType() == type && !entity.IsDestroyed())
                    return entity;
            }

            return null;
        }

        public static List<Entity> GetEntitiesByName(string name)
        {

            List<Entity> result = new List<Entity>();
            foreach (Entity entity in EntityManager.entities)
            {

                if (entity.GetName() == name && !entity.IsDestroyed())
                    result.Add(entity);
            }

            return result;
        }


        public static void RemoveEntity(string id)
        {

            foreach (Entity entity in EntityManager.entities)
            {
                if (entity.id != id)
                    continue;
                

                entity.Destroy();
                entities.Remove(entity);
                break;
            }
        }

        public static void UpdateEntities()
        {

            //updates disabled entities not seen by the camera last frame
            Camera.UpdateDisabled();

            foreach (Entity entity in EntityManager.GetVisibleEntities())
            {

                if (!entity.isWaiting)
                {

                    if (entity.IsDisabled())
                        continue;

                    entity.Update(Program.GetTick());
                }
            }
        }

        public static List<Entity> GetEntities()
        {

            return EntityManager.entities.GetRange(0, entities.Count);
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

            List<Entity> entities = EntityManager.GetEntitiesAroundPosition(entity.position, range );

            if (entities.Count == 0)
                return null;

            entities.Remove(entity);

            return entities.FirstOrDefault();
        }

        public static List<Entity> GetEntitiesAroundPosition(Position position, int range=1, bool solidsOnly = true)
        {

            if (range <= 0)
                range = 1;

            List<Entity> result = new List<Entity>();
            foreach (Entity entity in EntityManager.GetAliveEntities())
            {

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

        public static List<Entity> GetVisibleEntities(bool ignoreCache=false)
        {

            if (!ignoreCache && EntityManager.visibleEntities != null)
                return EntityManager.visibleEntities;

            List<Entity> result = new List<Entity>();
            foreach( Entity entity in EntityManager.entities )
            {

                if (entity.IsVisible() && !entity.IsDestroyed())
                    result.Add(entity);
            }

            EntityManager.visibleEntities = result;
            return result;
        }

        public static List<Entity> GetAliveEntities(bool ignoreCache = false)
        {

            if (!ignoreCache && EntityManager.aliveEntities != null)
                return EntityManager.aliveEntities;

            List<Entity> result = new List<Entity>();
            foreach (Entity entity in EntityManager.entities)
            {

                if (!entity.IsDestroyed())
                    result.Add(entity);
            }

            EntityManager.aliveEntities = result;
            return result;
        }
    }
}