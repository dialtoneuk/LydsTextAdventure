using System;

namespace LydsTextAdventure
{
    public class EntityTree : Entity
    {

        protected int randomSeed;

        protected Type[] rewards = new Type[]
        {
              typeof(ItemWood)
        };

        public EntityTree()
        {

            this.name = "Tree";
            this.texture = new Texture('^');
            this.isStatic = true;
            this.isSolid = false;
            this.randomSeed = new Random().Next(10, 10000);
        }

        public override void Draw(int x, int y, Camera camera)
        {

            if (this.isHovering)
                Surface.Write(x + 1, y, "[Q: Harvest]");
        }

        public override void OnClick(Player player)
        {

            foreach (Type reward in rewards)
                player.Inventory.TryAddItem(Item.CreateItem(reward));

            this.RemoveEntity();
        }
    }
}
