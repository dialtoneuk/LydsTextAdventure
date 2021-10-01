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

            this.SetSpeed(30);
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

        public override void Draw(int x, int y)
        {

            if(this.writeOver)
                Surface.WriteOver(x, y, this.text);
            else
                Surface.Write(x, y, this.text);
        }

        public void SetText(string str)
        {

            this.text = str;
        }
    }
}
