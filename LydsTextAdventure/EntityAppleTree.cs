using System;

namespace LydsTextAdventure
{
    public class EntityAppleTree : EntityTree
    {


        public EntityAppleTree()
        {

            this.Name = "AppleTree";
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