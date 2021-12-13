namespace LydsTextAdventure
{
    public class TileWater : Tile
    {

        public TileWater()
        {

            this.isSolid = true;
            this.texture = new Texture('-', System.ConsoleColor.Blue);
        }
    }
}
