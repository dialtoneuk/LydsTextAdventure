using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LydsTextAdventure
{
    class SceneGame : Scene
    {

        protected WorldChunks world;
        protected Player player;
        protected Camera camera;

        public SceneGame(string name, List<Command> commands = null) : base(name, commands)
        {
        }

        protected override List<Command> LoadCommands()
        {

            //scene commands
            return new List<Command>(){
                new Command("down", () => {
                    MovementManager.MoveEntity( this.player, new Position(this.player.position.x, this.player.position.y + 1));
                }, "s"),
                new Command("up", () => {
                    MovementManager.MoveEntity( this.player, new Position(this.player.position.x, this.player.position.y - 1));
                }, "w"),
                new Command("left", () => {
                    MovementManager.MoveEntity( this.player, new Position(this.player.position.x - 1, this.player.position.y));
                }, "a"),
                new Command("right", () => {
                    MovementManager.MoveEntity( this.player, new Position(this.player.position.x + 1, this.player.position.y));
                }, "d"),
                new Command("position", () =>
                {
                    Program.DebugLog(this.player.position.ToString());
                }, "p"),
                new Command("test", () =>
                {
                    Program.DebugLog( EntityManager.GetMainCamera().GetMousePosition().ToString() );
                }, "g"),
                new Command("click", () =>
                {

                    //gui elements
                    Position pos = InputController.GetMousePosition();
                    foreach(Window window in WindowManager.GetOpenWindows())
                    {

                        foreach(GuiElement element in window.guiElements)
                            if(GuiElement.IsInsideOf(pos, element))
                            {
                                element.OnClick();
                                break;
                            }
                    }

                    //entitys
                    foreach(Entity entity in EntityManager.GetVisibleEntities())
                    {

                            if(Entity.IsMouseOver(pos, entity))
                            {
                                entity.OnClick(this.player);
                                break;
                            }
                    }
                }, "q", ConsoleKey.Q)
            };
        }

        public override void Before()
        {

            this.world = new WorldChunks(8, 8);
            this.world.GenerateWorld();

            this.player = new Player();

            for (int i = 0; i < 10; i++)
            {

                EntityMoving m = new EntityMoving();
                m.position.y = 10 * i;
                m.position.x = 10 + i;
                m.SetSpeed(i);
            }

            this.camera = new Camera(this.player, Camera.Perspective.CENTER_ON_OWNER);
            this.camera.SetMainCamera(true);
            this.camera.SetSize(79, 41);
            this.camera.SetName("Main Camera");
            this.camera.position.x = 0;
            this.camera.position.y = 0;

            WindowPlayerStatistics stats = new WindowPlayerStatistics();
            stats.SetPlayer(this.player);
            stats.SetPosition(80, 0);

            WindowInventory inventory = new WindowInventory();
            inventory.SetPlayer(this.player);
            inventory.SetPosition(80, 9);

            base.Before();
        }

        public override void Update()
        {

            //create any new chunks around the player they haven't seen yet
            this.world.CreateChunksAroundPlayer(this.player, 8);

            //render world and entities using this camera
            this.camera.UpdateBuffer();

            //then do the base updates
            base.Update();
        }


        public override void Start()
        {

            InputController.IsTextInput = false;
            InputController.IsAwaitingInput = true;

            //set the players spawn position
            this.player.position.SetPosition(this.world.GetInitialSpawnPoint());

            //update spawn chunks
            this.world.UpdateChunks();

            base.Start();
        }
    }
}
