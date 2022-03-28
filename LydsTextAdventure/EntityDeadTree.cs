using System;

namespace LydsTextAdventure
{
    public class EntityDeadTree : EntityTree
    {


        public EntityDeadTree()
        {

            this.Name = "DeadTree";
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