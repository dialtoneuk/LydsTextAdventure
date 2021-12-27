using System;

namespace LydsTextAdventure
{
    public class EntityDeadTree : EntityTree
    {


        public EntityDeadTree()
        {

            this.name = "DeadTree";
            this.rewards = new Type[]
            {
                  typeof(ItemWood),
                  typeof(ItemStick)
            };
            this.texture = new Texture('|');
            this.isStatic = true;
        }
    }
}