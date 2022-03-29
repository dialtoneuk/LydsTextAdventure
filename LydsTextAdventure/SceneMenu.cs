using System.Collections.Generic;

namespace LydsTextAdventure
{
    class SceneMenu : Scene
    {

        protected Camera camera;
        protected InputManager inputManager = new InputManager();
        protected World world;

        public SceneMenu(string name, List<Command> commands = null) : base(name, commands) { }

        protected override List<Command> LoadCommands()
        {
            List<Command> commands = new List<Command>();
            commands.AddRange(inputManager.GetInteractionCommands());
            return commands;
        }


        public override void Before()
        {

            this.player = new Player();
            this.player.isVisible = false;
            this.inputManager.SetPlayer(this.player);

            this.camera = new Camera();
            this.camera.SetSize(Buffer.WindowWidth, Buffer.WindowHeight);
            this.camera.SetMainCamera(true);

            this.camera.position.x = 0;
            this.camera.position.y = 0;

            _ = new WindowMenu();

            base.Before();
        }

        public override void Update()
        {

            this.camera.UpdateBuffer();
            base.Update(); //must call base
        }


        public override void Start()
        {

            InputController.IsAwaitingInput = true;
            InputController.IsTextInput = false;

            base.Start();
        }
    }
}
