using System;
using System.Collections.Generic;
using System.Text;

namespace LydsTextAdventure
{

    [Serializable]
    public class WorldData
    {

    
        protected Tile[,] worldData;
        protected Entity[] entities;

        public readonly string id = Guid.NewGuid().ToString();
        public readonly string worldId;
        public int worldWidth;
        public int worldHeight;
        public Type worldType;

        public WorldData(Tile[,] worldData, List<Entity> entities, World world)
        {

            this.worldData = worldData;
            this.entities = new Entity[entities.Count];
            this.worldId = world.id;
            this.worldWidth = world.width;
            this.worldHeight = world.height;
            this.worldType = world.GetType();

            int i = 0;
            foreach(Entity entity in entities)
            {

                this.entities[i++] = entity;
            } 
        }
    }
}
