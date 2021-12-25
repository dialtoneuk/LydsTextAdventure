namespace LydsTextAdventure
{
    public class TileWorldBorder: Tile
    {

        public TileWorldBorder()
        {

            this.isSolid = true;
            this.texture = new Texture('\\', System.ConsoleColor.White);
        }
    }
}
