using System;
using System.Collections.Generic;
using System.Text;

namespace LydsTextAdventure
{
    class World
    {

        public readonly int chunkSize = 24;
        public readonly int renderDistance = 8;
        public Dictionary<string, Chunk> worldData = new Dictionary<string, Chunk>();
        
        public World()
        {

        }

        //returns true if a chunk exists

        public bool HasChunkAtPosition(Position position, out Chunk chunk)
        {

           
            int x = 0, y = 0;

            if (position.x != 0)
                x = position.x / chunkSize;

            if (position.y != 0)
                y = position.y / chunkSize;

            return this.worldData.TryGetValue(string.Concat(x, "_", y), out chunk);
        }

        public static void GenerateSpawn(ref World world)
        {

            for(int x = 0 - world.renderDistance; x <= world.renderDistance; x++ )
                for (int y = 0 - world.renderDistance; y <= world.renderDistance; y++)
                {

                    Program.DebugLog(string.Concat("generating chunk ", x, "/" , y ));
                    world.worldData.Add( string.Concat(x,"_",y), new Chunk(ref world));
                }
        }
    }
}
