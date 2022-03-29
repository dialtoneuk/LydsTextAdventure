namespace LydsTextAdventure
{
    public class TileCliff : Tile
    {

        public TileCliff()
        {

            this.isSolid = false;
            this.texture = new Texture('▒', System.ConsoleColor.Gray);
        }
    }
}
