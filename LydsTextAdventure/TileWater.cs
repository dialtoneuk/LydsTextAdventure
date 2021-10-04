using System;
using System.Collections.Generic;
using System.Text;

namespace LydsTextAdventure
{
    public class TileWater : Tile
    {

        public TileWater()
        {

            this.isSolid = true;
            this.texture = new Texture(' ');
        }
    }
}
