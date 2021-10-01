using System;
using System.Collections.Generic;

namespace LydsTextAdventure
{

   public class EntityManager
   {

        private static List<Entity> entities = new List<Entity>();

        public static void RegisterEntity(Entity entity)
        {

            Program.GetCommandController().Register(entity.RegisterCommands());
            entities.Add(entity);
            Program.DebugLog("entity " + entity.ToString() + " created" );
        }

        public static void DestroyAllEntities()
        {

            foreach (Entity entity in EntityManager.entities)
            {
                entity.Destroy();
            }

            EntityManager.entities = new List<Entity>();
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

        public static List<Entity> GetEntities()
        {

            return EntityManager.entities.GetRange(0, entities.Count);
        }

        public static List<Entity> GetVisibleEntities()
        {

            List<Entity> result = new List<Entity>();
            foreach( Entity entity in EntityManager.entities )
            {

                if (entity.IsVisible() && !entity.IsDestroyed())
                    result.Add(entity);
            }

            return result;
        }

        public static List<Entity> GetAliveEntities()
        {

            List<Entity> result = new List<Entity>();
            foreach (Entity entity in EntityManager.entities)
            {

                if (!entity.IsDestroyed())
                    result.Add(entity);
            }

            return result;
        }
    }
}