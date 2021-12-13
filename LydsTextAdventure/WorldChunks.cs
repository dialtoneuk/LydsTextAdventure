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

        public float waterLevel = 0.0035f;
        public float stoneLevel = 0.45f;
        public float lavaLevel = 0.65f;
        public float deepWaterLevel = 0.00005f;


        public WorldChunks(int width = 12, int height = 12) : base( width, height )
        {


            Random random = new Random();

            this.waterLevel = this.waterLevel * random.Next(6, 18);
            this.worldSeed = random.Next(777, 666666666);
            this.worldOctaves = random.Next(6, 18);
            this.noise.SetSeed(this.worldSeed);
            this.noise.SetFractalOctaves(this.worldOctaves);
            this.noise.SetNoiseType(FastNoise.NoiseType.Perlin);
            this.noise.SetFrequency(0.019f);
            this.noise.SetFractalGain(1.0f);

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

                    if(this.chunks[new Tuple<int,int>(chunkX, chunkY)].GetTileFromWorldPosition(realx, realy).IsHard() )
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

        private bool IsTileNear(int startx, int starty, int range = 1, Type type = null)
        {

            if (type == null)
                type = typeof(TileDeepWater);

            for (int x = 0 - range; x <=  range; x++)
            {
                for (int y = 0 - range; y <= range; y++)
                {

                    Tile tile = this.GetTile((startx + x), ( starty + y));

                    if (tile == null)
                        continue;

                    if (tile.GetType() == type)
                        return true;

                    //also do it for deepwater if water
                    if (type == typeof(TileWater) && tile.GetType() == typeof(TileDeepWater))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public void InitializeChunk(int x, int y, bool isSpawn = false)
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


                    if(startX + _x >= 0 && startX + _x <= 1 || startY + _y > -2 && startY + _y <= 0)
                    {
                        chunk.chunkData[_x, _y] = new TileWorldBorder();
                        continue;
                    }

                    if (isSpawn)
                    {
                        chunk.chunkData[_x, _y] = new TileSpawnGrass();
                        continue;
                    }

                    float val = this.noise.GetPerlin(startX + _x, startY + _y);
         
                    //add water
                    if (val < this.waterLevel && val > this.deepWaterLevel)
                        chunk.chunkData[_x, _y] = new TileWater();
                    else if(val < this.deepWaterLevel)
                        chunk.chunkData[_x, _y] = new TileDeepWater();

                    //add stone
                    if (val > this.waterLevel + this.stoneLevel)
                        chunk.chunkData[_x, _y] = new TileStone();

                    //add lava last
                    if (val > this.lavaLevel)
                        chunk.chunkData[_x, _y] = new TileLava();
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
                    if (( chunk.chunkData[_x, _y].GetType() == typeof(TileGrass) || chunk.chunkData[_x, _y].GetType() == typeof(TileSpawnGrass) ) && this.IsTileNear((chunk.chunkX * Chunk.CHUNK_WIDTH) + _x, (chunk.chunkY * Chunk.CHUNK_HEIGHT) + _y, 2, typeof(TileWater)))
                    {
                        chunk.chunkData[_x, _y] = new TileSand();
                        continue;
                    }

                    //place dirt near stone
                    if (( chunk.chunkData[_x, _y].GetType() == typeof(TileGrass) || chunk.chunkData[_x, _y].GetType() == typeof(TileSpawnGrass)) && this.IsTileNear((chunk.chunkX * Chunk.CHUNK_WIDTH) + _x, (chunk.chunkY * Chunk.CHUNK_HEIGHT) + _y, 2, typeof(TileStone)))
                    {
                        chunk.chunkData[_x, _y] = new TileDirt();
                        continue;
                    }

                    //place magna near lava
                    if (chunk.chunkData[_x, _y].GetType() == typeof(TileStone) && this.IsTileNear((chunk.chunkX * Chunk.CHUNK_WIDTH) + _x, (chunk.chunkY * Chunk.CHUNK_HEIGHT) + _y, 1, typeof(TileLava)))
                    {
                        chunk.chunkData[_x, _y] = new TileMagma();
                        continue;
                    }
                }

            if (chunk.fresh)
            {

                //spawn plants here
                chunk.fresh = false;
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

             return this.chunks.TryGetValue(new Tuple<int, int>(x, y), out Chunk _);
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
                    else if(this.chunks.ContainsKey(new Tuple<int, int>(chunkX, chunkY)) && !this.chunks[new Tuple<int, int>(chunkX, chunkY)].IsReady())
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

                    if(this.IsAreaValid(x - 16, y - 16, 16, 16))
                    {
                        return new Position(x + 2, y + 2);
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
            //2 padding for smoothing
            for (int x = -2; x < this.width + 2; x++)
                for (int y = -2; y < this.height + 2; y++)
                {

                    this.chunks[new Tuple<int, int>(x, y)] = new Chunk(x, y, chunkId++);
                    this.InitializeChunk(x, y, (x >= 0 && y >= 0 && x < this.width && y < this.height));
                }

            this.UpdateChunks();
        }
    }
}
