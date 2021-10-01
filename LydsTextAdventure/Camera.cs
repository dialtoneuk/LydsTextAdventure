using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

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
        protected Camera reference;

        public readonly Position cameraPosition;
        private char[,] temporaryBuffer;
        private char[][] buffer;
        public int width = 128;
        public int height = 32;

        protected bool drawBorder = true;
        protected bool drawTitle = true;
        protected bool mainCamera = false;

        private Camera.Perspective perspective;
        private List<Entity> renderEntities;

        public Camera(Entity entity = null, Camera.Perspective perspective = Camera.Perspective.CENTER_ON_OWNER, Position origin=null )
        {

            this.SetName("Default Camera");
            this.owner = entity;
            this.perspective = perspective;
            this.reference = this;

            if (origin != null)
                this.cameraPosition = origin;
            else
                this.cameraPosition = new Position(0, 0);

            //creates the view buffer
            this.temporaryBuffer = new char[this.width, this.height];

            //sets the name of this camera
 
            Program.DebugLog("Camera has been created", "camera");
        }

        public void SetDrawBorder(bool draw)
        {

            this.drawBorder = draw;
        }


        public void SetDrawTitle(bool draw)
        {

            this.drawTitle = draw;
        }

        public override bool IsVisible()
        {

            return true;
        }

        public void SetMainCamera(bool val)
        {

            this.mainCamera = true;
        }

        public bool IsMainCamera()
        {

            return this.mainCamera;
        }

        public override bool DrawTexture()
        {

            return false;
        }

        public void SetSize(int width, int height)
        {

            this.width = width;
            this.height = height;
        }

        public override void Update(int tick)
        {

            if (this.perspective.Equals(Camera.Perspective.CENTER_ON_OWNER) && this.owner != null)
                this.cameraPosition.SetPosition(this.CenterOnOwner());

            this.UpdateBuffer();
        }

        public virtual void Render(bool drawBorder = true)
        {

            this.Render(new char[,] { }, EntityManager.GetVisibleEntities());
        }

        public virtual void Render(List<Entity> entities)
        {

            this.Render(new char[,] { }, entities);
        }

        public virtual void Render(char[,] data, List<Entity> entities)
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

        public virtual void Render(World world, List<Entity> entities)
        {

            if(world != null )
            {

                char[,] worldData = world.Draw(this.cameraPosition.x, this.cameraPosition.y, this.width, this.height);

                for (int x = 0; x < this.width; x++)
                {

                    for (int y = 0; y < this.height; y++)
                    {

                        this.temporaryBuffer[x, y] = worldData[x, y];
                    }
                }
            }

            this.RenderEntityGroup(entities);
            this.renderEntities = entities;
        }

        private void RenderEntityGroup(List<Entity> entities)
        {

            //prepare entities for buffer
            foreach (Entity entity in entities)
            {

                if (!entity.IsVisible() || entity.IsDestroyed())
                    continue;

                if (!entity.DrawTexture())
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

        public void UpdateBuffer()
        {

            this.buffer = new char[this.height][];

            for (int y = 0; y < this.height; y++)
            {

                char[] line = new char[this.width];

                for (int x = 0; x < width; x++)
                {

                    if (this.drawBorder)
                    {
                        if (x == 0 || x == this.width - 1)
                            line[x] = '|';
                        else if (y == 1 || y == this.height - 1)
                            line[x] = '=';
                        else
                            line[x] = this.temporaryBuffer[x, y];

                        continue;
                    }

                    line[x] = this.temporaryBuffer[x, y];
                }

                this.buffer[y] = line;
            }
        }


        public bool IsDrawingTitle()
        {

            return this.drawTitle;
        }


        public bool IsDrawingBorder()
        {

            return this.drawBorder;
        }

        public override void Draw(int posx, int posy)
        {
            if (posx < 0)
                posx = 0;

            if (posy < 0)
                posy = 0;

   

            if(this.buffer != null && Program.GetTick() % 48 == 0)
            {

                int y = 0;
                foreach (char[] line in this.buffer)
                {

                    Console.SetCursorPosition(posx, posy++);

                    if (y == 0 && this.drawTitle)
                        Console.Write("[ " + this.GetName() + " ]");
                    else
                        Console.Write(line);

                    y++;
                }
            }
 
            //draw entities stuff
            if(this.renderEntities != null)
                foreach (Entity entity in this.renderEntities)
                {

                    if (!entity.IsVisible() || entity.IsDestroyed() || entity.GetType() == typeof(Camera) )
                        continue;

                    int x = entity.position.x - (this.cameraPosition.x);
                    int y = entity.position.y - (this.cameraPosition.y);

                    if (x < 0 || x >= this.width)
                    {

                        if (this.IsMainCamera() && entity.IsHiddenOutsideView() && !entity.IsOutsideView())
                            entity.SetOutsideView(true);

                        continue;
                    }

                    if (y < 0 || y >= this.height)
                    {

                        if (this.IsMainCamera() && entity.IsHiddenOutsideView() && !entity.IsOutsideView())
                            entity.SetOutsideView(true);

                        continue;
                    }

                    entity.SetCamera(ref this.reference);
                    entity.Draw(x + this.position.x, y + this.position.y);

                    if(this.IsMainCamera() && entity.IsHiddenOutsideView() && entity.IsOutsideView() )
                        entity.SetOutsideView(false);
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
