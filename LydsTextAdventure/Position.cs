using System;
using System.Collections.Generic;
using System.Text;

namespace LydsTextAdventure
{
    class Position
    {

        public int x;
        public int y;

        public Position(int x, int y )
        {

            this.x = x;
            this.y = y;
        }
        
        public bool IsEqual(Position position)
        {

            return (position.x == this.x && position.y == this.y);
        }
    }
}
