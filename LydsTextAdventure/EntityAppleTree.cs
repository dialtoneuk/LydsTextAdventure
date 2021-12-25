namespace LydsTextAdventure
{
    public class EntityAppleTree : EntityTree
    {
        public EntityAppleTree(string name = "AppleTree") : base(name)
        {

            this.texture = new Texture('A');
            this.isStatic = true;
        }
    }
}