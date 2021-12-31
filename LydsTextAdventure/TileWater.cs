namespace LydsTextAdventure
{
    public class TileWater : Tile
    {

        public TileWater()
        {

            this.isSolid = true;
            this.isFluid = true;
            this.texture = new Texture('_', System.ConsoleColor.Blue);
        }
    }
}
