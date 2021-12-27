using System;
using System.Collections.Generic;
using System.Text;

namespace LydsTextAdventure
{
    class GuiItem : GuiElement
    {

        protected Item item;


        public GuiItem()
        {

            this.SetSize(16, 3);
        }

        public void SetItem(Item item)
        {

            this.item = item;
        }

        public override void Draw(int x, int y, Camera camera = null, Window window = null)
        {

            Surface.DrawBox(x, y, this.Width, this.Height, window.GetRectangle());

            if (this.item != null)
                Surface.DrawText(x + 2, y + this.Height / 2, item.name + " [" + item.ItemQuantity + "]", window.GetRectangle());
            else
                Surface.DrawText(x + 2, y + this.Height / 2, "empty", window.GetRectangle());
        }
    }
}
