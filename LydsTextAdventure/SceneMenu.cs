using System;
using System.Collections.Generic;
using System.Text;

namespace LydsTextAdventure
{
    class SceneMenu : Scene
    {

        protected Camera camera;
        protected readonly TitleScreen titleScreen = new TitleScreen();

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

            this.camera = new Camera();
            this.camera.SetDrawBorder(false);
            this.camera.SetDrawTitle(false);

            this.camera.position.x = 1;
            this.camera.position.y = 1;

            //generate title screen background
            this.titleScreen.Generate(this.camera.width, this.camera.height);

            EntityMovingText text = new EntityMovingText();
            text.position.SetPosition(this.camera.GetViewCenter());
            text.position.x = text.GetDistance();

            text.SetText("Lyds Text Adventure");

            base.Before();
        }

        public override void Update()
        {

            if(Program.GetTick() % 100 == 0 )
                this.titleScreen.Generate(this.camera.width, this.camera.height);

            //render world and entities using this camera
            this.camera.Render(this.titleScreen.GetBuffer(), EntityManager.GetVisibleEntities());

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
