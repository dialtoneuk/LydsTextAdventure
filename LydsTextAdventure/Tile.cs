using System;
using System.Collections.Generic;
using System.Text;

namespace LydsTextAdventure
{
    public class Tile
    {

        public readonly Texture texture;

        public bool isSolid = false;
        
        public Tile(Texture texture=null)
        {

            if (texture != null)
                this.texture = texture;
            else
                this.texture = new Texture();
        }
    }
}
