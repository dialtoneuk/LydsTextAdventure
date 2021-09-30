using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LydsTextAdventure
{
    public class Entity
    {

        public const int MaxSpeed = 5042;
        public const int MaxHealth = 5042;

        public readonly Position position = new Position(0, 0);
        public readonly Texture texture = new Texture();
        public readonly string id = Guid.NewGuid().ToString();

        private string name = "Entity";

        public bool visible = true;
        public bool destroyed = false;
        public bool isWaiting = false;
        public int sleepTime = 0;

        public Entity()
        {

            EntityManager.RegisterEntity(this);
        }

        public virtual void Wait(int miliseconds)
        {

            if (this.isWaiting)
                return;

            this.isWaiting = true;
            Task.Delay(miliseconds).ContinueWith(task =>
            {
                this.isWaiting = false;
            });
        }

        public virtual string GetName()
        {

            return name;
        }

        public virtual void SetName(string name)
        {

            this.name = name;
        }

        public virtual void OnColision(Entity entity)
        {

        }

        public virtual void OnPlayerColision(Player player)
        {

        }

        public virtual void Update(int tick)
        {

        }

        public virtual void Draw(int x, int y)
        {

#if DEBUG
            Console.SetCursorPosition(x, y + 1);
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Write("entity id: " + this.id);
            Console.SetCursorPosition(x, y + 2);
            Console.Write("entity name: " + this.name);
            Console.SetCursorPosition(x, y + 3);
            Console.Write("entity position: " + this.position.ToString());
            Console.SetCursorPosition(x, y + 4);
            Console.Write("entity type: " + this.GetType().ToString());
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
