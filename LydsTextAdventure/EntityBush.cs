namespace LydsTextAdventure
{
    public class EntityBush : Entity
    {
        public EntityBush()
        {

            this.Name = "Bush";
            this.texture = new Texture('b', System.ConsoleColor.Blue);
            this.isStatic = true;
        }

        public override void Draw(int x, int y, Camera camera)
        {

            if (this.isHovering)
                Surface.Write(x + 1, y, "[Q: Forage " + this.Name + "]");
        }
    }
}
