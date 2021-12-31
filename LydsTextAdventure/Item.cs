using System;
using System.Collections.Generic;
using System.Text;

namespace LydsTextAdventure
{
    public class Item
    {

        public string Name
        {

            get;
            protected set;
        } = "Default Item";

        public bool isCraftable = true;
        public bool isTradable = true;
        public float Value
        {
            get;
            protected set;
        }

        public readonly string id = Guid.NewGuid().ToString();

        public char Icon
        {
            get;
            protected set;
        } = '?';

        public int ItemQuantity { get; protected set; } = 1;
        public int MaxQuantity { get; protected set; } = 32;

        public int ItemQuality { get; protected set; } = 1;
        //will try and create an Item from its type
        public static Item CreateItem(Type type)
        {

            var inst = Activator.CreateInstance(type);

            if (!inst.GetType().IsSubclassOf(typeof(Item)))
                throw new ApplicationException("invalid type created as it is not a subclass of item");

            return (Item)inst;
        }

        public bool TryMerge(Item item)
        {

            if (this.GetType() != item.GetType())
                return false;

            if (this.ItemQuantity + item.ItemQuantity > this.MaxQuantity)
                return false;

            this.ItemQuantity += item.ItemQuantity;
            Program.DebugLog("merged item " + this.Name + "[" + this.ItemQuantity + "]", "inventory");
            return true;
        }

        public void SetQuality(int quality)
        {


            if (quality < 0)
                throw new ApplicationException("cannot be 0");

            this.ItemQuality = quality;
        }

        public void SetQuantity(int quantity)
        {

            if (quantity < 0 && quantity > MaxQuantity)
                throw new ApplicationException("cannot be 0 or larger than " + MaxQuantity);

            this.ItemQuantity = quantity;
        }
    }
}
