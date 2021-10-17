using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LydsTextAdventure
{

    public class World
    {

  
        public Tile[,] world;
        public int width;
        public int height;

        public readonly int seed;

        public bool isWaiting = false;
        public bool isDisabled = false;

        public readonly string id = Guid.NewGuid().ToString();
        private readonly FastNoise noise;

        public World(int width = 712, int height = 712)
        {

            this.seed = new Random().Next(0, int.MaxValue);
            this.noise = new FastNoise(this.seed);
            this.world = new Tile[width, height];

            this.width = width;
            this.height = height;

            this.noise.SetFrequency(0.005f);

            WorldManager.RegisterWorld(this);
            this.LoadEntities();
        }


        public Tile GetTile(int x, int y)
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

        public virtual char[,] Draw( int startx, int starty, int width, int height)
        {

            char[,] result = new char[width, height];
 
            for(int x = 0; x < width; x++)
            {
                int actualx = x + startx;

                for (int y = 0; y < height; y++)
                {
                    int actualy = y + starty;

                    if(actualy < 0 || actualx < 0 || actualy >= this.height || actualx >= this.width){
                        result[x, y] = ' ';
                    } 
                    else
                    {
                        result[x, y] = this.world[actualx, actualy].texture.character;
                    }
                }
            }
           
            return result;
        }

        public virtual void GenerateWorld()
        {

            Texture water = new Texture('~', ConsoleColor.Blue);
            Texture sand = new Texture(',', ConsoleColor.Cyan);
            Texture ground = new Texture('_', ConsoleColor.Gray);
            Texture stone = new Texture('.', ConsoleColor.Gray);

            for(int x = 0; x < this.width; x++ )
            {

                for(int y = 0; y < this.height; y++)
                {

                    float noiseValue = this.noise.GetNoise(x, y);
                    Tile tile;


                    if (noiseValue < 0.1)
                        tile = new TileWater();
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
