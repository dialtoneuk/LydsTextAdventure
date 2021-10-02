using System;
using System.Collections.Generic;
using System.Text;

namespace LydsTextAdventure
{

    [Serializable]
    public class WorldData
    {

        protected TileData[,] worldData;
        protected EntityData[] entities;

        public WorldData(Tile[,] worldData, List<Entity> entities, World world)
        {

            this.worldData = new TileData[world.width, world.height];

            for(int x = 0; x < worldData.GetLength(0); x++ )
            {

                for (int y = 0; y < worldData.GetLength(1); y++)
                {

                    this.worldData[x, y] = new TileData(x,y, worldData[x,y].GetType());
                }
            }

            this.entities = new EntityData[entities.Count];

            int i = 0;
            foreach(Entity entity in entities)
            {

                this.entities[i] = new EntityData(entity);
                i++;
            }    
        }
    }
}
