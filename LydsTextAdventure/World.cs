using System;
using System.Threading.Tasks;

namespace LydsTextAdventure
{

    public class World
    {


        public Tile[,] world;
        public int width;
        public int height;

        public int seed
        {
            get;
            protected set;
        }

        public bool isWaiting = false;
        public bool isDisabled = false;

        public readonly string id = Guid.NewGuid().ToString();
        protected readonly FastNoise noise;

        public World(int width = 712, int height = 712)
        {

            this.noise = new FastNoise(this.seed);
            this.world = new Tile[width, height];

            this.width = width;
            this.height = height;

            if (this.seed == 0)
                this.SetSeed();

            WorldManager.RegisterWorld(this);
            this.LoadEntities();
        }

        public void SetSeed(int seed = 0)
        {

            if (seed != 0)
                this.seed = seed;

            this.seed = new Random().Next(64, int.MaxValue);
        }

        public virtual Position GetInitialSpawnPoint()
        {

            return new Position(0, 0);
        }


        public virtual Tile GetTile(int x, int y)
        {

            if (x > this.width || y > this.height || x < 0 || y < 0)
                return null;

            return this.world[x, y];
        }

        public void SaveWorld()
        {

            //saves the world to a file
        }

        public virtual void LoadWorld()
        {

            //loads the world from a file or a different place
        }

        public void SetSize(int width, int height)
        {

            this.width = width;
            this.height = height;
        }

        //returns true if a chunk exists

        public bool HasChunkAtPosition()
        {

            return true;
        }


        public virtual void LoadEntities()
        {

            //load entities here
        }

        public virtual void Update()
        {

            //updates world
        }

        public bool IsDisabled()
        {

            return this.isDisabled;
        }

        public virtual void Wait(int miliseconds)
        {

            if (this.isWaiting)
                return;

            this.isWaiting = true;
            Task.Delay(miliseconds).ContinueWith(task =>
            {
                this.isWaiting = false;
            });
        }

        public virtual Camera.TempBuffer[,] Draw(int startx, int starty, int width, int height)
        {

            Camera.TempBuffer[,] result = new Camera.TempBuffer[width, height];

            for (int x = 0; x < width; x++)
            {
                int actualx = x + startx;

                for (int y = 0; y < height; y++)
                {
                    int actualy = y + starty;

                    if (actualy < 0 || actualx < 0 || actualy >= this.height || actualx >= this.width)
                    {
                        result[x, y].texture = ' ';
                    }
                    else
                    {

                        Texture tex = this.world[actualx, actualy].GetTexture();
                        result[x, y].texture = tex.character;
                        result[x, y].colour = tex.color;
                    }
                }
            }

            return result;
        }

        public virtual void GenerateWorld()
        {

            Texture sand = new Texture(',', ConsoleColor.Cyan);
            Texture ground = new Texture('_', ConsoleColor.Gray);
            Texture stone = new Texture('.', ConsoleColor.Gray);

            for (int x = 0; x < this.width; x++)
            {

                for (int y = 0; y < this.height; y++)
                {

                    float noiseValue = this.noise.GetNoise(x, y);
                    Tile tile;


                    if (noiseValue < 0.1)
                        tile = new TileGrass();
                    else if (noiseValue < 0.2)
                        tile = new Tile(sand);
                    else if (noiseValue < 0.3)
                        tile = new Tile(ground);
                    else
                        tile = new Tile(stone);

                    this.world[x, y] = tile;
                }
            }
        }
    }
}
