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

            return new List<Command>(){
                new Command("down", () => {
                    this.player.positon.x++;
                }, "s"),
                new Command("up", () => {
                    this.player.positon.x--;
                }, "w"),
                new Command("left", () => {
                    this.player.positon.y--;
                }, "a"),
                new Command("right", () => {
                    this.player.positon.y++;
                }, "d")
            };
        }

        public override void Before()
        {

            this.world = new World();

            //do this as a task
            World.GenerateSpawn(ref this.world);

            this.player = new Player();
            this.camera = new Camera(ref this.player);

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

            this.camera.Render(this.world, null, 92, 30);
        }


        public override void Start()
        {

            Program.GetInput().ToggleAwaitingInput();
            base.Start();
        }
    }
}
