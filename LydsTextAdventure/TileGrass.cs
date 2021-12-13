namespace LydsTextAdventure
{
    public class TileGrass : Tile
    {

        public TileGrass()
        {

            this.isSolid = false;
            this.texture = new Texture('g', System.ConsoleColor.Green);
        }
    }
}
