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

            if ((tick % Math.Max(1, 10 - this.stanimaRechargeRate) == 0) && this.stanima <= this.maxStanima)
                this.stanima++;

            this.Wait(10); //wait 10ms before doing it again;
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

            color = System.ConsoleColor.Green;
            if (this.stanima <= 50)
                color = System.ConsoleColor.Yellow;
            else if (this.stanima <= 30)
                color = System.ConsoleColor.Red;
            else if (this.stanima < 15)
                color = System.ConsoleColor.DarkRed;

            Surface.DrawText(x, y + 3, "stanima: " + this.stanima, camera.GetViewRectangle(), color);
#endif

            base.Draw(x, y, camera);
        }
    }
}
