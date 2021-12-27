using System;
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

        public string name
        {
            get;
            protected set;
        }

        public bool visible = true;
        public bool destroyed = false;
        public bool isWaiting = false;
        public bool isStatic = false;
        public bool disabled = false;
        public bool alwaysOn = false;
        public bool isMarkedForDeletion = false;
        public bool isSolid = true;
        public bool isHovering = false;
        public bool drawTexture = true;

        public int width = 1;
        public int height = 1;
        public int sleepTime = 0;
        public int health = 0;
        public int countPosition = -1;

        public static bool IsMouseOver(Position position, Entity entity)
        {

            Position screenPosition = EntityManager.GetMainCamera().GetScreenPosition(entity);

            if (position.x > screenPosition.x - Math.Max(0, entity.width) && position.x < screenPosition.x + Math.Max(0, entity.width))
                if (position.y < screenPosition.y + Math.Max(0, entity.height) && position.y > screenPosition.y - Math.Max(0, entity.height))
                    return true;

            return false;
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

            this.name = this.GetType().ToString();
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

        //will try and create an Entity from its type
        public static Entity CreateEntity(Type type)
        {

            var inst = Activator.CreateInstance(type);

            if (!inst.GetType().IsSubclassOf(typeof(Entity)))
                throw new ApplicationException("invalid type created as it is not a subclass of item");

            return (Entity)inst;
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

            return this.name;
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

        public virtual void OnHover()
        {

#if DEBUG

#endif
        }

        public virtual void OnClick(Player player)
        {

#if DEBUG

#endif
        }

        public virtual void Draw(int x, int y, Camera camera)
        {

#if DEBUG
            Surface.DrawText(x, y + 1, this.name, camera.GetViewRectangle());

            if (this.isHovering)
                Surface.Write(x + 1, y + 2, "[ Hovering! ]");
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
