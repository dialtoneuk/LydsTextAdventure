namespace LydsTextAdventure
{
    public class TileGrass : Tile
    {

        public TileGrass()
        {

            this.isSolid = false;
            this.texture = new Texture('"', System.ConsoleColor.Green);
        }
    }
}
