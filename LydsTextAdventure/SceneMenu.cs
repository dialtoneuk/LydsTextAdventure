using System.Collections.Generic;

namespace LydsTextAdventure
{
    class SceneMenu : Scene
    {

        protected Camera camera;
        protected Player player;
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

                        for (int i = 0; i < window.guiElements.Count; i++) { GuiElement element = window.guiElements[i]; if(GuiElement.IsInsideOf(pos, element))
                                element.OnClick(); } }

                    List<Entity> list = EntityManager.GetVisibleEntities(true);

                    for (int i = 0; i < list.Count; i++)
                    {

                        Entity entity = list[i];

                        if(Entity.IsMouseOver(pos, entity))
                                entity.OnClick(this.player);
                    }
                }, "q")
            };
        }


        public override void Before()
        {

            this.player = new Player();
            this.player.SetVisible(false);

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
