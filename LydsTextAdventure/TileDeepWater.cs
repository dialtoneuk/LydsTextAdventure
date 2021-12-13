namespace LydsTextAdventure
{
    public class TileDeepWater : Tile
    {

        public TileDeepWater()
        {

            this.isSolid = true;
            this.texture = new Texture('_', System.ConsoleColor.DarkBlue);
        }
    }
}
