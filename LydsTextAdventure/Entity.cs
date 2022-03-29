namespace LydsTextAdventure
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="Entity" />.
    /// </summary>
    public class Entity
    {

        public int stanima;
        public int maxStanima = 200;
        public int stanimaRechargeRate = 12;
        /// <summary>
        /// Defines the MaxSpeed.
        /// </summary>
        public const int MaxSpeed = 1000;

        /// <summary>
        /// Defines the MaxHealth.
        /// </summary>
        public const int MaxHealth = 5042;

        public int zIndex = 5;

        public bool drawCameraBuffer = true;

        /// <summary>
        /// Defines the position.
        /// </summary>
        public readonly Position position = new Position(0, 0);

        /// <summary>
        /// Defines the texture.
        /// </summary>
        public Texture texture = new Texture();

        /// <summary>
        /// Gets or sets the world.
        /// </summary>
        public World World
        {
            get; set;
        }

        public Buffer.Types buffer = Buffer.Types.ENTITY_BUFFER;

        /// <summary>
        /// Defines the id.
        /// </summary>
        public readonly string id = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name
        {
            get; protected set;
        }

        /// <summary>
        /// Defines the visible.
        /// </summary>
        public bool isVisible = true;

        /// <summary>
        /// Defines the destroyed.
        /// </summary>
        public bool isDestroyed = false;

        /// <summary>
        /// Defines the isWaiting.
        /// </summary>
        public bool isWaiting = false;

        /// <summary>
        /// Defines the isStatic.
        /// </summary>
        public bool isStatic = false;

        /// <summary>
        /// Defines the disabled.
        /// </summary>
        public bool isDisabled = false;

        /// <summary>
        /// Defines the alwaysOn.
        /// </summary>
        public bool isAlwaysOn = false;

        /// <summary>
        /// Defines the isMarkedForDeletion.
        /// </summary>
        public bool isMarkedForDeletion = false;

        /// <summary>
        /// Defines the isSolid.
        /// </summary>
        public bool isSolid = true;

        /// <summary>
        /// Defines the isHovering.
        /// </summary>
        public bool isHovering = false;

        /// <summary>
        /// Defines the drawTexture.
        /// </summary>
        public bool shouldDrawTexture = true;

        /// <summary>
        /// Gets or sets the Width.
        /// </summary>
        public int Width { get; protected set; } = 1;

        /// <summary>
        /// Gets or sets the Height.
        /// </summary>
        public int Height { get; protected set; } = 1;

        /// <summary>
        /// Gets or sets the Health.
        /// </summary>
        public int Health { get; protected set; } = 0;

        /// <summary>
        /// Gets or sets the countPosition.
        /// </summary>
        public int CountPosition { get; protected set; } = -1;

        /// <summary>
        /// The IsMouseOver.
        /// </summary>
        /// <param name="position">The position<see cref="Position"/>.</param>
        /// <param name="entity">The entity<see cref="Entity"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public static bool IsMouseOver(Position position, Entity entity)
        {

            Position screenPosition = EntityManager.GetMainCamera().GetScreenPosition(entity);

            if (position.x > screenPosition.x - Math.Max(1, entity.Width) && position.x < screenPosition.x + Math.Max(1, entity.Width))
                if (position.y < screenPosition.y + Math.Max(1, entity.Height) && position.y > screenPosition.y - Math.Max(1, entity.Height))
                    return true;

            return false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
        /// </summary>
        public Entity()
        {

            this.Name = this.GetType().ToString();
            EntityManager.RegisterEntity(this);
        }

        /// <summary>
        /// The SetIndex.
        /// </summary>
        /// <param name="position">The position<see cref="int"/>.</param>
        public void SetIndex(int position)
        {

            if (this.CountPosition == -1 && position >= 0)
                this.CountPosition = position;
        }

        /// <summary>
        /// The RemoveEntity.
        /// </summary>
        public void RemoveEntity()
        {

            EntityManager.RemoveEntity(this.id);
        }

        /// <summary>
        /// The Wait.
        /// </summary>
        /// <param name="miliseconds">The miliseconds<see cref="int"/>.</param>
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

        /// <summary>
        /// The ShouldDrawTexture.
        /// </summary>
        /// <returns>The <see cref="bool"/>.</returns>
        public virtual bool ShouldDrawTexture()
        {

            return this.shouldDrawTexture;
        }

        //will try and create an Entity from its type
        /// <summary>
        /// The CreateEntity.
        /// </summary>
        /// <param name="type">The type<see cref="Type"/>.</param>
        /// <returns>The <see cref="Entity"/>.</returns>
        public static Entity CreateEntity(Type type)
        {

            var inst = Activator.CreateInstance(type);

            if (!inst.GetType().IsSubclassOf(typeof(Entity)))
                throw new ApplicationException("invalid type created as it is not a subclass of item");

            return (Entity)inst;
        }

        /// <summary>
        /// The SetName.
        /// </summary>
        /// <param name="name">The name<see cref="string"/>.</param>
        public void SetName(string name)
        {

            this.Name = name;
        }

        /// <summary>
        /// The OnColision.
        /// </summary>
        /// <param name="entity">The entity<see cref="Entity"/>.</param>
        public virtual void OnColision(Entity entity)
        {
        }

        /// <summary>
        /// The OnPlayerColision.
        /// </summary>
        /// <param name="player">The player<see cref="Player"/>.</param>
        public virtual void OnPlayerColision(Player player)
        {
        }

        /// <summary>
        /// The SetDisabled.
        /// </summary>
        /// <param name="val">The val<see cref="bool"/>.</param>
        public void SetDisabled(bool val)
        {

            this.isDisabled = val;
        }

        /// <summary>
        /// The Destroy.
        /// </summary>
        public virtual void Destroy()
        {

            Program.DebugLog("entity " + this.ToString() + " destroyed", "entity");
        }

        /// <summary>
        /// The ToString.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public override string ToString()
        {

            return this.id + ":" + this.Name + ":" + this.GetType().ToString() + "[" + this.CountPosition + "]";
        }

        /// <summary>
        /// The Update.
        /// </summary>
        /// <param name="tick">The tick<see cref="int"/>.</param>
        public virtual void Update(int tick)
        {
        }

        /// <summary>
        /// The OnHover.
        /// </summary>
        public virtual void OnHover()
        {
        }

        /// <summary>
        /// The OnClick.
        /// </summary>
        /// <param name="player">The player<see cref="Player"/>.</param>
        public virtual void OnClick(Player player)
        {
        }

        /// <summary>
        /// The Draw.
        /// </summary>
        /// <param name="x">The x<see cref="int"/>.</param>
        /// <param name="y">The y<see cref="int"/>.</param>
        /// <param name="camera">The camera<see cref="Camera"/>.</param>
        public virtual void Draw(int x, int y, Camera camera)
        {


        }

        /// <summary>
        /// The GetTexture.
        /// </summary>
        /// <returns>The <see cref="Texture"/>.</returns>
        public Texture GetTexture()
        {

            return this.texture;
        }

        /// <summary>
        /// The GetHealth.
        /// </summary>
        /// <returns>The <see cref="int"/>.</returns>
        public virtual int GetHealth()
        {

            return this.Health;
        }


        public int GetDistance(Entity entity)
        {

            if (entity == null || this == null)
                return -1;

            int a = Math.Abs(entity.position.x - this.position.x);
            int b = Math.Abs(entity.position.y - this.position.y);
            return (int)Math.Sqrt((a * a) + (b * b));
        }

        /// <summary>
        /// The SetHealth.
        /// </summary>
        /// <param name="health">The health<see cref="int"/>.</param>
        public void SetHealth(int health)
        {

            if (health < 0)
                health = 0;

            this.Health = health;
        }
    }
}
