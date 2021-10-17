using System;

namespace LydsTextAdventure
{

    [Serializable]
    public class Tile
    {

        //need to pack texture
        [field: NonSerialized()]
        public Texture texture;

        public bool isSolid = false;

        public Tile(Texture texture = null)
        {

            if (texture != null)
                this.texture = texture;
        }
    }
}
