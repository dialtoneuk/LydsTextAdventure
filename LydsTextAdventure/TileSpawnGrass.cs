namespace LydsTextAdventure
{
    public class TileSpawnGrass : TileGrass
    {

        public TileSpawnGrass()
        {

            this.isSolid = false;
            this.texture = new Texture('"', System.ConsoleColor.DarkGreen);
        }
    }
}
