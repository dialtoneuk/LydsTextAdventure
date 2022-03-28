using System;

namespace LydsTextAdventure
{
    public class EntityDeadTree : EntityTree
    {


        public EntityDeadTree()
        {

            this.Name = "Dead Tree";
            this.rewards = new Type[]
            {
                  typeof(ItemWood),
                  typeof(ItemWood),
                  typeof(ItemWood),
                  typeof(ItemStick)
            };
            this.texture = new Texture('|', ConsoleColor.DarkRed);
        }
    }
}