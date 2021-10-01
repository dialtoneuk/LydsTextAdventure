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

        private Camera camera;

        private string name;

        public bool visible = true;
        public bool destroyed = false;
        public bool isWaiting = false;
        public int sleepTime = 0;

        public Entity(string name)
        {

            if(name != "")
                this.name = name;
            else
                this.name = this.GetType().ToString();

            EntityManager.RegisterEntity(this);
        }

        public void SetCamera(ref Camera camera)
        {

            this.camera = camera;
        }

        public Entity()
        {

            name = this.GetType().ToString();
            EntityManager.RegisterEntity(this);
        }

        public virtual void RemoveEntity()
        {

            EntityManager.RemoveEntity(this.id);
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

        public virtual bool DrawTexture()
        {

            return true;
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

        public virtual void Destroy()
        {

            Program.DebugLog("entity " + this.ToString() + " destroyed");
        }

        public override string ToString()
        {

            return this.id + ":" + this.name + ":" + this.GetType().ToString();
        }

        public virtual void Update(int tick)
        {

            //does nothing
        }

        public virtual void Draw(int x, int y)
        {

            if (this.camera == null)
                return;

#if DEBUG
            Console.BackgroundColor = ConsoleColor.Red;

            //keeps entities inside the camera
            Surface.SetCameraContext(ref this.camera);

            //requires camera context to be set
            Surface.WriteSurface(x, y + 1, "entity id: " + this.id);
            Surface.WriteSurface(x, y + 2, "entity name: " + this.name);
            Surface.WriteSurface(x, y + 3, "entity position: " + this.position.ToString());
            Surface.WriteSurface(x, y + 4, "entity type: " + this.GetType().ToString());
            Surface.WriteSurface(x, y + 5, "entity name: " + this.GetType().ToString());

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
