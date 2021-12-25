using System;
using System.Collections.Generic;
using System.Text;

namespace LydsTextAdventure
{
    public class ItemWood : Item
    {

        public ItemWood(string name = "Wood", bool isCraftable = true, bool isTradable = true) : base(name, isCraftable, isTradable)
        {

            this.value = 1.0f; //how much it is worth
        }
    }
}
