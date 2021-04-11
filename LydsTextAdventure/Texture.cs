using System;
using System.Collections.Generic;
using System.Text;

namespace LydsTextAdventure
{
    class Texture
    {

        public readonly Char character;
        public readonly ConsoleColor color;

        public Texture(Char character='?', ConsoleColor color = ConsoleColor.White )
        {

            this.character = character;
            this.color = color;
        }

        public virtual void Update()
        {

        }
    }
}
