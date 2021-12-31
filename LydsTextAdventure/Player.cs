namespace LydsTextAdventure
{
    public class Player : Entity
    {

        private PlayerInventory inventory = new PlayerInventory();

        public Player()
        {

            this.SetHealth(100); //default health
            this.texture = new Texture('@');
        }

        public PlayerInventory Inventory
        {
            get => this.inventory;
            private set => this.inventory = value;
        }

        public override void Draw(int x, int y, Camera camera)
        {

            Surface.DrawText(x, y + 1, "hp: " + this.Health.ToString(), camera.GetViewRectangle());

            base.Draw(x, y, camera);
        }
    }
}
