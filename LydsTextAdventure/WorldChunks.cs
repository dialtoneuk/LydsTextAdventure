using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LydsTextAdventure
{
    class WorldChunks : World
    {


        protected Dictionary<Tuple<int, int>, Chunk> chunks;

        public int worldStartX;
        public int worldWidth;
        public int worldHeight;
        public int worldStartY;

        public int worldSeed
        {
            get;
        }

        public int worldOctaves
        {
            get;
        }


        private int chunkId = 0;

        public float waterLevel = 0.0075f;
        public float deepWaterLevel = 0.001f;


        public WorldChunks(int width = 12, int height = 12) : base( width, height )
        {


            Random random = new Random();

            this.waterLevel = this.waterLevel * random.Next(6, 18);
            this.worldSeed = random.Next(777, 666666666);
            this.worldOctaves = random.Next(6, 18);
            this.noise.SetSeed(this.worldSeed);
            this.noise.SetFractalOctaves(this.worldOctaves);
            this.noise.SetNoiseType(FastNoise.NoiseType.Perlin);
            this.noise.SetFrequency(0.025f);

            this.chunks = new Dictionary<Tuple<int, int>, Chunk>();
        }


        public override Tile GetTile(int x, int y)
        {
            int chunkX = x / Chunk.CHUNK_WIDTH;
            int chunkY = y / Chunk.CHUNK_HEIGHT;

            if (x > ( this.worldWidth * Chunk.CHUNK_WIDTH ) || y > ( this.worldHeight * Chunk.CHUNK_HEIGHT)
                || !this.chunks.ContainsKey(new Tuple<int, int>(chunkX, chunkY)))
                return null;

    
            return this.chunks[new Tuple<int, int>(chunkX, chunkY)].GetTileFromWorldPosition(x, y);
        }

        public bool IsAreaValid(int startx, int starty, Rectangle rect)
        {
            return this.IsAreaValid(startx, starty, rect.Width, rect.Height);
        }

        public bool IsAreaValid(int startx, int starty, int width, int height)
        {

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {

                    int realx = startx + x;
                    int realy = starty + y;

                    int chunkX = realx / Chunk.CHUNK_WIDTH;
                    int chunkY = realy / Chunk.CHUNK_HEIGHT;

                    if(chunkX < this.worldStartX || chunkY < this.worldStartY || realx > (this.worldWidth * Chunk.CHUNK_WIDTH) || realy > (this.worldHeight * Chunk.CHUNK_HEIGHT))
                    {
                        return false;
                    }
                
                    if(!this.chunks.ContainsKey(new Tuple<int, int>(chunkX, chunkY))){
                        return false;
                    }

                    if(this.chunks[new Tuple<int,int>(chunkX, chunkY)].GetTileFromWorldPosition(realx, realy).isSolid )
                    {
                        return false;
                    }

                }

            return true;
        }

        public async void CreateChunksAroundPlayer(Player player, int renderDistance = 4)
        {

            int playerChunkX = player.position.x / Chunk.CHUNK_WIDTH;
            int playerChunkY = player.position.y / Chunk.CHUNK_HEIGHT;

            for (int x = 0 - renderDistance; x < renderDistance; x++)
                for (int y = 0 - renderDistance; y < renderDistance; y++)
                    if (!this.IsRendered(playerChunkX + x, playerChunkY + y))
                    {
                        this.CreateChunk(playerChunkX + x, playerChunkY + y);

                        await Task.Run(() =>
                        {
                            this.InitializeChunk(playerChunkX + x, playerChunkY + y);
                        });

                        Chunk chunk = this.chunks[new Tuple<int, int>(playerChunkX + x, playerChunkY + y)];
                        this.UpdateChunk(ref chunk);
                    }
        }

        public void CreateChunk(int x, int y)
        {

            if (x < worldStartX)
            {
                worldWidth++;
                worldStartX = x;
            }
            else
            {
                worldWidth++;
            }


            if (y < worldStartY)
            {
                worldHeight++;
                worldStartY = y;
            } else
            {
                worldHeight++;
            }

            //create new chunk
            this.chunks[new Tuple<int, int>(x, y)] = new Chunk(x, y, chunkId);

            Program.DebugLog(string.Format("added chunk {0} x: {1} y: {2}", chunkId, x, y));

            chunkId = chunkId + 1;
        }

        private bool IsTileNearWater(int startx, int starty, int range = 1)
        {

            for(int x = 0 - range; x <=  range; x++)
            {
                for (int y = 0 - range; y <= range; y++)
                {

                    Tile tile = this.GetTile((startx + x), ( starty + y));

                    if (tile == null)
                        continue;

                    if (tile.GetType() == typeof(TileWater) || tile.GetType() == typeof(TileDeepWater))
                        return true;

                }
            }

            return false;
        }

        public void InitializeChunk(int x, int y)
        {

            if(x > this.worldWidth ||  y > this.worldHeight || x < this.worldStartX || y < this.worldStartY)
            {
                return;
            }

            int startX = Math.Abs(x * Chunk.CHUNK_WIDTH);
            int startY = Math.Abs(y * Chunk.CHUNK_HEIGHT);

            Chunk chunk = this.chunks[new Tuple<int, int>(x, y)];

            //first lets do add our water to the chunk (we will dynamically fill beaches after)
            for (int _x = 0; _x < Chunk.CHUNK_WIDTH; _x++)
                for (int _y = 0; _y < Chunk.CHUNK_HEIGHT; _y++)
                {

                    float waterLevel = this.noise.GetPerlin(startX + _x, startY + _y);
                    if (waterLevel < this.waterLevel && waterLevel > this.deepWaterLevel)
                        chunk.chunkData[_x, _y] = new TileWater();
                    else if(waterLevel < this.deepWaterLevel)
                        chunk.chunkData[_x, _y] = new TileDeepWater();
                }

            //this is used later to spawn stuff like trees and plants
            chunk.chunkNutrients = (int)Math.Floor(this.noise.GetPerlin(Math.Abs(x) + this.worldWidth, Math.Abs(y) + this.worldHeight) * this.worldOctaves);

            //how metals spawn
            chunk.chunkOre = (int)Math.Floor(this.noise.GetPerlin(Math.Abs(x / 2) + this.worldWidth, Math.Abs(y / 2) + this.worldHeight) * this.worldOctaves);

            //how metals spawn
            chunk.chunkDanger = (int)Math.Floor(this.noise.GetPerlin(Math.Abs(x * 2) + this.worldWidth, Math.Abs(y * 2) + this.worldHeight) * this.worldOctaves);

            //set its ready
            chunk.SetIsReady();

            //set again
            this.chunks[new Tuple<int, int>(x, y)] = chunk;

            Program.DebugLog(string.Format("init {0} x: {1} y: {2} n: {3} o: {4} d: {5}", chunk.chunkId, x, y, chunk.chunkNutrients, chunk.chunkOre, chunk.chunkDanger), "chunk");
        }

        public void UpdateChunk(ref Chunk chunk)
        {

            for (int _x = 0; _x < Chunk.CHUNK_WIDTH; _x++)
                for (int _y = 0; _y < Chunk.CHUNK_HEIGHT; _y++)
                {
                        
                    //add beaches and sand near water
                    if (chunk.chunkData[_x, _y].GetType() == typeof(TileGrass) && this.IsTileNearWater((chunk.chunkX * Chunk.CHUNK_WIDTH) + _x, (chunk.chunkY * Chunk.CHUNK_HEIGHT) + _y))
                    {
                        chunk.chunkData[_x, _y] = new TileSand();
                    }
                    
                }
        }

        public void UpdateChunks()
        {

            foreach(KeyValuePair<Tuple<int,int>, Chunk> pair in this.chunks)
            {

                Chunk chunk = this.chunks[pair.Key];
                this.UpdateChunk(ref chunk);
            }
        }


        public bool IsRendered(int x, int y)
        {

            try
            {
                return this.chunks.TryGetValue(new Tuple<int, int>(x, y), out Chunk _);

            } catch
            {
                return false;
            }

        }

        public override char[,] Draw(int startx, int starty, int width, int height)
        {

            char[,] result = new char[width, height];

            for (int x = 0; x < width; x++)
            {

                int actualx = x + startx;

                for (int y = 0; y < height; y++)
                {

                    int actualy = y + starty;
                    int chunkX = actualx / Chunk.CHUNK_WIDTH;
                    int chunkY = actualy / Chunk.CHUNK_HEIGHT;
                                 
                    if (actualy <= ((worldStartY) * Chunk.CHUNK_HEIGHT) || actualx <= ((worldStartX) * Chunk.CHUNK_WIDTH) || chunkX >= (worldStartX + worldWidth) 
                        || chunkY >= (worldStartY + worldHeight) || !this.chunks.ContainsKey(new Tuple<int, int>(chunkX, chunkY)))
                        result[x, y] = ' ';
                    else if(!this.chunks[new Tuple<int, int>(chunkX, chunkY)].IsReady())
                        result[x, y] = 'L';
                    else
                        result[x, y] = this.chunks[new Tuple<int, int>(chunkX, chunkY)].GetTileFromWorldPosition(actualx, actualy).texture.character;          
                }
            }
            return result;
        }

        public override Position GetInitialSpawnPoint()
        {
            
            for(int x = (worldStartX * Chunk.CHUNK_WIDTH); x < (this.worldWidth * Chunk.CHUNK_WIDTH); x++)
                for(int y = (worldStartY * Chunk.CHUNK_HEIGHT); y < (this.worldHeight * Chunk.CHUNK_HEIGHT); y++)
                {

                    if(this.IsAreaValid(x - 5, y - 5, 5, 5))
                    {
                        return new Position(x, y);
                    }
                }

            throw new ApplicationException("no start position found");
        }

        //sets up the spawn area
        public override void GenerateWorld()
        {

            this.worldWidth = this.width * 2;
            this.worldHeight = this.height * 2;
            this.worldStartX = 0;
            this.worldStartY = 0;

            //first lets just generate the spawn chunks
            for (int x = 0; x < this.width; x++)
                for (int y = 0; y < this.height; y++)
                {

                    this.chunks[new Tuple<int, int>(x, y)] = new Chunk(x, y, chunkId++);
                    this.InitializeChunk(x, y);
                }

            this.UpdateChunks();
        }
    }
}
