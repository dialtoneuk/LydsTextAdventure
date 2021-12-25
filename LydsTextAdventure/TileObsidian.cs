namespace LydsTextAdventure
{
    public class TileObsidian : Tile
    {

        public TileObsidian()
        {

            this.isSolid = false;
            this.texture = new Texture('0', System.ConsoleColor.Red);
        }
    }
}
