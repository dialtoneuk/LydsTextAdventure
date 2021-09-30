using System;
using System.Collections.Generic;

namespace LydsTextAdventure
{

   public class EntityManager
   {

        private static readonly List<Entity> entities = new List<Entity>();

        public static void RegisterEntity(Entity entity)
        {

            entities.Add(entity);
            Program.DebugLog("entity " + entity.id + " has been added" );

            Program.GetCommandController().Register(entity.RegisterCommands());
            Program.DebugLog("entity " + entity.id + " has commands registered" );
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