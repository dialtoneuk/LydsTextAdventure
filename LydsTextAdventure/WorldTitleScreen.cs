using System;
using System.Collections.Generic;
using System.Text;

namespace LydsTextAdventure
{
    class WorldTitleScreen : World
    {

        public override void GenerateWorld()
        {

 
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {

                    this.world[x, y] = new Tile(new Texture(Texture.RandomChar()));
                }
            }
        }

        public override void Update()
        {

            this.GenerateWorld();
            this.Wait(4000);
        }
    }
}
