﻿namespace LydsTextAdventure
{
    public class EntityTree : Entity
    {
        public EntityTree(string name = "Tree") : base(name)
        {

            this.texture = new Texture('^');
            this.isStatic = true;
        }

        public override void Draw(int x, int y, Camera camera)
        {

            if (this.isHovering)
                Surface.Write(x + 1, y, "[ Hovering! ]");
        }
    }
}
