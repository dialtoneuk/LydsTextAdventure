using System.Collections.Generic;

namespace LydsTextAdventure
{
    class SceneMenu : Scene
    {

        protected Camera camera;
        protected WorldTitleScreen world;

        public SceneMenu(string name, List<Command> commands = null) : base(name, commands)
        { }

        protected override List<Command> LoadCommands()
        {

            return new List<Command>(){
                new Command("game", () =>
                {
                    SceneManager.EndScene();
                    SceneManager.StartScene("sceneGame");
                }, "g")
            };
        }


        public override void Before()
        {

            this.world = new WorldTitleScreen();
            this.world.SetSize(Buffer.WindowWidth * 2, Buffer.WindowHeight * 2);
            this.world.GenerateWorld();

            this.camera = new Camera();
            this.camera.SetSize(Buffer.WindowWidth, Buffer.WindowHeight);
            this.camera.SetMainCamera(true);
            this.camera.SetDrawBorder(false);
            this.camera.SetDrawTitle(false);

            this.camera.position.x = 0;
            this.camera.position.y = 0;

            EntityMovingText text = new EntityMovingText();
            text.position.SetPosition(this.camera.GetViewCenter());
            text.position.x = text.GetDistance();

            text.SetText("Lyds Text Adventure");

            base.Before();
        }

        public override void Update()
        {

            this.camera.UpdateBuffer();
            base.Update(); //must call base
        }


        public override void Start()
        {

            Program.GetInputController().SetTextInput(true);
            Program.GetInputController().SetAwaitingInput(true);

            base.Start();
        }
    }
}
