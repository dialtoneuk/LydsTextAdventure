using System;

namespace LydsTextAdventure
{
    public class EntityTree : Entity
    {

        public const int BASE_HARVEST_COST = 50;
        protected int randomSeed;

        protected Type[] rewards = new Type[]
        {
              typeof(ItemWood)
        };

        public EntityTree()
        {

            this.Name = "Tree";
            this.texture = new Texture('^', ConsoleColor.Green);
            this.isSolid = false;
            this.randomSeed = new Random().Next(10, 10000);
        }

        public override void Draw(int x, int y, Camera camera)
        {

            if (this.isHovering)
                Surface.Write(x + 1, y, "[Q: Harvest " + this.Name + "]");
        }

        public override void OnClick(Player player)
        {


            if (player.stanima - BASE_HARVEST_COST <= 0)
            {
                Program.DebugLog("You do not have enough energy");
                return;
            }

            player.stanima -= BASE_HARVEST_COST;

            foreach (Type reward in rewards)
                player.Inventory.TryAddItem(Item.CreateItem(reward));

            this.RemoveEntity();
        }
    }
}
