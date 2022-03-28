namespace LydsTextAdventure
{
    public class EntityMushroom : Entity
    {
        public EntityMushroom()
        {

            this.Name = "Mushroom";
            this.texture = new Texture('M', System.ConsoleColor.White);
        }

        public override void Draw(int x, int y, Camera camera)
        {

            if (this.isHovering)
                Surface.Write(x + 1, y, "[Q: Pick Mushroom]");
        }
    }
}
