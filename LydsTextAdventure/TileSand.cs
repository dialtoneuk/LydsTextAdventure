namespace LydsTextAdventure
{
    public class TileSand : Tile
    {

        public TileSand()
        {

            this.isSolid = false;
            this.texture = new Texture('s', System.ConsoleColor.Yellow);
        }
    }
}
