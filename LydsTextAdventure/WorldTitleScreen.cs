namespace LydsTextAdventure
{
    class WorldTitleScreen : World
    {

        public override void GenerateWorld()
        {


            for (int x = 0; x < this.width; x++)
            {
                for (int y = 0; y < this.height; y++)
                {

                    this.world[x, y] = new Tile(new Texture(Texture.RandomChar()));
                }
            }
        }

        public override void Update()
        {

            this.GenerateWorld();
            this.Wait(4000);
        }
    }
}
