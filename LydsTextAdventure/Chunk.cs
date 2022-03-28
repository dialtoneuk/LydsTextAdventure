using System;
using System.Collections.Generic;
using System.Text;

namespace LydsTextAdventure
{
    public class Chunk
    {

        public Dictionary<Tuple<int, int>, Tile> chunkData
        {
            get;
        }

        public bool fresh = true;

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

        public const int CHUNK_WIDTH = 31;
        public const int CHUNK_HEIGHT = 31;

        //the default tile we create chunks with
        public readonly Tile defaultTile = new TileGrass();

        public Chunk(int chunkX, int chunkY, int chunkId, Biome biome)
        {

            this.chunkX = chunkX;
            this.chunkY = chunkY;
            this.chunkId = chunkId;
            this.chunkData = new Dictionary<Tuple<int, int>, Tile>();
            defaultTile.texture.color = biome.GetGrassColour();

            for (int x = 0; x < CHUNK_WIDTH; x++)
                for (int y = 0; y < CHUNK_HEIGHT; y++)
                    this.chunkData[new Tuple<int, int>(x, y)] = defaultTile;
        }

        public int GetX(int startx)
        {

            return (this.chunkX * CHUNK_WIDTH) + startx;
        }

        public int GetY(int starty)
        {

            return (this.chunkY * CHUNK_HEIGHT) + starty;
        }

        public bool IsReady()
        {

            return this.ready;
        }

        public char[,] Render()
        {

            char[,] result = new char[CHUNK_WIDTH, CHUNK_HEIGHT];

            for (int x = 0; x < CHUNK_WIDTH; x++)
                for (int y = 0; y < CHUNK_HEIGHT; y++)
                {
                    result[x, y] = this.chunkData[new Tuple<int, int>(x, y)].texture.character;
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
            return this.chunkData[new Tuple<int, int>(_x, _y)];
        }


        public bool TryGetTileFromWorldPosition(int x, int y, out Tile value)
        {
            int _x = x - (this.chunkX * Chunk.CHUNK_WIDTH);
            int _y = y - (this.chunkY * Chunk.CHUNK_WIDTH);
            return this.chunkData.TryGetValue(new Tuple<int, int>(_x, _y), out value);
        }

        public void SetTileFromWorldPosition(Tile tile, int x, int y)
        {
            int _x = x - (this.chunkX * Chunk.CHUNK_WIDTH);
            int _y = y - (this.chunkY * Chunk.CHUNK_WIDTH);
            this.chunkData[new Tuple<int, int>(_x, _y)] = tile;
        }
    }
}
