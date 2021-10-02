using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LydsTextAdventure
{
    class SceneGame : Scene
    {

        protected World world;
        protected Player player;
        protected Camera camera;
        protected Camera cameraSec;
        protected Camera cameraThr;
        protected Camera cameraFour;
        protected Camera cameraFive;
        protected Camera cameraSix;

        public SceneGame(string name, List<Command> commands = null) : base(name, commands)
        { }

        protected override List<Command> LoadCommands()
        {

            //scene commands
            return new List<Command>()
            {
          
            };
        }

        public override void Before()
        {

            this.world = new World();
            this.world.GenerateWorld();

            this.player = new Player();

            this.camera = new Camera((Entity)this.player, Camera.Perspective.CENTER_ON_OWNER);
            this.camera.SetMainCamera(true);
            this.camera.SetSize(64, 32);
            this.camera.SetName("Main Camera");
            this.camera.position.x = 0;
            this.camera.position.y = 0;

            this.cameraSix = new Camera((Entity)this.player, Camera.Perspective.CENTER_ON_OWNER);
            this.cameraSix.SetMainCamera(true);
            this.cameraSix.SetSize(32, 32);
            this.cameraSix.SetName("Main Camera");
            this.cameraSix.position.x = 65;
            this.cameraSix.position.y = 0;


            EntityMoving ent2 = new EntityMoving("Slow Entity");
            ent2.position.x = 10;
            ent2.position.y = 80;
            ent2.SetSpeed(10);

            EntityMoving ent3 = new EntityMoving("Fast Entity");
            ent3.position.x = 20;
            ent3.position.y = 60;
            ent3.SetMovementType(EntityMoving.MovementType.VERTICAL);
            ent3.SetSpeed(20);

            this.cameraSec = new Camera(ent2, Camera.Perspective.CENTER_ON_OWNER);
            this.cameraSec.SetSize(32, 32);
            this.cameraSec.SetName("Secondary Camera");
            this.cameraSec.position.x = 0;
            this.cameraSec.position.y = 33;

            this.cameraThr = new Camera(ent2, Camera.Perspective.CENTER_ON_OWNER);
            this.cameraThr.SetSize(32, 32);
            this.cameraThr.SetName("Secondary Camera");
            this.cameraThr.position.x = 33;
            this.cameraThr.position.y = 33;

            this.cameraFour = new Camera(ent2, Camera.Perspective.CENTER_ON_OWNER);
            this.cameraFour.SetSize(32, 32);
            this.cameraFour.SetName("Secondary Camera");
            this.cameraFour.position.x = 66;
            this.cameraFour.position.y = 33;

            this.cameraFive = new Camera(ent2, Camera.Perspective.CENTER_ON_OWNER);
            this.cameraFive.SetSize(32, 32);
            this.cameraFive.SetName("Secondary Camera");
            this.cameraFive.position.x = 99;
            this.cameraFive.position.y = 33;

            for (int i = 0; i < 200; i++)
            {

                EntityMoving e = new EntityMoving("#" + i);
                e.position.x = 10;
                e.position.y = 5 * i;
                e.SetSpeed(5);
            }
           

            base.Before();
        }

        public override void Update()
        {

            //render world and entities using this camera
            this.camera.UpdateBuffer();
            this.cameraSec.UpdateBuffer();
            this.cameraThr.UpdateBuffer();
            this.cameraFour.UpdateBuffer();
            this.cameraFive.UpdateBuffer();
            this.cameraSix.UpdateBuffer();

            base.Update();
        }


        public override void Start()
        {

            Program.GetInputController().SetTextInput(false);
            Program.GetInputController().SetAwaitingInput(true);

            base.Start();
        }
    }
}
