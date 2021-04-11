using System;
using System.Collections.Generic;
using System.Text;

namespace LydsTextAdventure
{
    class Chunk
    {

        private Tile[,] tiles;
        private int chunkSize;

        public Chunk(ref World world, bool autoGenerate = true)
        {

            this.chunkSize = world.chunkSize;
            this.tiles = new Tile[this.chunkSize, this.chunkSize];
       

            if (autoGenerate)
                this.GenerateChunk(ref world);
        }


        public Tile GetTile(int x, int y )
        {

            return tiles[x, y];
        }
     
        public void GenerateChunk(ref World world)
        {

            Random rnd = new Random();
            Char room = (char)rnd.Next('Z', 'a');

            for (int x = 0; x < this.chunkSize; x++)
                for (int y = 0; y < this.chunkSize; y++)
                    this.tiles[x, y] = new Tile(new Texture(room)); 
        }
    }
}
