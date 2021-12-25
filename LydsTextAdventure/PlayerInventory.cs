using System;
using System.Collections.Generic;

namespace LydsTextAdventure
{

    public class PlayerInventory
    {

        public List<Item> items = new List<Item>();

        public int MaxCapacity
        {
            get;
        } 
        public PlayerInventory(int startingCapacity = 100)
        {

            this.MaxCapacity = startingCapacity;
        }

        public bool TryAddItem(Item item)
        {

            if (this.items.Count == MaxCapacity)
                return false;

            items.Add(item);
            return true;
        }
    
    }
}