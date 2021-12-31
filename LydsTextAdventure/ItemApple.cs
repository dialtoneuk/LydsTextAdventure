using System;
using System.Collections.Generic;
using System.Text;

namespace LydsTextAdventure
{
    public class ItemApple : Item
    {

        public ItemApple()
        {

            this.MaxQuantity = 6;
            this.Name = "Apple";
            this.Value = 10.0f; //how much it is worth
            this.Icon = 'A';
        }
    }
}
