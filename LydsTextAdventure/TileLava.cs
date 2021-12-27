namespace LydsTextAdventure
{
    public class TileLava : Tile
    {

        public TileLava()
        {

            this.isPlantable = false;
            this.isSolid = false;
            this.texture = new Texture('L', System.ConsoleColor.Red);
        }
    }
}
