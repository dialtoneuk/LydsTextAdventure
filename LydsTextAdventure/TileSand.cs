namespace LydsTextAdventure
{
    public class TileSand : Tile
    {

        public TileSand()
        {

            this.isSolid = false;
            this.texture = new Texture('\'', System.ConsoleColor.Yellow);
        }
    }
}
