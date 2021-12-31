namespace LydsTextAdventure
{
    public class TileWater : Tile
    {

        public TileWater()
        {

            this.isSolid = false;
            this.isFluid = true;
            this.texture = new Texture('░', System.ConsoleColor.Blue);
        }
    }
}
