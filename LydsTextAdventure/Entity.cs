using System;
using System.Collections.Generic;
using System.Text;

namespace LydsTextAdventure
{
    public class Entity
    {


        public readonly Position position = new Position(0, 0);
        public readonly Texture texture = new Texture();

        public readonly string id = Guid.NewGuid().ToString();
        public bool visible = true;
        public bool destroyed = false;

        public Entity()
        {

            EntityManager.RegisterEntity(this);
        }

        public virtual void OnColision(Entity entity)
        {

        }

        public virtual void OnPlayerColision(Player player)
        {

        }

        public virtual void Draw(int x, int y)
        {

#if DEBUG
            Console.SetCursorPosition(x, y + 1);
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Write("entity id: " + this.id);
            Console.BackgroundColor = ConsoleColor.Black;
#endif
        }

        public virtual Texture GetTexture()
        {

            return this.texture;
        }

        public virtual List<Command> RegisterCommands()
        {

            return new List<Command>();
        }

        public virtual bool IsVisible()
        {

            return this.visible;
        }


        public virtual bool IsDestroyed()
        {

            return this.destroyed;
        }
    }
}
