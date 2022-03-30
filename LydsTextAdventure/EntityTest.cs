using System;

namespace LydsTextAdventure
{
    public class EntityTest : Entity
    {

        protected OscMovement movement;

        public EntityTest()
        {

            this.Name = "Test";
            this.texture = new Texture('T', ConsoleColor.Red);

            Random rand = new Random();
            this.movement = new OscMovement(this, rand.Next(4, 60), rand.Next(1, 4));
        }

        public override void Update(int tick)
        {

            this.position.SetPosition(this.movement.GetPosition());
        }

        public override void Draw(int x, int y, Camera camera)
        {


            Surface.DrawBox(x, y, 6, 6, camera.GetViewRectangle(), Buffer.Types.GUI_BUFFER, true, ConsoleColor.Red);
            Surface.DrawBox(x + 1, y + 1, 4, 4, camera.GetViewRectangle(), Buffer.Types.GUI_BUFFER, true, ConsoleColor.Green);
            Surface.DrawBox(x + 2, y + 2, 2, 2, camera.GetViewRectangle(), Buffer.Types.GUI_BUFFER, true, ConsoleColor.Blue);
            base.Draw(x, y, camera);
        }
    }
}