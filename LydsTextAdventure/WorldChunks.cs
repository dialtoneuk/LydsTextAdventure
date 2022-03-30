using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LydsTextAdventure
{
    class WorldChunks : World
    {


        public const int MAX_NUTRIENTS = 14;
        public const int NUTRIENT_MODIFIER = 1;
        public const int FOLIAGE_CHANCE = 15;
        public const int MIN_NUTRIENTS = -12;
        public const int MAX_ENTITIES_PER_CHUNK = 16;
        public const int WORLD_START_POS = 912;

        private int worldStartX;
        private int worldWidth;
        private int worldHeight;
        private int worldStartY;

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

        public Dictionary<Tuple<int, int>, Chunk> chunks;

        public Biome Biome;
        protected List<Biome> Biomes = new List<Biome>();
        private readonly FastNoise biomeNoiseMap;
        private readonly FastNoise biomeTemperatureMap;
        private readonly Random random;


        private int chunkId = 0;

        public WorldChunks(int width = 6, int height = 6) : base(width, height)
        {

            this.SetSeed();
            this.Biomes = new List<Biome>
            {
                new BiomeApocolypse(this.seed),
                new BiomeMushrooms(this.seed),
                new BiomeFlatlands(this.seed),
                new BiomeWaterlands(this.seed),
                new BiomeGravels(this.seed),
                new BiomeOcean(this.seed),   //Disabled Until biome fading is in
                new BiomeRadioactive(this.seed),
                new BiomeMountains(this.seed),
            };

            this.random = new Random(this.seed);
            this.biomeNoiseMap = new FastNoise(this.seed);
            this.biomeTemperatureMap = new FastNoise(this.seed);
            this.chunks = new Dictionary<Tuple<int, int>, Chunk>();

            this.SetupNoiseMap();
        }

        public virtual void SetupNoiseMap()
        {

            this.biomeNoiseMap.SetFrequency(0.035f);
            this.biomeTemperatureMap.SetFrequency(0.015f);
        }


        public override Tile GetTile(int x, int y)
        {
            int chunkX = x / Chunk.CHUNK_WIDTH;
            int chunkY = y / Chunk.CHUNK_HEIGHT;

            if (!this.chunks.ContainsKey(new Tuple<int, int>(chunkX, chunkY)))
                return null;


            if (!this.chunks[new Tuple<int, int>(chunkX, chunkY)].TryGetTileFromWorldPosition(x, y, out Tile tile))
                return null;

            return tile;
        }

        public bool TryGetTile(int x, int y, out Tile tile)
        {
            int chunkX = x / Chunk.CHUNK_WIDTH;
            int chunkY = y / Chunk.CHUNK_HEIGHT;

            if (!this.chunks.ContainsKey(new Tuple<int, int>(chunkX, chunkY)))
            {
                tile = null;
                return false;
            }


            if (!this.chunks[new Tuple<int, int>(chunkX, chunkY)].TryGetTileFromWorldPosition(x, y, out tile))
                return false;

            return true;
        }

        public void SetTile(Tile tile, int x, int y)
        {

            int chunkX = x / Chunk.CHUNK_WIDTH;
            int chunkY = y / Chunk.CHUNK_HEIGHT;

            if (!this.chunks.ContainsKey(new Tuple<int, int>(chunkX, chunkY)))
                return;

            this.chunks[new Tuple<int, int>(chunkX, chunkY)].SetTileFromWorldPosition(tile, x, y);
            this.chunks[new Tuple<int, int>(chunkX, chunkY)].needsUpdate = true;
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

                    if (!this.chunks.ContainsKey(new Tuple<int, int>(chunkX, chunkY)))
                        return false;

                    if (!this.chunks[new Tuple<int, int>(chunkX, chunkY)].TryGetTileFromWorldPosition(realx, realy, out Tile tile))
                        return false;

                    if (tile.isFluid || tile.isSolid)
                        return false;
                }

            return true;
        }

        public void CreateChunksAroundPlayer(Player player, int renderDistance = 4)
        {

            int playerChunkX = player.position.x / Chunk.CHUNK_WIDTH;
            int playerChunkY = player.position.y / Chunk.CHUNK_HEIGHT;

            for (int x = 0 - renderDistance; x < renderDistance; x++)
                for (int y = 0 - renderDistance; y < renderDistance; y++)
                    if (!this.IsRendered(playerChunkX + x, playerChunkY + y))
                    {
                        this.CreateChunk(playerChunkX + x, playerChunkY + y);
                        this.InitializeChunk(playerChunkX + x, playerChunkY + y);

                        Chunk chunk = this.chunks[new Tuple<int, int>(playerChunkX + x, playerChunkY + y)];
                        chunk.generateFoliage = true;
                        this.UpdateChunk(chunk);
                        chunk.needsUpdate = true;
                    }
        }

        public void UpdateChunksAroundPlayer(Player player, int renderDistance = 2)
        {

            int playerChunkX = player.position.x / Chunk.CHUNK_WIDTH;
            int playerChunkY = player.position.y / Chunk.CHUNK_HEIGHT;

            for (int x = 0 - renderDistance; x < renderDistance; x++)
                for (int y = 0 - renderDistance; y < renderDistance; y++)
                    if (this.IsRendered(playerChunkX + x, playerChunkY + y))
                    {
                        Chunk chunk = this.chunks[new Tuple<int, int>(playerChunkX + x, playerChunkY + y)];
                        if (chunk.needsUpdate)
                            this.UpdateChunk(chunk);
                    }
        }


        public void CreateChunk(int x, int y)
        {

            this.DecideBiome(x, y);

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
            this.chunks[new Tuple<int, int>(x, y)] = new Chunk(x, y, chunkId, Biome);
            chunkId = chunkId + 1;
        }

        private bool IsTileNear(int startx, int starty, int range = 1, Type type = null)
        {

            if (type == null)
                type = typeof(TileWater);

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

            return chunk.chunkData[new Tuple<int, int>(x, y)].isPlantable == true;
        }

        public void DecideBiome(int x, int y)
        {

            float biomeValue = Math.Max(-1, Math.Min(1, this.biomeNoiseMap.GetPerlin(x, y) * 2));

            Program.DebugLog("Biome value:" + biomeValue);

            if (biomeValue >= -0.4 && biomeValue <= 0.6)
            {
                Biome = this.GetTemperateBiome(x, y);
                return;
            }

            if (biomeValue <= -0.8 || biomeValue >= 0.8)
            {

                //Sea
                Biome = this.Biomes[5];
                return;
            }

            if (biomeValue > 0)
                //transitional biome
                Biome = this.Biomes[3];
            else
                Biome = this.Biomes[4];
        }

        public Biome GetTemperateBiome(int x, int y)
        {


            float biomeValue = Math.Max(-1, Math.Min(1, this.biomeTemperatureMap.GetPerlin(x, y) * 2));
            Program.DebugLog("Temp value:" + biomeValue);

            if (biomeValue >= 0 && biomeValue <= 0.35)
            {
                return this.Biomes[2];
            }

            if (biomeValue >= 0.35 && biomeValue <= 0.55)
            {
                return this.Biomes[1];
            }

            if (biomeValue >= 0.55)
            {
                return this.Biomes[7];
            }

            if (biomeValue <= 0 && biomeValue >= -0.25)
            {
                return this.Biomes[6];
            }

            return this.Biomes[0];
        }

        public void InitializeChunk(int x, int y)
        {


            int startX = x * Chunk.CHUNK_WIDTH;
            int startY = y * Chunk.CHUNK_HEIGHT;

            Chunk chunk = this.chunks[new Tuple<int, int>(x, y)];
            chunk.chunkBiome = Biome;

            for (int tilex = 0; tilex < Chunk.CHUNK_WIDTH; tilex++)
                for (int tiley = 0; tiley < Chunk.CHUNK_HEIGHT; tiley++)
                {

                    //first lets add lakes
                    float waterLevel = Biome.GetNoiseController(Biome.NoiseController.LAKES).GetPerlin(startX + tilex, startY + tiley);

                    if (waterLevel >= Biome.waterLevel)
                        chunk.chunkData[new Tuple<int, int>(tilex, tiley)] = new TileWater(waterLevel);


                    //then lets add extra detail puddles (smaller Lakes)
                    float puddleLevel = Biome.GetNoiseController(Biome.NoiseController.PUDDLES).GetPerlin(startX + tilex, startY + tiley);

                    if (puddleLevel >= Biome.puddleLevel)
                        chunk.chunkData[new Tuple<int, int>(tilex, tiley)] = new TileWater(waterLevel);

                    //then lets add lava lakes
                    if (Biome.GenerateLava())
                    {

                        float lavaLevel = Biome.GetNoiseController(Biome.NoiseController.LAVA).GetPerlin(startX + tilex, startY + tiley);

                        if (lavaLevel >= Biome.lavaLevel)
                            chunk.chunkData[new Tuple<int, int>(tilex, tiley)] = new TileLava();

                        chunk.chunkData[new Tuple<int, int>(tilex, tiley)].RegisterTile(this, new Position(startX + tilex, startY + tiley));
                    }

                    float mountainsLevel = Biome.GetNoiseController(Biome.NoiseController.MOUNTAINS).GetPerlin(startX + tilex, startY + tiley);

                    if (mountainsLevel >= Biome.stoneLevel)
                        chunk.chunkData[new Tuple<int, int>(tilex, tiley)] = new TileStone();

                    chunk.chunkData[new Tuple<int, int>(tilex, tiley)].RegisterTile(this, new Position(startX + tilex, startY + tiley));
                }

            this.PlaceFoliage(chunk);

            if (this.IsRendered(x - 1, y - 1))
                chunks[new Tuple<int, int>(x - 1, y - 1)].needsUpdate = true;

            if (this.IsRendered(x + 1, y + 1))
                chunks[new Tuple<int, int>(x + 1, y + 1)].needsUpdate = true;

            if (this.IsRendered(x - 1, y + 1))
                chunks[new Tuple<int, int>(x - 1, y + 1)].needsUpdate = true;

            if (this.IsRendered(x + 1, y - 1))
                chunks[new Tuple<int, int>(x + 1, y - 1)].needsUpdate = true;

            //this is used later to spawn stuff like trees and plants                                                                                                                                             
            chunk.chunkNutrients = Biome.nutrientRate + (int)Math.Floor(this.noise.GetPerlin((x) + this.WorldWidth, (y) + this.WorldHeight));
            //how metals spawn
            chunk.chunkOre = Biome.oreRate + (int)Math.Floor(this.noise.GetPerlin((x / 2) + this.WorldWidth, (y / 2) + this.WorldHeight));
            //how mobs
            chunk.chunkDanger = Biome.dangerRate + (int)Math.Floor(this.noise.GetPerlin((x * 2) + this.WorldWidth, (y * 2) + this.WorldHeight));
            chunk.needsUpdate = true;

            //set again
            this.chunks[new Tuple<int, int>(x, y)] = chunk;

            Program.DebugLog(string.Format("init {0} x: {1} y: {2} n: {3} o: {4} d: {5}", chunk.chunkId, x, y, chunk.chunkNutrients, chunk.chunkOre, chunk.chunkDanger), "chunk");
        }

        private bool CheckType(Type type, Type[] types)
        {
            bool found = false;
            for (int i = 0; i < types.Length; i++)
            {
                Type t = types[i];
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
            for (int i = 0; i < types.Length; i++)
            {
                Type t = types[i];
                if (this.IsTileNear(realx, realy, width, t))
                    found = true;
            }

            return found;
        }


        public void PlaceFoliage(Chunk chunk)
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

            chunk.generateFoliage = false;
            Program.DebugLog(String.Format("done generating foliage + details with chunk {0}", chunk.chunkId));
        }

        public void UpdateChunk(Chunk chunk)
        {

            for (int _x = 0; _x < Chunk.CHUNK_WIDTH; _x++)
                for (int _y = 0; _y < Chunk.CHUNK_HEIGHT; _y++)
                {

                    int realX = (chunk.chunkX * Chunk.CHUNK_WIDTH) + _x;
                    int realY = (chunk.chunkY * Chunk.CHUNK_HEIGHT) + _y;

                    if (this.CheckType(chunk.chunkData[new Tuple<int, int>(_x, _y)].GetType(), new Type[]
                    {
                        typeof(TileGrass),
                        typeof(TileSpawnGrass),
                        typeof(TileDirt)
                    }) && this.IsNearTypes(realX, realY, new Type[]{
                        typeof(TileWater),
                    }, 1))
                    {
                        chunk.chunkData[new Tuple<int, int>(_x, _y)] = new TileSand();
                    }

                    if (this.CheckType(chunk.chunkData[new Tuple<int, int>(_x, _y)].GetType(), new Type[]{
                        typeof(TileGrass),
                        typeof(TileSpawnGrass) ,
                    }) && this.IsNearTypes(realX, realY, new Type[] { typeof(TileStone), typeof(TileVolcanicRock) }, 3))
                    {
                        chunk.chunkData[new Tuple<int, int>(_x, _y)] = new TileDirt();
                    }


                    if (this.CheckType(chunk.chunkData[new Tuple<int, int>(_x, _y)].GetType(), new Type[]{
                        typeof(TileStone),
                        typeof(TileVolcanicRock),
                    }) && this.IsNearTypes(realX, realY, typeof(TileWater), 1))
                    {
                        chunk.chunkData[new Tuple<int, int>(_x, _y)] = new TileCliff();
                    }

                    //make obsidian
                    if (this.CheckType(chunk.chunkData[new Tuple<int, int>(_x, _y)].GetType(), new Type[]
                    {
                        typeof(TileLava)
                    }) && this.IsNearTypes(realX, realY, new Type[]{
                        typeof(TileWater)
                    }, 1))
                    {
                        chunk.chunkData[new Tuple<int, int>(_x, _y)] = new TileObsidian();
                    }

                    //make volcanic rock
                    if (this.CheckType(chunk.chunkData[new Tuple<int, int>(_x, _y)].GetType(), new Type[]
                    {
                        typeof(TileLava)
                    }) && this.IsNearTypes(realX, realY, new Type[]{
                        typeof(TileGrass),
                        typeof(TileDirt),
                        typeof(TileStone),
                    }))
                    {
                        chunk.chunkData[new Tuple<int, int>(_x, _y)] = new TileVolcanicRock();
                    }

                    //make volcanic rock
                    if (this.CheckType(chunk.chunkData[new Tuple<int, int>(_x, _y)].GetType(), new Type[]
                    {
                        typeof(TileLava),
                    }) && this.IsNearTypes(realX, realY, new Type[]{
                        typeof(TileStone)
                    }))
                    {
                        chunk.chunkData[new Tuple<int, int>(_x, _y)] = new TileMagma();
                    }

                    chunk.chunkData[new Tuple<int, int>(_x, _y)].RegisterTile(this, new Position(realX, realY));
                }

            chunk.needsUpdate = false;

            if (chunk.generateFoliage)
                this.PlaceFoliage(chunk);

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

        public override Camera.TempBuffer[,] Draw(int startx, int starty, int width, int height)
        {

            Camera.TempBuffer[,] result = new Camera.TempBuffer[width, height];

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
                        result[x, y].texture = ' ';
                    else if (this.chunks.ContainsKey(new Tuple<int, int>(chunkX, chunkY)) && !this.chunks[new Tuple<int, int>(chunkX, chunkY)].IsReady())
                        result[x, y].texture = 'L';
                    else
                    {

                        if (!this.chunks[new Tuple<int, int>(chunkX, chunkY)].TryGetTileFromWorldPosition(actualx, actualy, out Tile tile))
                            result[x, y].texture = ' ';
                        else
                        {
                            Texture tex = tile.GetTexture();
                            result[x, y].texture = tex.character;
                            result[x, y].colour = tex.color;
                        }
                    }

                }
            }

            return result;
        }

        public override Position GetInitialSpawnPoint()
        {

            return this.GetInitialSpawnPoint(16);
        }

        public Position GetInitialSpawnPoint(int radius = 6)
        {

            for (int x = (WorldStartX * Chunk.CHUNK_WIDTH); x < (WorldStartX * Chunk.CHUNK_WIDTH) + (this.WorldWidth * Chunk.CHUNK_WIDTH); x++)
                for (int y = (WorldStartY * Chunk.CHUNK_HEIGHT); y < (WorldStartY * Chunk.CHUNK_HEIGHT) + (this.WorldHeight * Chunk.CHUNK_HEIGHT); y++)
                    if (this.IsAreaValid(x - radius, y - radius, radius, radius))
                        return new Position(x, y);


            return new Position(WORLD_START_POS, WORLD_START_POS);
        }

        //sets up the spawn area
        public override void GenerateWorld()
        {

            this.WorldWidth = this.width * 4;
            this.WorldHeight = this.height * 4;
            this.WorldStartX = WORLD_START_POS;
            this.WorldStartY = WORLD_START_POS;

            //first lets just generate the spawn chunks
            //2 padding for smoothing
            for (int x = -2; x < this.width + 2; x++)
            {

                int realX = x + WORLD_START_POS;


                if (realX < this.worldStartX)
                    this.worldStartX = realX;

                for (int y = -2; y < this.height + 2; y++)
                {


                    int realY = y + WORLD_START_POS;

                    if (realY < this.worldStartY)
                        this.worldStartY = realY;

                    this.CreateChunk(realX, realY);
                    this.InitializeChunk(realX, realY);
                }
            }

            this.UpdateChunks();
        }
    }
}
