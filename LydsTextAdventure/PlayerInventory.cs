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

        public bool HasItem(Type type)
        {

            foreach (Item item in items)
            {

                if (item.GetType() == type)
                    return true;
            }

            return false;
        }

        public Item GetItemByType(Type type)
        {

            foreach (Item item in items)
            {

                if (item.GetType() == type)
                    return item;
            }

            throw new ApplicationException("type not found");
        }

        public Item FindMergablePartner(Type type, Item child)
        {

            foreach (Item item in items)
            {

                if (item.GetType() == type && item.ItemQuantity + child.ItemQuantity <= item.MaxQuantity)
                    return item;
            }

            return null;
        }

        public void UpdateItem(string id, Item replace)
        {


            for (int i = 0; i < items.Count; i++)
            {
                Item item = (Item)items[i];
                if (item.id == id)
                {
                    items[i] = replace;
                    break;
                }
            }
        }

        public bool TryAddItem(Item item)
        {

            Action<Item> _a = (item) =>
            {

                if (this.items.Count + 1 >= MaxCapacity)
                    return;

                items.Add(item);
                Program.DebugLog("added item " + item.Name + "[" + item.ItemQuantity + "]", "inventory");
            };

            if (this.HasItem(item.GetType()))
            {

                Item res = this.FindMergablePartner(item.GetType(), item);
                if (res == null || !res.TryMerge(item))
                    _a(item);
            }
            else
                _a(item);

            return true;
        }

    }
}