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
                }, "g")
            };
        }

        public override void Before()
        {

            this.world = new WorldChunks(4, 4);
            this.world.GenerateWorld();

            this.player = new Player();
            this.player.SetSolid(false); //player can walk through world

            for (int i = 0; i < 10; i++)
            {

                EntityMoving m = new EntityMoving();
                m.position.y = 10 * i;
                m.position.x = 10 + i;
                m.SetSpeed(i);
            }

            this.camera = new Camera(this.player, Camera.Perspective.CENTER_ON_OWNER);
            this.camera.SetMainCamera(true);
            this.camera.SetSize(79, 32);
            this.camera.SetName("Main Camera");
            this.camera.position.x = 0;
            this.camera.position.y = 0;

            WindowPlayerStatistics stats = new WindowPlayerStatistics();
            WindowConsole test = new WindowConsole();

            stats.SetPlayer(this.player);
            stats.SetPosition(80, 0);
            test.SetPosition(0, 33);

            base.Before();
        }

        public override void Update()
        {

            //create any new chunks around the player they haven't seen yet
            this.world.CreateChunksAroundPlayer(this.player, 4);

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
