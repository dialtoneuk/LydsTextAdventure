using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LydsTextAdventure
{
    class SceneGame : Scene
    {

        protected World world;
        protected World worldSecondary;
        protected Player player;
        protected Camera camera;
        protected Camera cameraSecondary;

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


            this.worldSecondary = new World();
            this.worldSecondary.Generate();

            this.player = new Player();

            this.camera = new Camera((Entity)this.player, Camera.Perspective.CENTER_ON_OWNER);
            this.camera.SetMainCamera(true);
            this.camera.SetSize(32, 32);
            this.camera.SetName("Main Camera");
            this.camera.position.x = 2;
            this.camera.position.y = 2;

            this.cameraSecondary = new Camera((Entity)this.player, Camera.Perspective.CENTER_ON_OWNER);
            this.cameraSecondary.SetSize(32, 32);
            this.cameraSecondary.SetName("Secondary Camera");
            this.cameraSecondary.position.x = 2;
            this.cameraSecondary.position.y = 34;

            for (int i = 0; i < 5000; i++)
            {

                EntityMoving ent = new EntityMoving("Frank #" + i.ToString());
                ent.SetDistance(5);
                ent.position.x = 10;
                ent.position.y = 10 + ( (i + 1) * 2 );

                ent.SetSpeed(i);
            }


            base.Before();
        }

        public override void Update()
        {

            //render world and entities using this camera
            this.camera.Render(this.world, EntityManager.GetAliveEntities());
            this.cameraSecondary.Render(this.worldSecondary, EntityManager.GetEntitiesByType(typeof(Player)));

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
