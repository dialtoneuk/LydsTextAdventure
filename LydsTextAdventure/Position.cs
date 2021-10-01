using System;
using System.Collections.Generic;
using System.Text;

namespace LydsTextAdventure
{
    public class Position
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
         
        public void SetPosition(Position position)
        {

            this.x = position.x;
            this.y = position.y;
        }

        public override string ToString()
        {
            return "x/y: " + this.x + " / " + this.y;
        }
    }
}
