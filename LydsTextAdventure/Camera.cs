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

        public void Render(World world, List<Entity> entities, int width=128, int height=42)
        {

            if (this.isActive == false)
                return;

            int realx = 0;
            int realy = 0;
            for(int x = 0; x < height; x++ )
            {

                for (int y = 0; y < width; y++)
                {

                    //render world
                    Chunk chunk;
                    Position position = new Position(this.cameraPosition.x + x, this.cameraPosition.y + y);
                    if (world.ChunkExists(position, out chunk))
                    {

                        Tile tile = chunk.GetTile(realx, realy);
                        Console.Write(tile.texture.character);
                    }

                    if (realy != 0 && realy % (world.chunkSize - 1) == 0)
                        realy = 0;
                    else
                        realy++;
                }

                Console.WriteLine();

                if (realx != 0 && realx % (world.chunkSize - 1) == 0)
                    realx = 0;
                else
                    realx++;
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
