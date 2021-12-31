using System;
using System.Collections.Generic;
using System.Text;

namespace LydsTextAdventure
{
    public class ItemStick : Item
    {

        public ItemStick()
        {

            this.Name = "Stick";
            this.Value = 10.0f; //how much it is worth
            this.MaxQuantity = 16;
        }
    }
}
