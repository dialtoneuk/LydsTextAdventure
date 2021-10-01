using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LydsTextAdventure
{
    public class Entity
    {

        public const int MaxSpeed = 1024;
        public const int MaxHealth = 5042;

        public readonly Position position = new Position(0, 0);
        public readonly Texture texture = new Texture();
        public readonly string id = Guid.NewGuid().ToString();

        private Camera camera;

        private string name;

        public bool visible = true;
        public bool destroyed = false;
        public bool isWaiting = false;
        public bool outsideView = false;
        public bool updateOutsideView = true;
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

        public virtual bool IsUpdatedOutsideView()
        {

            return this.updateOutsideView;
        }

        public virtual bool IsOutsideView()
        {

            return this.outsideView;
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

        public virtual void SetUpdateOutsideView(bool val)
        {

            this.updateOutsideView = val;
        }
        
        public virtual void SetOutsideView(bool cull)
        {

            this.outsideView = cull;
            Program.DebugLog("enity " + this.ToString() + " outside view: " + cull.ToString());
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
            Surface.DrawText(x, y + 1, "entity id: " + this.id);
            Surface.DrawText(x, y + 2, "entity name: " + this.name);
            Surface.DrawText(x, y + 3, "entity type: " + this.GetType().ToString());
            Surface.DrawText(x, y + 4, "entity position: " + this.position.ToString());

            //must end
            Surface.EndCameraContext();

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
