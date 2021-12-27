namespace LydsTextAdventure
{
    public class TileObsidian : Tile
    {

        public TileObsidian()
        {

            this.isPlantable = false;
            this.isSolid = false;
            this.texture = new Texture('0', System.ConsoleColor.Red);
        }
    }
}
