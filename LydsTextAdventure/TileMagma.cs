namespace LydsTextAdventure
{
    public class TileMagma : Tile
    {

        public TileMagma()
        {

            this.isSolid = false;
            this.texture = new Texture('▒', System.ConsoleColor.Red);
        }
    }
}
