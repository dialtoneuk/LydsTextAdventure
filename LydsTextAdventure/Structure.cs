using System;


namespace LydsTextAdventure
{

    public class Structure : Entity
    {

        protected Tile[,] tiles;
        public int structureId;

        public Structure()
        {

            this.shouldDrawTexture = false;
            this.isAlwaysOn = true;
            this.isSolid = true;
            this.Width = 32;
            this.Height = 32;
            this.zIndex = 10;

            this.CreateTiles();
        }

        public void CreateTiles()
        {

            this.tiles = new Tile[this.Width, this.Height];
        }

        public virtual void LoadStructure()
        {


        }

        public override void Draw(int x, int y, Camera camera)
        {

            for (int posx = 0; posx < tiles.GetLength(0); posx++)
            {
                for (int posy = 0; posy < tiles.GetLength(1); posy++)
                {

                    Surface.DrawText((posx + x), (posy + y), this.tiles[posx, posy].texture.character.ToString(), camera.GetViewRectangle(), this.tiles[posx, posy].texture.color, Buffer.Types.ENTITY_BUFFER);
                }
            }
        }

        public Tile[,] GetTiles()
        {
            return this.tiles;
        }
    }
}