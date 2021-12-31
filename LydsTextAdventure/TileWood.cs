namespace LydsTextAdventure
{
    public class TileWood : Tile
    {

        public TileWood()
        {

            this.isSolid = true;
            this.texture = new Texture('=', System.ConsoleColor.DarkYellow);
        }
    }
}
