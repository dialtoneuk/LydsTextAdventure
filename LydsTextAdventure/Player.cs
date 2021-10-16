using System;
using System.Collections.Generic;
using System.Text;

namespace LydsTextAdventure
{
    public class Player : Entity
    {

        public Player()
        {

            this.SetHealth(100);
            this.texture = new Texture('@');
        }

        public override void Draw(int x, int y, Camera camera)
        {

            Surface.DrawText(x, y + 1, "hp: " + this.health.ToString(), camera.GetViewRectangle());
        }
    }
}
