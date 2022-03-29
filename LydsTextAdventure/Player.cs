using System;

namespace LydsTextAdventure
{
    public class Player : Entity
    {

        private PlayerInventory inventory = new PlayerInventory();

        public Player()
        {

            this.SetHealth(100); //default health
            this.texture = new Texture('@');
            this.zIndex = 0;
            this.stanima = 100;
            this.drawCameraBuffer = false;
            this.buffer = Buffer.Types.TOP_BUFFER;
        }

        public PlayerInventory Inventory
        {
            get => this.inventory;
            private set => this.inventory = value;
        }

        public override void Update(int tick)
        {

            Random rand = new Random();
            if (this.stanima <= this.maxStanima)
                this.stanima += rand.Next(this.stanimaRechargeRate / 2, this.stanimaRechargeRate);

            if (this.stanima > this.maxStanima)
                this.stanima = this.maxStanima;

            this.Wait(100); //wait a bit;
        }

        public override void Draw(int x, int y, Camera camera)
        {

#if DEBUG
            System.ConsoleColor color = System.ConsoleColor.Green;

            if (Buffer.FPS <= 40)
                color = System.ConsoleColor.Yellow;
            else if (Buffer.FPS <= 20)
                color = System.ConsoleColor.Red;

            Surface.DrawText(x, y - 2, Buffer.FPS.ToString(), camera.GetViewRectangle(), color);

#endif

            base.Draw(x, y, camera);
        }
    }
}
