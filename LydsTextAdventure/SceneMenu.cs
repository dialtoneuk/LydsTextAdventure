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

                    Console.Clear();
                    SceneManager.EndScene();
                    SceneManager.StartScene("sceneGame");
                }, "g")
            };
        }

        public override void Before()
        {

            this.camera = new Camera();
            this.camera.position.x = 1;
            this.camera.position.y = 1;

            EntityMovingText text = new EntityMovingText("MenuText");
            text.position.SetPosition(this.camera.GetViewCenter());
            text.position.x = text.GetDistance();

            text.SetText("Lyds Text Adventure");

            base.Before();
        }


        public override void Draw()
        {

            this.camera.Render(this.titleScreen.Generate(Console.WindowWidth, Console.WindowHeight), EntityManager.GetVisibleEntities(), false);
        }


        public override void Start()
        {

            Program.GetInputController().SetTextInput(true);
            Program.GetInputController().SetAwaitingInput(true);

            base.Start();
        }
    }
}
