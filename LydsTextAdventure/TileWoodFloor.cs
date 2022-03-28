namespace LydsTextAdventure
{
    public class TileWoodFloor : Tile
    {

        public TileWoodFloor()
        {

            this.isSolid = false;
            this.texture = new Texture('▒', System.ConsoleColor.DarkYellow);
        }
    }
}
