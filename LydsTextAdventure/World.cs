using System;
using System.Collections.Generic;
using System.Text;

namespace LydsTextAdventure
{
    class World
    {

        public readonly int chunkSize = 24;
        public readonly int renderDistance = 8;
        public Dictionary<int[], Chunk> worldData = new Dictionary<int[], Chunk>();
        
        public World()
        {

        }

        //returns true if a chunk exists

        public bool ChunkExists(Position position, out Chunk chunk)
        {

           
            int x, y;
            if (position.x == 0)
                x = 0;
            else
                x = position.x / chunkSize;

            if (position.y == 0)
                y = 0;
            else
                y = position.y / chunkSize;

            //this is utterly retarded
            chunk = null;
            foreach (KeyValuePair<int[], Chunk> pair in this.worldData)
                if(pair.Key[0] == x && pair.Key[1] == y )
                    chunk = pair.Value;

            return chunk != null;
        }

        public static void GenerateSpawn(ref World world)
        {

            for(int x = 0 - world.renderDistance; x <= world.renderDistance; x++ )
                for (int y = 0 - world.renderDistance; y <= world.renderDistance; y++)
                {

                    Program.DebugLog(string.Concat("generating chunk ", x, "/" , y ));
                    world.worldData.Add(new int[] { x, y }, new Chunk(ref world));
                }
        }
    }
}
