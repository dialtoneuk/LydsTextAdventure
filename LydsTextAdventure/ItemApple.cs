using System;
using System.Collections.Generic;
using System.Text;

namespace LydsTextAdventure
{
    public class ItemApple : Item
    {

        public ItemApple(string name = "Apple", bool isCraftable = true, bool isTradable = true) : base(name, isCraftable, isTradable)
        {

            this.value = 10.0f; //how much it is worth
        }
    }
}
