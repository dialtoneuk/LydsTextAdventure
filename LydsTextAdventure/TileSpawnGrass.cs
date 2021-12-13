namespace LydsTextAdventure
{
    public class TileSpawnGrass : Tile
    {

        public TileSpawnGrass()
        {

            this.isSolid = false;
            this.texture = new Texture('"', System.ConsoleColor.DarkGreen);
        }
    }
}
