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
        protected Camera cameraTest;

        public SceneMenu(string name, List<Command> commands = null ) : base( name, commands )
        {}

        protected override List<Command> LoadCommands()
        {

            //scene commands
            return new List<Command>()
            {
                new Command("camera_right", () => {
                    this.camera.position.x++;
                    Console.Clear();
                }, "k"),
                new Command("camera_down", () => {
                    this.camera.position.y++;
                    Console.Clear();
                }, "u"),
                new Command("camera_left", () => {
                    this.camera.position.x--;
                    Console.Clear();
                }, "l"),
                new Command("camera_up", () => {
                    this.camera.position.y--;
                    Console.Clear();
                }, "i")
            };
        }

        public override void Before()
        {

            this.world = new World();
            this.world.Generate();

            this.player = new Player();

            this.camera = new Camera((Entity)this.player, Camera.Perspective.CENTER_ON_OWNER);
            this.camera.position.x = 1;
            this.camera.position.y = 1;

            Entity ent1 = new EntityMoving();
            ent1.SetName("Fucking Twat");
            ent1.position.x = 10;
            ent1.position.y = 10;

            EntityMoving ent3 = new EntityMoving();
            ent3.SetSpeed(1000);
            ent3.SetName("Another Fucking Twat");
            ent3.SetMovementType(EntityMoving.MovementType.VERTICAL);
            ent3.position.x = 55;
            ent3.position.y = 55;

            Entity ent2 = new EntityMoving();
            ent2.SetName("Special Ent");
            ent2.position.x = 100;
            ent2.position.y = 100;

            this.cameraTest = new Camera(ent3, Camera.Perspective.CENTER_ON_OWNER);
            this.cameraTest.SetName("Test Camera 2");
            this.cameraTest.position.x = 1;
            this.cameraTest.position.y = 33;
            this.cameraTest.SetSize(32, 32);

            base.Before();
        }

        public override void Draw()
        {

            //render world and entities using this camera
            this.camera.Render(this.world, EntityManager.GetVisibleEntities());
            this.cameraTest.Render(this.world, EntityManager.GetVisibleEntities());
        }


        public override void Start()
        {

            Program.GetInputController().ToggleAwaitingInput();
            base.Start();
        }
    }
}
