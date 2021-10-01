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
        protected Camera cameraTwo;

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
            this.camera.SetSize(92, 32);
            this.camera.position.x = 1;
            this.camera.position.y = 1;

  
            Entity ent1 = new EntityMoving("Barry");
            ent1.position.x = 10;
            ent1.position.y = 10;

            EntityMoving ent3 = new EntityMoving("Steve");
            ent3.SetSpeed(1000);
            ent3.SetMovementType(EntityMoving.MovementType.VERTICAL);
            ent3.position.x = 55;
            ent3.position.y = 55;

            this.cameraTwo = new Camera(ent3, Camera.Perspective.CENTER_ON_OWNER);
            this.cameraTwo.SetSize(32, 32);
            this.cameraTwo.position.x = 1;
            this.cameraTwo.position.y = 33;

            Entity ent2 = new EntityMoving("Jim");
            ent2.position.x = 100;
            ent2.position.y = 100;

            base.Before();
        }

        public override void Draw()
        {

            //render world and entities using this camera
            this.camera.Render(this.world, EntityManager.GetVisibleEntities());
            this.cameraTwo.Render(this.world, EntityManager.GetVisibleEntities());
        }


        public override void Start()
        {

            Program.GetInputController().SetTextInput(false);
            Program.GetInputController().SetAwaitingInput(true);

            base.Start();
        }
    }
}
