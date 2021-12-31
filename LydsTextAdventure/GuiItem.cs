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

            this.SetSize(16, 6);
        }

        public void SetItem(Item item)
        {

            this.item = item;
        }

        public override void Draw(int x, int y, Camera camera = null, Window window = null)
        {

            Surface.DrawBox(x, y, this.Width, this.Height, window.GetRectangle());

            if (this.item != null)
            {
                Surface.DrawText(x + 2 + (this.Width / 2 - 2), y + this.Height / 2 - 1, item.Icon.ToString(), window.GetRectangle());

                if (this.isHovering)
                    Surface.DrawText(x + 2, y + this.Height / 2, (item.isTradable ? "T" : "NT") + " / " + (item.isCraftable ? "C" : "NC"), window.GetRectangle());

                Surface.DrawText(x + 2, y + this.Height / 2 + 1, item.Name
                    + new string(Surface.BlankChars(this.Width - item.Name.Length - (6 + (item.ItemQuantity.ToString().Length)))) + "[" + item.ItemQuantity + "]", window.GetRectangle());
            }
            else
                Surface.DrawText(x + 2, y + this.Height / 2 + 1, "empty", window.GetRectangle());
        }
    }
}
