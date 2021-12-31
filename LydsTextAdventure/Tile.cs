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
        public bool isPlantable = false;
        public bool isFluid = false;

        public Tile(Texture texture = null)
        {

            if (texture != null)
                this.texture = texture;
        }

        public bool IsHard()
        {

            return this.isSolid;
        }
    }
}
