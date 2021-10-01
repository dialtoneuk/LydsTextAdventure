using System;
using System.Collections.Generic;
using System.Text;

namespace LydsTextAdventure
{
    public class EntityMovingText : EntityMoving
    {

        protected string text;
        protected bool writeOver = false;

        public EntityMovingText(string name = "") : base(name)
        {

            this.SetSpeed(5);
            this.SetDistance(32);
        }

        public void SetWriteOver(bool writeOver)
        {

            this.writeOver = writeOver;
        }
        
        public override bool DrawTexture()
        {

            return false;
        }

        public override void Draw(int x, int y, Camera camera)
        {
            Surface.DrawText(x, y, this.text, camera);
        }

        public void SetText(string str)
        {

            this.text = str;
        }
    }
}
