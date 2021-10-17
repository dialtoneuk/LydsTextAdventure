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

            this.camera.position.x = 0;
            this.camera.position.y = 0;

            EntityMovingText text = new EntityMovingText();
            text.position.SetPosition(this.camera.GetViewCenter());
            text.SetDistance(this.camera.width / 2);
            text.position.x = this.camera.width / 4;

            text.SetText("Lyds Text Adventure");

            EntityMovingText text2 = new EntityMovingText();
            text2.position.SetPosition(this.camera.GetViewCenter());
            text2.SetDistance(this.camera.width / 2);
            text2.SetSpeed(16);
            text2.position.x = this.camera.width / 4;
            text2.position.y += 1;

            text2.SetText("2.0");

            EntityMovingText text3 = new EntityMovingText();
            text3.position.SetPosition(this.camera.GetViewCenter());
            text3.SetDistance(this.camera.width / 2);
            text3.SetSpeed(8);
            text3.position.x = this.camera.width / 4;
            text3.position.y += 2;

            text3.SetText("Written by Llydia Cross");

            base.Before();
        }

        public override void Update()
        {

            this.camera.UpdateBuffer();
            base.Update(); //must call base
        }


        public override void Start()
        {

            InputController.isAwaitingInput = true;
            InputController.isTextInput = true;

            base.Start();
        }
    }
}
