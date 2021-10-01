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
            this.world.Generate();

            this.player = new Player();

            this.camera = new Camera((Entity)this.player, Camera.Perspective.CENTER_ON_OWNER);
            this.camera.SetMainCamera(true);
            this.camera.SetSize(92, 32);
            this.camera.SetName("Main Camera");
            this.camera.position.x = 2;
            this.camera.position.y = 2;


            EntityMoving ent2 = new EntityMoving("M Frank #");
            ent2.position.x = 10;
            ent2.position.y = 10;
            ent2.SetSpeed(10);

            this.cameraSec = new Camera(ent2, Camera.Perspective.CENTER_ON_OWNER);
            this.cameraSec.SetSize(32, 32);
            this.cameraSec.SetName("Secondary Camera");
            this.cameraSec.position.x = 2;
            this.cameraSec.position.y = 35;


            EntityMoving ent3 = new EntityMoving("M Frank #2");
            ent3.position.x = 10;
            ent3.position.y = 5;
            ent3.SetSpeed(1);

            base.Before();
        }

        public override void Update()
        {

            //render world and entities using this camera
            this.camera.Render(this.world, EntityManager.GetAliveEntities());
            this.cameraSec.Render(this.world, EntityManager.GetAliveEntities());

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
