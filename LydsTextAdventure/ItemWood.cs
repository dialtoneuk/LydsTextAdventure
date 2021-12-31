using System;
using System.Collections.Generic;
using System.Text;

namespace LydsTextAdventure
{
    public class ItemWood : Item
    {

        public ItemWood()
        {

            this.MaxQuantity = 256; //carry a lot of wood
            this.Name = "Wood";
            this.Value = 1.0f; //how much it is worth
            this.Icon = '=';
        }
    }
}
