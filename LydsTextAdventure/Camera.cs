using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace LydsTextAdventure
{
    class Camera : Entity
    {

        public enum Perspective : int
        {
            DEFAULT,
            CENTER_ON_OWNER
        }

        protected Entity owner;

        public readonly Position cameraPosition;
        public char[,] buffer;

        private int width = 128;
        private int height = 32;
        private Camera.Perspective perspective;

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
            this.buffer = new char[this.width, this.height];

            //sets the name of this camera
 
            Program.DebugLog("Camera has been created", "camera");
        }

        public override bool IsVisible()
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
        }

        public virtual void Render(World world, List<Entity> entities, bool drawBorder = true)
        {

            char[,] worldData = world.Draw(this.cameraPosition.x, this.cameraPosition.y, this.width, this.height);

            for(int x = 0; x < this.width; x++)
            {

                for (int y = 0; y < this.height; y++)
                {

                    this.buffer[x, y] = worldData[x, y];
                }
            }

            //prepare entities for buffer
            foreach(Entity entity in entities)
            {
                
                int x = entity.position.x - (this.cameraPosition.x);
                int y = entity.position.y - (this.cameraPosition.y);

                if (x < 0 || x >= width)
                    continue;

                if (y < 0 || y >= height)
                    continue;

                //draw entity texture
                this.buffer[x,y] = entity.GetTexture().character;
            }

            this.DrawBuffer(drawBorder);

            //draw entities stuff
            foreach(Entity entity in entities)
            {


                int x = entity.position.x - (this.cameraPosition.x);
                int y = entity.position.y - (this.cameraPosition.y);

                if (x < 0 || x >= width)
                    continue;

                if (y < 0 || y >= height)
                    continue;

                entity.Draw(x + this.position.x, y + this.position.y - 1);
            }

            this.CleanBuffer();
        }

        public void CleanBuffer()
        {

            this.buffer = new char[this.width, this.height];
        }

        public void DrawBuffer(bool drawBorder = true, bool drawName = true)
        {

            int posx = position.x;
            int posy = position.y;

            if (posx < 0)
                posx = 0;

            if (posy < 0)
                posy = 0;

            Console.SetCursorPosition(posx, posy);

            for (int y = 0; y < this.height; y++ )
            {

                char[] line = new char[this.width];

                for(int x = 0; x < width; x++ )
                {


                    if (drawBorder)
                    {
                        if (x == 0 || x == this.width - 1)
                            line[x] = '|';
                        else if (y == 1 || y == this.height - 1)
                            line[x] = '-';
                        else
                            line[x] = this.buffer[x, y];

                        continue;
                    }
                    line[x] = this.buffer[x, y];
                }

                Console.WriteLine(line);

                if(posy == position.y)
                {

                    Console.SetCursorPosition(posx + 4, posy);
                    Console.Write("[ " + this.GetName() + " ]");
                }

                Console.SetCursorPosition(posx, posy + y);
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
