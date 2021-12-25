using System;
using System.Collections.Generic;
using System.Text;

namespace LydsTextAdventure
{
    public class Item
    {

        public string name;
        public bool isCraftable;
        public bool isTradable;
        public float value = 0;
        public string id = Guid.NewGuid().ToString();

        public int ItemQuantity { get; private set; } = 0;

        public int ItemQuality { get; private set; } = 1;

        public Item(string name = "Default Item", bool isCraftable = false, bool isTradable = true)
        {

            this.isCraftable = isCraftable;
            this.isTradable = isTradable;
            this.name = name;                         
        }

        public void SetQuality(int quality)
        {


            if (quality < 0)
                throw new ApplicationException("cannot be 0");

            this.ItemQuality = quality;
        }

        public void SetQuantity(int quantity)
        {

            if (quantity < 0)
                throw new ApplicationException("cannot be 0");

            this.ItemQuantity = quantity;
        }
    }
}
