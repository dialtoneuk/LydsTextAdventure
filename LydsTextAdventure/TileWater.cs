namespace LydsTextAdventure
{
    public class TileWater : Tile
    {

        public TileWater()
        {

            this.isSolid = true;
            this.texture = new Texture('_', System.ConsoleColor.Blue);
        }
    }
}
