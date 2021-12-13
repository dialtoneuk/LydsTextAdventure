namespace LydsTextAdventure
{
    public class TileStone : Tile
    {

        public TileStone()
        {

            this.isSolid = true;
            this.texture = new Texture('S', System.ConsoleColor.Green);
        }
    }
}
