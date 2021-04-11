using System;
using System.Collections.Generic;
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
        protected Position cameraPosition;
        private Camera.Perspective perspective;
        public bool isActive = true;
       
        public Camera(ref Player player, Camera.Perspective perspective = Camera.Perspective.BIRDEYE_PLAYER, Position origin=null )
        {

            this.owner = player;
            this.perspective = perspective;

            if (origin != null)
                this.cameraPosition = origin;
            else
                this.cameraPosition = new Position(0, 0);

            Program.DebugLog("camera created", "camera");
        }
        
        public virtual void Update()
        {

            if (this.perspective.Equals(Camera.Perspective.BIRDEYE_PLAYER))
                this.cameraPosition = this.CenterOnPlayer();
        }

        public void Render(World world, List<Entity> entities, int width=64, int height=24)
        {


            char[][] array = new char[width * height][];

            if (this.isActive == false)
                return;

            int realx = 0;
            int realy = 0;
            for(int x = 0; x < height; x++ )
            {

                char[] line = new char[width];

                for (int y = 0; y < width; y++)
                {

                    //render world
                    Position position = new Position(this.cameraPosition.x + x, this.cameraPosition.y + y);
                    if (world.HasChunkAtPosition(position, out Chunk chunk))
                        line[y] = chunk.GetTile(realx, realy).texture.character;
                   
                    if (realy != 0 && realy % (world.chunkSize - 1) == 0)
                        realy = 0;
                    else
                        realy++;
                }

                array[x] = line;

                if (realx != 0 && realx % (world.chunkSize - 1) == 0)
                    realx = 0;
                else
                    realx++;
            }

            //render character array here
            foreach (char[] line in array)
            {

                if (line == null || line.Length == 0)
                    continue;

                Console.Write(line);
                Console.Write(Environment.NewLine);
            }
        }

        public void UpdatePerspective(Camera.Perspective perspective)
        {

            
            this.perspective = perspective;
            Program.DebugLog("perspective changed to " + perspective.ToString(), "camera");
        }

        private Position CenterOnPlayer()
        {

            return new Position(this.owner.positon.x + ((this.owner.positon.x + 1) / 2),
                this.owner.positon.y + (this.owner.positon.y + 1) / 2);
        }
    }
}
