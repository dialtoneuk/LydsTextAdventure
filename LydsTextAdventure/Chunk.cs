using System;
using System.Collections.Generic;
using System.Text;

namespace LydsTextAdventure
{
    public class Chunk
    {

        public Tile[,] chunkData {
            get;
        }

        public int chunkNutrients;
        public int chunkOre;
        public int chunkDanger;

        public int chunkId
        {
            get;
        }

        public readonly int chunkX;
        public readonly int chunkY;
        private bool ready = false;

        public const int CHUNK_WIDTH = 64;
        public const int CHUNK_HEIGHT = 64;

        //the default tile we create chunks with
        public static readonly Tile defaultTile = new TileGrass();

        public Chunk(int chunkX, int chunkY, int chunkId)
        {

            this.chunkX = chunkX;
            this.chunkY = chunkY;
            this.chunkId = chunkId;
            this.chunkData = new Tile[CHUNK_WIDTH, CHUNK_HEIGHT];

            for (int x = 0; x < CHUNK_WIDTH; x++)
                for (int y = 0; y < CHUNK_HEIGHT; y++)
                    this.chunkData[x, y] = defaultTile;
        }

        public bool IsReady()
        {

            return this.ready;
        }

        public char[,] Render()
        {

            char[,] result = new char[CHUNK_WIDTH, CHUNK_HEIGHT];

            for(int x = 0; x < CHUNK_WIDTH; x++)
                for(int y = 0; y < CHUNK_HEIGHT; y++)
                {
                    result[x, y] = this.chunkData[x, y].texture.character;
                }

            return result;
        }

        public void SetIsReady()
        {

            ready = true;
        }

        public Tile GetTileFromWorldPosition(int x, int y)
        {

            int _x = x - (this.chunkX * Chunk.CHUNK_WIDTH);
            int _y = y - (this.chunkY * Chunk.CHUNK_WIDTH);
            return this.chunkData[Math.Abs(_x),Math.Abs(_y)];
        }
    }
}
