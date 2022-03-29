namespace LydsTextAdventure
{
    public class TileVolcanicRock : Tile
    {

        public TileVolcanicRock()
        {

            this.isSolid = false;
            this.texture = new Texture('▒', System.ConsoleColor.DarkGray);
        }
    }
}
