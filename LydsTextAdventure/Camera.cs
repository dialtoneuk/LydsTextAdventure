using System;
using System.Collections.Generic;
using System.Linq;

namespace LydsTextAdventure
{
    public class Camera : Entity
    {

        public enum Perspective : int
        {
            DEFAULT,
            CENTER_ON_OWNER
        }

        public struct TempBuffer
        {
            public char texture;
            public ConsoleColor colour;
        }

        //holds a list of all of our cameras
        private static List<Entity> cameras = new List<Entity>();

        protected Entity owner;
        protected bool mainCamera = false;

        public readonly Position cameraPosition;

        private TempBuffer[,] temporaryBuffer;
        private Camera.Perspective perspective;
        public List<Entity> renderEntities;
        public readonly List<Entity> renderedEntities = new List<Entity>();

        public Camera(Entity entity = null, Camera.Perspective perspective = Camera.Perspective.CENTER_ON_OWNER, Position origin = null)
        {

            this.SetName("Default Camera");
            this.owner = entity;
            this.perspective = perspective;

            if (origin != null)
                this.cameraPosition = origin;
            else
                this.cameraPosition = new Position(0, 0);

            //creates the view buffer
            this.temporaryBuffer = new TempBuffer[this.Width, this.Height];

            //cameras are not solids
            this.isSolid = false;
            this.shouldDrawTexture = false;
            this.SetSize(32, 32);
            //sets the name of this camera

            Program.DebugLog("Camera has been created", "camera");
            SceneManager.CurrentScene.AddSceneCamera(this);
            cameras = SceneManager.CurrentScene.GetSceneCameras();
        }

        public Rectangle GetViewRectangle()
        {

            return new Rectangle(this.Width, this.Height);
        }
        public static void UpdateDisabled()
        {


            List<Entity> allRendered = new List<Entity>();

            foreach (Camera camera in cameras)
            {

                List<Entity> ents = camera.renderedEntities;

                foreach (Entity e in ents)
                    allRendered.Add(e);
            }

            List<Entity> list = EntityManager.GetAliveEntities();
            for (int i1 = 0; i1 < list.Count; i1++)
            {

                Entity ent = list[i1];

                if (ent == null)
                    continue;

                if (ent.isAlwaysOn && ent.isDisabled)
                {
                    ent.SetDisabled(false);
                    continue;
                }

                if (ent.GetType() == typeof(Camera))
                    continue;

                bool found = false;
                for (int i = 0; i < allRendered.Count; i++)
                {
                    Entity comp = allRendered[i];
                    if (ent.id == comp.id)
                    {
                        ent.SetDisabled(false);
                        found = true;
                        break;
                    }

                }

                if (!found)
                {
                    ent.SetDisabled(true);
                }
            }
        }

        public Position GetScreenPosition(Entity entity)
        {


            int x = entity.position.x - (this.cameraPosition.x) + 1;
            int y = entity.position.y - (this.cameraPosition.y) + 1;

            Position position = new Position(x, y);
            return position;
        }

        public Position GetMousePosition()
        {

            Position mouse = InputController.GetMousePosition();
            int x = mouse.x + (this.cameraPosition.x) - 1;
            int y = mouse.y + (this.cameraPosition.y) - 1;

            Position position = new Position(x, y);
            return position;
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

            this.Width = width;
            this.Height = height;

            //recreates the view buffer
            this.temporaryBuffer = new TempBuffer[this.Width, this.Height];
        }

        public override void Update(int tick)
        {

            if (this.perspective.Equals(Camera.Perspective.CENTER_ON_OWNER) && this.owner != null)
                this.cameraPosition.SetPosition(this.CenterOnOwner());

            this.renderedEntities.Clear();
        }

        public virtual void UpdateBuffer(List<Entity> entities)
        {

            if (this.World != null)
                this.UpdateBuffer(entities);
        }

        public virtual void UpdateBuffer(char[,] data, List<Entity> entities)
        {

            for (int x = 0; x < this.Width; x++)
            {

                for (int y = 0; y < this.Height; y++)
                {

                    this.temporaryBuffer[x, y].texture = data[x, y];
                }
            }

            this.RenderEntityGroup(entities);
            this.renderEntities = entities;
        }

        public Position GetViewCenter()
        {

            return new Position(this.cameraPosition.x + this.Width / 2, this.cameraPosition.y + this.Height / 2);
        }

        public virtual void UpdateBuffer()
        {

            if (this.World != null && !this.World.IsDisabled())
            {

                TempBuffer[,] worldData = this.World.Draw(this.cameraPosition.x, this.cameraPosition.y, this.Width, this.Height);

                for (int x = 0; x < this.Width; x++)
                {

                    for (int y = 0; y < this.Height; y++)
                    {

                        if (x >= worldData.GetLength(0) || y >= worldData.GetLength(1))
                        {
                            continue;
                        }

                        Tile tile = this.World.GetTile(this.cameraPosition.x + x, this.cameraPosition.y + y);
                        ConsoleColor colour;

                        if (tile == null)
                            colour = ConsoleColor.Black;
                        else
                            colour = tile.texture.color;

                        this.temporaryBuffer[x, y].texture = worldData[x, y].texture;
                        this.temporaryBuffer[x, y].colour = colour;
                    }
                }
            }

            if (EntityManager.GetVisibleEntities() != null)
            {
                List<Entity> entities = EntityManager.GetVisibleEntities().OrderBy((entity) => entity.zIndex).Reverse().ToList();
                this.RenderEntityGroup(entities);
                this.renderEntities = entities;
            }


        }

        private void RenderEntityGroup(List<Entity> entities)
        {

            //prepare entities for buffer
            for (int i = 0; i < entities.Count; i++)
            {
                Entity entity = entities[i];
                if (!entity.isVisible || entity.isDestroyed)
                    continue;

                if (!entity.ShouldDrawTexture())
                    continue;

                int x = entity.position.x - (this.cameraPosition.x);
                int y = entity.position.y - (this.cameraPosition.y);

                if ((x < 0 || x >= this.Width) && !entity.isAlwaysOn)
                    continue;

                if ((y < 0 || y >= this.Height) && !entity.isAlwaysOn)
                    continue;

                if (entity.drawCameraBuffer)
                {
                    //draw entity texture + colour                        
                    this.temporaryBuffer[x, y].texture = entity.GetTexture().character;
                    this.temporaryBuffer[x, y].colour = entity.GetTexture().color;
                }
                else
                    Surface.DrawText(x, y, entity.GetTexture().character.ToString(), this.GetViewRectangle(), entity.GetTexture().color, entity.buffer);
            }
        }

        public override void Destroy()
        {

            //remove from cameras list
            Camera.cameras.Remove(this);

            base.Destroy();
        }

        public void CleanBuffer()
        {

            this.temporaryBuffer = new TempBuffer[this.Width, this.Height];
        }

        public override void Draw(int posx, int posy, Camera camera)
        {
            if (posx < 0)
                posx = 0;

            if (posy < 0)
                posy = 0;

            Buffer.AddToBuffer(Buffer.Types.WORLD_BUFFER, this.temporaryBuffer, posx, posy);

            //draw entities stuff
            if (this.renderEntities != null)
                for (int i = 0; i < this.renderEntities.Count; i++)
                {
                    Entity entity = this.renderEntities[i];
                    if (entity.GetType() == typeof(Camera))
                        continue;

                    int x = entity.position.x - (this.cameraPosition.x);
                    int y = entity.position.y - (this.cameraPosition.y);

                    if ((x < 0 || x >= this.Width) && !entity.isAlwaysOn)
                    {
                        continue;
                    }

                    if ((y < 0 || y >= this.Height) && !entity.isAlwaysOn)
                    {
                        continue;
                    }

                    entity.Draw(x + posx, y + posy, this);
                    this.renderedEntities.Add(entity);
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

            return new Position(this.owner.position.x - (int)Math.Floor((decimal)(this.Width / 2) - 1),
                this.owner.position.y - (int)Math.Floor((decimal)(this.Height / 2)) - 1);
        }
    }
}
