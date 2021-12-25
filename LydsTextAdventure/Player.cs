namespace LydsTextAdventure
{
    public class Player : Entity
    {

        protected PlayerInventory inventory = new PlayerInventory();

        public Player()
        {

            this.SetHealth(100); //default health
            this.texture = new Texture('@');
        }

        public override void Draw(int x, int y, Camera camera)
        {

            Surface.DrawText(x, y + 1, "hp: " + this.health.ToString(), camera.GetViewRectangle());

            base.Draw(x, y, camera);
        }
    }
}
