using System;

namespace LydsTextAdventure
{
    public class EntityAppleTree : EntityTree
    {


        public EntityAppleTree()
        {

            this.name = "AppleTree";
            this.rewards = new Type[]
            {
                  typeof(ItemWood),
                  typeof(ItemApple)
            };
            this.texture = new Texture('A');
            this.isStatic = true;
        }
    }
}