using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LydsTextAdventure
{
    class SceneMenu : Scene
    {

        protected World world;
        protected Player player;
        protected Camera camera;

        public SceneMenu(string name, List<Command> commands = null ) : base( name, commands )
        {}

        protected override List<Command> LoadCommands()
        {

            //scene commands
            return new List<Command>()
            {
                new Command("camera_debug", () => {
                    Program.DebugLog( this.camera.cameraPosition.ToString() );
                }, "u"),
            };
        }

        public override void Before()
        {

            this.world = new World();
            this.world.Generate();

            this.player = new Player();
            this.camera = new Camera(ref this.player);


            Entity ent = new Entity();
            ent.position.x = 10;
            ent.position.y = 10;

            Entity ent2 = new Entity();
            ent2.position.x = 100;
            ent2.position.y = 100;

            //call base
            base.Before();
        }

        public override void Update()
        {

            this.camera.Update();
            base.Update();
        }

        public override void Draw()
        {

            this.camera.Render(this.world, EntityManager.GetVisibleEntities());
        }


        public override void Start()
        {

            Program.GetInputController().ToggleAwaitingInput();
            base.Start();
        }
    }
}
