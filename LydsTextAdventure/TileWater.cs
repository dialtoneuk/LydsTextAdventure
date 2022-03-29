using System;
namespace LydsTextAdventure
{
    public class TileWater : Tile
    {

        public float waterValue;


        public TileWater(float waterValue = 0)
        {

            this.isSolid = false;
            this.isFluid = true;
            this.waterValue = waterValue;
            this.texture = new Texture('░', System.ConsoleColor.Blue);
        }

        public override Texture GetTexture()
        {

            if (this.world == null)
                return this.texture;

            if (this.world.GetType() == typeof(WorldChunks))
            {

                WorldChunks chunks = (WorldChunks)this.world;
                Biome biome = chunks.chunks[new Tuple<int, int>(this.position.x / Chunk.CHUNK_WIDTH, this.position.y / Chunk.CHUNK_HEIGHT)].chunkBiome;
                this.texture.color = biome.GetWaterColour();

                if (waterValue > biome.deepWaterLevel)
                    this.texture.color = biome.GetDeepWaterColour();
            }


            return this.texture;
        }
    }
}
