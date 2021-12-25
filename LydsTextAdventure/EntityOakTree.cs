namespace LydsTextAdventure
{
    public class EntityOakTree : EntityTree
    {
        public EntityOakTree(string name = "OakTree") : base(name)
        {

            this.texture = new Texture('O');
            this.isStatic = true;
        }
    }
}

