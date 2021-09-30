using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace LydsTextAdventure
{
    class Camera
    {

        public enum Perspective : int
        {
            DEFAULT,
            BIRDEYE_PLAYER
        }

        protected Player owner;
        public Position cameraPosition;
        private Camera.Perspective perspective;

        public bool isActive = true;

        public char[,] buffer;

        public readonly int width = 128;
        public readonly int height = 32;
       
        public Camera(ref Player player, Camera.Perspective perspective = Camera.Perspective.BIRDEYE_PLAYER, Position origin=null )
        {

            this.owner = player;
            this.perspective = perspective;

            if (origin != null)
                this.cameraPosition = origin;
            else
                this.cameraPosition = new Position(0, 0);


            this.buffer = new char[this.width, this.height];

            Program.DebugLog("camera created", "camera");
        }
        
        public virtual void Update()
        {

            if (this.perspective.Equals(Camera.Perspective.BIRDEYE_PLAYER))
                this.cameraPosition = this.CenterOnPlayer();
        }

        public void Render(World world, List<Entity> entities)
        {

            char[,] worldData = world.Draw( this.cameraPosition.x, this.cameraPosition.y, this.width, this.height);

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
                
                int x = entity.position.x - this.cameraPosition.x;
                int y = entity.position.y - this.cameraPosition.y;

                if (x < 0 || x >= width)
                    continue;

                if (y < 0 || y >= height)
                    continue;

                //draw entity texture
                this.buffer[x,y] = entity.GetTexture().character;
            }

            this.DrawBuffer();

            //draw entities stuff
            foreach(Entity entity in entities)
            {


                int x = entity.position.x - this.cameraPosition.x;
                int y = entity.position.y - this.cameraPosition.y;

                if (x < 0 || x >= width)
                    continue;

                if (y < 0 || y >= height)
                    continue;

                entity.Draw(x, y);

            }

            System.Threading.Thread.Sleep(10);
            this.CleanBuffer();
        }

        public void CleanBuffer()
        {

            this.buffer = new char[this.width, this.height];
        }

        public void DrawBuffer()
        {

            Console.SetCursorPosition(0, 0);

            for(int y = 0; y < this.height; y++ )
            {

                char[] line = new char[this.width];

                for(int x = 0; x < width; x++ )
                {

                    line[x] = this.buffer[x, y];
                }

                Console.WriteLine(line);
            }
        }

        public void UpdatePerspective(Camera.Perspective perspective)
        {

            this.perspective = perspective;
            Program.DebugLog("perspective changed to " + perspective.ToString(), "camera");
        }

        private Position CenterOnPlayer()
        {

            return new Position(this.owner.position.x - (int)Math.Floor((decimal)(this.width / 2) - 1),
                this.owner.position.y - (int)Math.Floor((decimal)(this.height / 2)) - 1);
        }
    }
}
