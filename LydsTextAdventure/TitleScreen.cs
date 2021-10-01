using System;

namespace LydsTextAdventure
{

    public class TitleScreen
    {

        protected char[,] buffer;

        public char[,] Generate(int width, int height)
        {

            buffer = new char[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {

                    buffer[x, y] = Texture.RandomChar();
                }
            }

            return buffer;
        }

        public char[,] GetBuffer()
        {

            if (this.buffer == null)
                throw new ApplicationException("buffer is null");

            return this.buffer;
        }
    }
}