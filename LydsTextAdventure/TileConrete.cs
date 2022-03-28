namespace LydsTextAdventure
{
    public class TileConcrete : Tile
    {

        public TileConcrete()
        {

            this.isSolid = true;
            this.texture = new Texture('█', System.ConsoleColor.Gray);
        }
    }
}
