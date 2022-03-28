namespace LydsTextAdventure
{
    public class EntityPsychedelicMushroom : Entity
    {
        public EntityPsychedelicMushroom()
        {

            this.Name = "Magic Mushroom";
            this.texture = new Texture('P', System.ConsoleColor.White);
        }

        public override void Draw(int x, int y, Camera camera)
        {

            if (this.isHovering)
                Surface.Write(x + 1, y, "[Q: Harvest Magic Mushroom ]");
        }
    }
}
