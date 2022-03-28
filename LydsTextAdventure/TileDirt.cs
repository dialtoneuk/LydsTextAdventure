namespace LydsTextAdventure
{
    public class TileDirt : Tile
    {

        public TileDirt()
        {

            this.isSolid = false;
            this.texture = new Texture('.', System.ConsoleColor.DarkYellow);
        }
    }
}
