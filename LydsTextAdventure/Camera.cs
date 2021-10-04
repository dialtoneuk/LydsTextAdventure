using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace LydsTextAdventure
{
    public class Camera : Entity
    {

        public enum Perspective : int
        {
            DEFAULT,
            CENTER_ON_OWNER
        }

        protected Entity owner;

        public readonly Position cameraPosition;
        private char[,] temporaryBuffer;
        public int width = 132;
        public int height = 52;

        protected bool drawBorder = true;
        protected bool drawTitle = true;
        protected bool mainCamera = false;

        private Camera.Perspective perspective;
        private List<Entity> renderEntities;
        private List<Entity> renderedEntities = new List<Entity>();

        public Camera(Entity entity = null, Camera.Perspective perspective = Camera.Perspective.CENTER_ON_OWNER, Position origin=null )
        {

            this.SetName("Default Camera");
            this.owner = entity;
            this.perspective = perspective;

            if (origin != null)
                this.cameraPosition = origin;
            else
                this.cameraPosition = new Position(0, 0);

            //creates the view buffer
            this.temporaryBuffer = new char[this.width, this.height];

            //cameras are not solids
            this.SetSolid(false);
            this.SetDrawTexture(false);

            //sets the name of this camera
 
            Program.DebugLog("Camera has been created", "camera");
        }

        public static void UpdateDisabled()
        {

            List<Entity> cameras = EntityManager.GetEntitiesByType(typeof(Camera));
            List<Entity> allRendered = new List<Entity>(); 

            foreach (Camera camera in cameras )
            {

                List<Entity> ents = camera.renderedEntities;

                foreach (Entity e in ents)
                    allRendered.Add(e);
            }

            foreach(Entity ent in EntityManager.GetAliveEntities())
            {

                if(ent.IsAlwaysOn() && ent.IsDisabled()){
                    ent.SetDisabled(false);
                    continue;
                }

                if(ent.GetType() == typeof(Camera))
                    continue;

                bool found = false;
                foreach(Entity comp in allRendered)
                {
                    if(ent.id == comp.id)
                    {
                        ent.SetDisabled(false);
                        found = true;
                        break;
                    }
               
                }

                if (!found){
                    ent.SetDisabled(true);
                }
            }
        }

        public void SetDrawBorder(bool draw)
        {

            this.drawBorder = draw;
        }

        public void SetDrawTitle(bool draw)
        {

            this.drawTitle = draw;
        }

        public void SetMainCamera(bool val)
        {

            this.mainCamera = val;
        }

        public bool IsMainCamera()
        {

            return this.mainCamera;
        }

        public void SetSize(int width, int height)
        {

            this.width = width;
            this.height = height;

            //recreates the view buffer
            this.temporaryBuffer = new char[this.width, this.height];
        }

        public override void Update(int tick)
        {

            if (this.perspective.Equals(Camera.Perspective.CENTER_ON_OWNER) && this.owner != null)
                this.cameraPosition.SetPosition(this.CenterOnOwner());

            this.renderedEntities.Clear();
        }

        public virtual void UpdateBuffer(List<Entity> entities)
        {

            if (this.world != null)
                this.UpdateBuffer(this.world.Draw(this.cameraPosition.x, this.cameraPosition.y, this.width, this.height), entities);
        }

        public virtual void UpdateBuffer(char[,] data, List<Entity> entities)
        {

            for(int x = 0; x < this.width; x++)
            {

                for (int y = 0; y < this.height; y++)
                {

                    this.temporaryBuffer[x, y] = data[x, y];
                }
            }

            this.RenderEntityGroup(entities);
            this.renderEntities = entities;
        }

        public Position GetViewCenter()
        {

            return new Position(this.cameraPosition.x + this.width / 2, this.cameraPosition.y + this.height / 2);
        }

        public virtual void UpdateBuffer()
        {

            if(this.world != null || this.world.IsDisabled() )
            {

                char[,] worldData = this.world.Draw(this.cameraPosition.x, this.cameraPosition.y, this.width, this.height);

                for (int x = 0; x < this.width; x++)
                {

                    for (int y = 0; y < this.height; y++)
                    {

                        this.temporaryBuffer[x, y] = worldData[x, y];
                    }
                }
            }

            this.RenderEntityGroup( EntityManager.GetVisibleEntities() );
            this.renderEntities = EntityManager.GetVisibleEntities();
        }

        private void RenderEntityGroup(List<Entity> entities)
        {

            //prepare entities for buffer
            foreach (Entity entity in entities)
            {

                if (!entity.IsVisible() || entity.IsDestroyed())
                    continue;

                if (!entity.ShouldDrawTexture())
                    continue;

                int x = entity.position.x - (this.cameraPosition.x);
                int y = entity.position.y - (this.cameraPosition.y);

                if (x < 0 || x >= width)
                    continue;

                if (y < 0 || y >= height)
                    continue;

                //draw entity texture
                this.temporaryBuffer[x, y] = entity.GetTexture().character;
            }
        }

        public void CleanBuffer()
        {

            this.temporaryBuffer = new char[this.width, this.height];
        }


        public bool IsDrawingTitle()
        {

            return this.drawTitle;
        }

        public bool IsDrawingBorder()
        {

            return this.drawBorder;
        }

        public override void Draw(int posx, int posy, Camera camera)
        {
            if (posx < 0)
                posx = 0;

            if (posy < 0)
                posy = 0;

            Buffer.AddToBuffer(Buffer.Types.WORLD_BUFFER, this.temporaryBuffer, posx, posy);

            //draw entities stuff
            if(this.renderEntities != null)
                foreach (Entity entity in this.renderEntities)
                {

                    if (entity.GetType() == typeof(Camera) )
                        continue;

                    int x = entity.position.x - (this.cameraPosition.x);
                    int y = entity.position.y - (this.cameraPosition.y);

                    if (x < 0 || x >= this.width)
                    {
                        continue;
                    }

                    if (y < 0 || y >= this.height)
                    {
                        continue;
                    } 

                    entity.Draw(x + posx, y + posy, this);
                    renderedEntities.Add(entity);
                }
        }

        public void UpdatePerspective(Camera.Perspective perspective)
        {

            this.perspective = perspective;
            Program.DebugLog("perspective changed to " + perspective.ToString(), "camera");
        }

        private Position CenterOnOwner()
        {

            if (this.owner == null)
                throw new ApplicationException("invalid owner");

            return new Position(this.owner.position.x - (int)Math.Floor((decimal)(this.width / 2) - 1),
                this.owner.position.y - (int)Math.Floor((decimal)(this.height / 2)) - 1);
        }
    }
}
