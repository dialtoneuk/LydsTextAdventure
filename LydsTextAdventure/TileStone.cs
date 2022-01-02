namespace LydsTextAdventure
{
    public class TileStone : Tile
    {

        public TileStone()
        {

            this.isSolid = true;
            this.texture = new Texture('█', System.ConsoleColor.Gray);
        }
    }
}
