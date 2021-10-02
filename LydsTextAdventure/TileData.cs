using System;
using System.Collections.Generic;
using System.Text;

namespace LydsTextAdventure
{

    [Serializable]
    public class TileData
    {

        public Type type;
        public int x;
        public int y;

        public TileData(int x, int y, Type type)
        {

            this.x = x;
            this.y = y;
            this.type = type;
        }

        //any states to be wrote here
    }
}
