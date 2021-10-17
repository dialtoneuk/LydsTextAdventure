using System.Collections.Generic;

namespace LydsTextAdventure
{
    class SceneMenu : Scene
    {

        protected Camera camera;
        protected WorldTitleScreen world;

        public SceneMenu(string name, List<Command> commands = null) : base(name, commands)
        {
        }

        protected override List<Command> LoadCommands()
        {

            return new List<Command>(){
                new Command("click", () =>
                {

                    Position pos = InputController.GetMousePosition();
                    foreach(Window window in WindowManager.GetOpenWindows())
                    {

                        foreach(GuiElement element in window.guiElements)
                            if(GuiElement.IsInsideOf(pos, element))
                                element.OnClick();
                    }

                    foreach(Entity entity in EntityManager.GetVisibleEntities())
                    {

                            if(Entity.IsMouseOver(pos, entity))
                                entity.OnClick();
                    }
                }, "q")
            };
        }


        public override void Before()
        {

            this.world = new WorldTitleScreen();
            this.world.SetSize(Buffer.WindowWidth, Buffer.WindowHeight);
            this.world.GenerateWorld();

            this.camera = new Camera();
            this.camera.SetSize(Buffer.WindowWidth, Buffer.WindowHeight);
            this.camera.SetMainCamera(true);

            this.camera.position.x = 0;
            this.camera.position.y = 0;

            WindowMenu menu = new WindowMenu();

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
