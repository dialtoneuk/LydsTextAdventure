using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LydsTextAdventure
{
    public class Entity
    {

        public const int MaxSpeed = 1000;
        public const int MaxHealth = 5042;

        public readonly Position position = new Position(0, 0);
        public readonly Texture texture = new Texture();
        public readonly string id = Guid.NewGuid().ToString();

        private string name;

        public bool visible = true;
        public bool destroyed = false;
        public bool isWaiting = false;
        public bool disabled = false;
        public bool automaticDisable = true;
        public int sleepTime = 0;
        public int health = 0;
        public int countPosition = -1;

        public Entity(string name)
        {

            if(name != "")
                this.name = name;
            else
                this.name = this.GetType().ToString();

            EntityManager.RegisterEntity(this);
        }

        public Entity()
        {

            name = this.GetType().ToString();
            EntityManager.RegisterEntity(this);
        }

        public void SetIndex(int position)
        {

            if (this.countPosition == -1 && position >= 0)
                this.countPosition = position;
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

        public virtual bool IsAutomaticDisabled()
        {

            return this.automaticDisable;
        }

        public virtual bool IsDisabled()
        {

            return this.disabled;
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

        public virtual void SetAutomaticDisable(bool val)
        {

            this.automaticDisable = val;
        }
        
        public virtual void SetDisabled(bool val)
        {

            this.disabled = val;
            Program.DebugLog("entity " + this.ToString() + " is disabled: " + val.ToString());
        }

        public virtual void Destroy()
        {

            Program.DebugLog("entity " + this.ToString() + " destroyed");
        }

        public override string ToString()
        {

            return this.id + ":" + this.name + ":" + this.GetType().ToString() + "[" + this.countPosition + "]";
        }

        public virtual void Update(int tick)
        {

            //does nothing
        }

        public virtual void Draw(int x, int y, Camera camera)
        {

#if DEBUG
            //requires camera context to be set
            Surface.DrawText(x, y + 1, this.name, camera);
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


        public virtual int GetHealth()
        {

            return this.health;
        }

        public void SetHealth(int health)
        {

            this.health = health;
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
