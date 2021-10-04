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
        
        public bool IsEqual(int x_, int y_)
        {

            return (this.x == x_ && this.y == y_);
        }

        public override bool Equals(object obj)
        {

            Position pos = (Position)obj;
            return pos.GetHashCode() == this.GetHashCode();
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

        public override int GetHashCode()
        {
            return HashCode.Combine(x, y);
        }
    }
}
