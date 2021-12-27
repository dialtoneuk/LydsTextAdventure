namespace LydsTextAdventure
{
    public class TileGrass : Tile
    {

        public TileGrass()
        {

            this.isPlantable = true;
            this.isSolid = false;
            this.texture = new Texture('"', System.ConsoleColor.Green);
        }
    }
}
