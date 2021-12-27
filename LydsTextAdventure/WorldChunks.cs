using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LydsTextAdventure
{
    class WorldChunks : World
    {

        public const float WATER_LEVEL = 0.0035f;
        public const float STONE_LEVEL = 0.45f;
        public const float LAVA_LEVEL = 0.63f;
        public const float DEEP_WATER_LEVEL = 0.00005f;

        public const int MAX_NUTRIENTS = 12;
        public const int NUTRIENT_MODIFIER = 4;
        public const int FOLIAGE_CHANCE = 30;
        public const int MIN_NUTRIENTS = -4;
        public const int MAX_ENTITIES_PER_CHUNK = 128;

        private int worldStartX;
        private int worldWidth;
        private int worldHeight;
        private int worldStartY;

        public int WorldSeed
        {
            get;
        }

        public int WorldOctaves
        {
            get;
        }
        public int WorldStartX
        {
            get => this.worldStartX;
            private set => this.worldStartX = value;
        }
        public int WorldWidth
        {
            get => this.worldWidth;
            private set => this.worldWidth = value;
        }
        public int WorldHeight
        {
            get => this.worldHeight;
            private set => this.worldHeight = value;
        }
        public int WorldStartY
        {
            get => this.worldStartY;
            private set => this.worldStartY = value;
        }

        protected Dictionary<Tuple<int, int>, Chunk> chunks;

        public readonly Biome Biome = new Biome();
        private int chunkId = 0;

        public WorldChunks(int width = 6, int height = 6) : base(width, height)
        {


            Random random = new Random();

            this.WorldSeed = random.Next(777, 666666666);
            this.WorldOctaves = random.Next(6, 18);
            this.noise.SetSeed(this.WorldSeed);
            this.noise.SetFractalOctaves(this.WorldOctaves);
            this.noise.SetNoiseType(FastNoise.NoiseType.Perlin);
            this.noise.SetFrequency(0.019f);
            this.noise.SetFractalGain(1.0f);

            this.chunks = new Dictionary<Tuple<int, int>, Chunk>();
        }


        public override Tile GetTile(int x, int y)
        {
            int chunkX = x / Chunk.CHUNK_WIDTH;
            int chunkY = y / Chunk.CHUNK_HEIGHT;

            if (x > (this.WorldWidth * Chunk.CHUNK_WIDTH) || y > (this.WorldHeight * Chunk.CHUNK_HEIGHT)
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

                    if (chunkX < this.WorldStartX || chunkY < this.WorldStartY || realx > (this.WorldWidth * Chunk.CHUNK_WIDTH) || realy > (this.WorldHeight * Chunk.CHUNK_HEIGHT))
                        return false;

                    if (!this.chunks.ContainsKey(new Tuple<int, int>(chunkX, chunkY)))
                        return false;

                    if (this.chunks[new Tuple<int, int>(chunkX, chunkY)].GetTileFromWorldPosition(realx, realy).IsHard())

                        return false;
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
                        this.UpdateChunk(chunk);
                    }
        }

        public void CreateChunk(int x, int y)
        {

            if (x < WorldStartX)
            {
                WorldWidth++;
                WorldStartX = x;
            }
            else
            {
                WorldWidth++;
            }


            if (y < WorldStartY)
            {
                WorldHeight++;
                WorldStartY = y;
            }
            else
            {
                WorldHeight++;
            }

            //create new chunk
            this.chunks[new Tuple<int, int>(x, y)] = new Chunk(x, y, chunkId);
            chunkId = chunkId + 1;
        }

        private bool IsTileNear(int startx, int starty, int range = 1, Type type = null)
        {

            if (type == null)
                type = typeof(TileDeepWater);

            for (int x = 0 - range; x <= range; x++)
            {
                for (int y = 0 - range; y <= range; y++)
                {

                    Tile tile = this.GetTile((startx + x), (starty + y));

                    if (tile == null)
                        continue;

                    if (tile.GetType() == type)
                        return true;
                }
            }

            return false;
        }

        private bool IsTilePlantable(Chunk chunk, int x, int y)
        {

            return chunk.chunkData[x, y].isPlantable == true;
        }

        public void InitializeChunk(int x, int y, bool isSpawn = false)
        {

            int startX = x * Chunk.CHUNK_WIDTH;
            int startY = y * Chunk.CHUNK_HEIGHT;

            Chunk chunk = this.chunks[new Tuple<int, int>(x, y)];

            //first lets do add our water to the chunk (we will dynamically fill beaches after)
            for (int _x = 0; _x < Chunk.CHUNK_WIDTH; _x++)
                for (int _y = 0; _y < Chunk.CHUNK_HEIGHT; _y++)
                {


                    if (isSpawn)
                    {
                        chunk.chunkData[_x, _y] = new TileSpawnGrass();
                        continue;
                    }

                    if (startX + x < 0 || startY + y < 0)
                    {
                        chunk.chunkData[_x, _y] = new TileWorldBorder();
                        continue;
                    }
                    else if (startX + x == 0 || startY + y == 0)
                    {
                        chunk.chunkData[_x, _y] = new TileWater();
                        continue;
                    }

                    float val = this.noise.GetPerlin(startX + _x, startY + _y);

                    //add water
                    if (val < WATER_LEVEL && val > DEEP_WATER_LEVEL)
                        chunk.chunkData[_x, _y] = new TileWater();
                    else if (val < DEEP_WATER_LEVEL)
                        chunk.chunkData[_x, _y] = new TileDeepWater();

                    //add stone
                    if (val > WATER_LEVEL + STONE_LEVEL)
                        chunk.chunkData[_x, _y] = new TileStone();

                    //add lava last
                    if (val > LAVA_LEVEL)
                        chunk.chunkData[_x, _y] = new TileLava();
                }

            //this is used later to spawn stuff like trees and plants
            chunk.chunkNutrients = (int)Math.Floor(this.noise.GetPerlin((x) + this.WorldWidth, (y) + this.WorldHeight) * this.WorldOctaves);
            //how metals spawn
            chunk.chunkOre = (int)Math.Floor(this.noise.GetPerlin((x / 2) + this.WorldWidth, (y / 2) + this.WorldHeight) * this.WorldOctaves);
            //how mobs
            chunk.chunkDanger = (int)Math.Floor(this.noise.GetPerlin((x * 2) + this.WorldWidth, (y * 2) + this.WorldHeight) * this.WorldOctaves);

            //set again
            this.chunks[new Tuple<int, int>(x, y)] = chunk;

            Program.DebugLog(string.Format("init {0} x: {1} y: {2} n: {3} o: {4} d: {5}", chunk.chunkId, x, y, chunk.chunkNutrients, chunk.chunkOre, chunk.chunkDanger), "chunk");
        }

        private bool CheckType(Type type, Type[] types)
        {
            bool found = false;
            foreach (Type t in types)
            {
                if (t == type)
                    found = true;
            }

            return found;
        }

        private bool IsNearTypes(int realx, int realy, Type type, int width = 2)
        {

            return this.IsNearTypes(realx, realy, new Type[] { type }, width);
        }

        private bool IsNearTypes(int realx, int realy, Type[] types, int width = 2)
        {

            bool found = false;
            foreach (Type t in types)
            {

                if (this.IsTileNear(realx, realy, width, t))
                    found = true;
            }

            return found;
        }


        public void UpdateFreshChunk(Chunk chunk)
        {

            int nutrientRate = Math.Min(MAX_NUTRIENTS, Math.Max(MIN_NUTRIENTS, chunk.chunkNutrients));
            int entityCount = 0;

            Type[] types = Biome.GetFoliageTypes(nutrientRate);

            if (types.Length == 0)
                return;

            for (int x = 0; x < Chunk.CHUNK_WIDTH; x++)
            {
                for (int y = 0; y < Chunk.CHUNK_HEIGHT; y++)
                {
                    if (!this.Biome.CanSeed(nutrientRate, FOLIAGE_CHANCE + (nutrientRate * NUTRIENT_MODIFIER)) || !this.IsTilePlantable(chunk, x, y))
                        continue;

                    if (entityCount > MAX_ENTITIES_PER_CHUNK)
                        break;

                    Entity foliage = Entity.CreateEntity(types[Biome.biomeRandom.Next(0, types.Length - 1)]);
                    foliage.position.x = chunk.GetX(x);
                    foliage.position.y = chunk.GetY(y);

                    entityCount++;
                }
            }
        }

        public void UpdateChunk(Chunk chunk)
        {

            for (int _x = 0; _x < Chunk.CHUNK_WIDTH; _x++)
                for (int _y = 0; _y < Chunk.CHUNK_HEIGHT; _y++)
                {

                    int realX = (chunk.chunkX * Chunk.CHUNK_WIDTH) + _x;
                    int realY = (chunk.chunkY * Chunk.CHUNK_HEIGHT) + _y;

                    if (this.CheckType(chunk.chunkData[_x, _y].GetType(), new Type[]
                    {
                        typeof(TileGrass),
                        typeof(TileSpawnGrass),
                        typeof(TileDirt)
                    }) && this.IsNearTypes(realX, realY, new Type[]{
                        typeof(TileWater),
                        typeof(TileDeepWater)
                    }, 2))
                    {
                        chunk.chunkData[_x, _y] = new TileSand();
                    }

                    if (this.CheckType(chunk.chunkData[_x, _y].GetType(), new Type[]{
                        typeof(TileGrass),
                        typeof(TileSpawnGrass)
                    }) && this.IsNearTypes(realX, realY, typeof(TileStone)))
                    {
                        chunk.chunkData[_x, _y] = new TileDirt();
                    }

                    if (this.CheckType(chunk.chunkData[_x, _y].GetType(), new Type[]{
                        typeof(TileStone)
                    }) && this.IsNearTypes(realX, realY, typeof(TileLava)))
                    {
                        chunk.chunkData[_x, _y] = new TileMagma();
                    }

                    //make obsidian
                    if (this.CheckType(chunk.chunkData[_x, _y].GetType(), new Type[]
                    {
                        typeof(TileLava)
                    }) && this.IsNearTypes(realX, realY, new Type[]{
                        typeof(TileGrass),
                        typeof(TileDirt),
                        typeof(TileWater),
                        typeof(TileDeepWater)
                    }, 1))
                    {
                        chunk.chunkData[_x, _y] = new TileObsidian();
                    }
                }

            if (chunk.fresh)
            {

                this.UpdateFreshChunk(chunk);
                //spawn plants here
                chunk.fresh = false;
            }

            chunk.SetIsReady();
        }

        public void UpdateChunks()
        {

            foreach (KeyValuePair<Tuple<int, int>, Chunk> pair in this.chunks)
            {
                Chunk chunk = this.chunks[pair.Key];
                this.UpdateChunk(chunk);
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


                    if (actualy <= ((WorldStartY) * Chunk.CHUNK_HEIGHT) || actualx <= ((WorldStartX) * Chunk.CHUNK_WIDTH) || chunkX >= (WorldStartX + WorldWidth)
                        || chunkY >= (WorldStartY + WorldHeight) || !this.chunks.ContainsKey(new Tuple<int, int>(chunkX, chunkY)))
                        result[x, y] = ' ';
                    else if (this.chunks.ContainsKey(new Tuple<int, int>(chunkX, chunkY)) && !this.chunks[new Tuple<int, int>(chunkX, chunkY)].IsReady())
                        result[x, y] = 'L';
                    else
                        result[x, y] = this.chunks[new Tuple<int, int>(chunkX, chunkY)].GetTileFromWorldPosition(actualx, actualy).texture.character;
                }
            }
            return result;
        }

        public override Position GetInitialSpawnPoint()
        {

            for (int x = (WorldStartX * Chunk.CHUNK_WIDTH); x < (this.WorldWidth * Chunk.CHUNK_WIDTH); x++)
                for (int y = (WorldStartY * Chunk.CHUNK_HEIGHT); y < (this.WorldHeight * Chunk.CHUNK_HEIGHT); y++)
                    if (this.IsAreaValid(x - 16, y - 16, 16, 16))
                        return new Position(x, y);


            throw new ApplicationException("no start position found");
        }

        //sets up the spawn area
        public override void GenerateWorld()
        {

            this.WorldWidth = this.width * 2;
            this.WorldHeight = this.height * 2;
            this.WorldStartX = 1;
            this.WorldStartY = 1;

            //first lets just generate the spawn chunks
            //2 padding for smoothing
            for (int x = -2; x < this.width + 2; x++)
                for (int y = -2; y < this.height + 2; y++)
                {

                    this.chunks[new Tuple<int, int>(x, y)] = new Chunk(x, y, chunkId++);
                    this.InitializeChunk(x, y, false);
                }

            this.UpdateChunks();
        }
    }
}
