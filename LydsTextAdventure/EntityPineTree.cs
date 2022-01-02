using System;

namespace LydsTextAdventure
{
    public class EntityPineTree : EntityTree
    {


        public EntityPineTree()
        {

            this.Name = "PineTree";
            this.rewards = new Type[]
            {
                  typeof(ItemWood),
                  typeof(ItemWood),
                  typeof(ItemWood),
                  typeof(ItemWood),
                  typeof(ItemWood),
                  typeof(ItemStick),
            };
            this.texture = new Texture('P', ConsoleColor.DarkGreen);
            this.isStatic = true;
        }
    }
}