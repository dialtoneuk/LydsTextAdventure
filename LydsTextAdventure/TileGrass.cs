using System;

namespace LydsTextAdventure
{
    public class TileGrass : Tile
    {

        public TileGrass()
        {

            this.isPlantable = true;
            this.isSolid = false;
            this.texture = new Texture('"', System.ConsoleColor.Green);
        }


        public override Texture GetTexture()
        {

            if (this.world.GetType() == typeof(WorldChunks))
            {

                WorldChunks chunks = (WorldChunks)this.world;
                this.texture.color = chunks.chunks[new Tuple<int, int>(this.position.x / Chunk.CHUNK_WIDTH, this.position.y / Chunk.CHUNK_HEIGHT)].chunkBiome.GetGrassColour();
            }

            return this.texture;
        }
    }
}
