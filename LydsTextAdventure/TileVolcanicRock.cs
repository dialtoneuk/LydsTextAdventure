namespace LydsTextAdventure
{
    public class TileVolcanicRock : Tile
    {

        public TileVolcanicRock()
        {

            this.isSolid = true;
            this.texture = new Texture('▒', System.ConsoleColor.DarkGray);
        }
    }
}
