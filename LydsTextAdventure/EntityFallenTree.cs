using System;

namespace LydsTextAdventure
{
    public class EntityFallenTree : EntityTree
    {

        public EntityFallenTree()
        {

            this.Name = "FallenTree";
            this.rewards = new Type[]
            {
                  typeof(ItemStick),
                  typeof(ItemStick),
                  typeof(ItemStick)
            };

            Random rand = new Random();

            if (rand.Next(1, 100) > 50)
                this.texture = new Texture('†');
            else if (rand.Next(1, 100) > 50)
                this.texture = new Texture('\\');
            else
                this.texture = new Texture('/');


            this.isStatic = true;
        }
    }
}