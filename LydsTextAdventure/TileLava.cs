namespace LydsTextAdventure
{
    public class TileLava : Tile
    {

        public TileLava()
        {

            this.isSolid = false;
            this.texture = new Texture('▓', System.ConsoleColor.DarkRed);
        }
    }
}
