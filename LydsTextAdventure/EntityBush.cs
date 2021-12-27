namespace LydsTextAdventure
{
    public class EntityBush : Entity
    {
        public EntityBush()
        {

            this.name = "Bush";
            this.texture = new Texture('b');
            this.isStatic = true;
        }

        public override void Draw(int x, int y, Camera camera)
        {

            if (this.isHovering)
                Surface.Write(x + 1, y, "[ Hovering! ]");
        }
    }
}
