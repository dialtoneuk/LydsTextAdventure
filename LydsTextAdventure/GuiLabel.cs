﻿using System;
using System.Collections.Generic;
using System.Text;

namespace LydsTextAdventure
{
    public class GuiLabel : GuiElement
    {

        protected string text = "Label";

        public void SetText(string text)
        {

            this.text = text;
        }

        public override void Draw(int x, int y, Camera camera = null, Window window = null)
        {

            Surface.Write(x, y, this.text);
        }
    }
}
