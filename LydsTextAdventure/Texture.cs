using System;

namespace LydsTextAdventure
{

    public class Texture
    {

        public readonly Char character;
        public readonly ConsoleColor color;

        public Texture(Char character = '?', ConsoleColor color = ConsoleColor.White)
        {

            this.character = character;
            this.color = color;
        }

        public static char RandomChar()
        {
            string chars = "_ -";
            Random rand = new Random();
            int num = rand.Next(0, chars.Length);
            return chars[num];
        }

        public virtual void Update()
        {

        }
    }
}
