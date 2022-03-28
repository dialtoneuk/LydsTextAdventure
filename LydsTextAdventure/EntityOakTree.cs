﻿using System;

namespace LydsTextAdventure
{
    public class EntityOakTree : EntityTree
    {

        public EntityOakTree()
        {

            this.Name = "Name";
            this.rewards = new Type[]
            {
                typeof(ItemWood),
                typeof(ItemWood),
                typeof(ItemStick),
                typeof(ItemStick),
            };

            this.texture = new Texture('O', ConsoleColor.Green);
        }
    }
}

