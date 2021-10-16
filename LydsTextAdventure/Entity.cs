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
        public Texture texture = new Texture();
        public World world;
        public readonly string id = Guid.NewGuid().ToString();

        private string name;

        public bool visible = true;
        public bool destroyed = false;
        public bool isWaiting = false;
        public bool disabled = false;
        public bool alwaysOn = false;
        public bool isSolid = true;
        public bool drawTexture = true;

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

        public bool IsSolid()
        {

            return this.isSolid;
        }

        public void SetSolid(bool val)
        {

            this.isSolid = val;
        }

        public Entity()
        {

            name = this.GetType().ToString();
            EntityManager.RegisterEntity(this);
        }

        public void SetWorld(World world)
        {

            this.world = world;
        }

        public World GetWorld()
        {

            return this.world;
        }

        public void SetIndex(int position)
        {

            if (this.countPosition == -1 && position >= 0)
                this.countPosition = position;
        }

        public void RemoveEntity()
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

        public virtual bool ShouldDrawTexture()
        {

            return this.drawTexture;
        }

        public void SetDrawTexture(bool val)
        {

            this.drawTexture = val;
        }

        public void SetVisible(bool val)
        {

            this.visible = val;
        }

        public bool IsAlwaysOn()
        {

            return this.alwaysOn;
        }

        public bool IsDisabled()
        {

            return this.disabled;
        }

        public string GetName()
        {

            return name;
        }

        public void SetName(string name)
        {

            this.name = name;
        }

        public virtual void OnColision(Entity entity)
        {

        }

        public virtual void OnPlayerColision(Player player)
        {

        }

        public void SetAlwaysOn(bool val)
        {

            this.alwaysOn = val;
        }
        
        public void SetDisabled(bool val)
        {

            this.disabled = val;

        }

        public virtual void Destroy()
        {

            Program.DebugLog("entity " + this.ToString() + " destroyed", "entity");
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
            Surface.DrawText(x, y + 1, this.name, camera.GetViewRectangle());
#endif
        }

        public Texture GetTexture()
        {

            return this.texture;
        }


        public virtual int GetHealth()
        {

            return this.health;
        }

        public void SetHealth(int health)
        {

            this.health = health;
        }

        public bool IsVisible()
        {

            return this.visible;
        }

        public bool IsDestroyed()
        {

            return this.destroyed;
        }
    }
}
