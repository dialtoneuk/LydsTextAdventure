using System;
using System.Collections.Generic;
using System.Text;

namespace LydsTextAdventure
{
    class WorldTitleScreen : World
    {
        protected char[,] buffer;

        public override void GenerateWorld()
        {

            buffer = new char[this.width, this.height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {

                    buffer[x, y] = Texture.RandomChar();
                }
            }
        }

        public override void Update()
        {

            this.GenerateWorld();
            this.Wait(4000);
        }

        public override char[,] Draw(int startx, int starty, int width, int height)
        {

            return buffer;
        }
    }
}
