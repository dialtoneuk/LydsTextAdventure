using System;
using System.Collections.Generic;
using System.Text;

namespace LydsTextAdventure
{
    public class Player : Entity
    {

        public new Texture texture = new Texture('@');

        public Player()
        {

            this.SetHealth(100);
        }

        public override void Draw(int x, int y, Camera camera)
        {

            Surface.DrawText(x, y + 1, "hp: " + this.health.ToString(), camera);
        }

        public override Texture GetTexture()
        {
            return this.texture;
        }
    }
}
