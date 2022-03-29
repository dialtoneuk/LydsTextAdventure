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
        public bool isFluid = false;
        public bool isPlantable = false;

        public World world;
        public Position position;

        public Tile(Texture texture = null)
        {

            if (texture != null)
                this.texture = texture;

        }

        public void RegisterTile(World world, Position position)
        {
            this.world = world;
            this.position = position;
        }

        public virtual Texture GetTexture()
        {

            return this.texture;
        }
    }
}
