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

#if DEBUG

            System.ConsoleColor color = System.ConsoleColor.Green;

            if (Buffer.FPS <= 40)
                color = System.ConsoleColor.Yellow;
            else if (Buffer.FPS <= 20)
                color = System.ConsoleColor.Red;

            Surface.DrawText(x, y + 2, "fps: " + Buffer.FPS, camera.GetViewRectangle(), color);
#endif

            base.Draw(x, y, camera);
        }
    }
}
