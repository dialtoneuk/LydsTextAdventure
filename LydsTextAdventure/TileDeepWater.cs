namespace LydsTextAdventure
{
    public class TileDeepWater : Tile
    {

        public TileDeepWater()
        {

            this.isSolid = true;
            this.isFluid = true;
            this.texture = new Texture('▒', System.ConsoleColor.DarkBlue);
        }
    }
}
