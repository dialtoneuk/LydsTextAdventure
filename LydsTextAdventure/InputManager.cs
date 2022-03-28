using System;
using System.Collections.Generic;
using System.Text;

namespace LydsTextAdventure
{

    //one per scene?
    public class InputManager
    {

        public const int MOUSE_RANGE = 8;

        public Player Player
        {
            get;
            protected set;
        }


        public void SetPlayer(Player player)
        {

            if (this.Player != null)
                throw new ApplicationException("cannot switch player");

            this.Player = player;
        }

        public Command[] GetCheatCommands()
        {
            return new Command[]
            {
                new Command("position", () => {
                    Program.DebugLog(this.Player.position.ToString());
                }, "p"),
                new Command("no_clip", () => {
                    this.Player.isSolid = !this.Player.isSolid;
                    Program.DebugLog("Noclip toggled");
                }, "n"),
            };
        }

        public Command[] GetMovementCommands()
        {

            return new Command[]
            {
                new Command("down", () => {
                    MovementManager.MoveEntity( this.Player, new Position(this.Player.position.x, this.Player.position.y + 1));;
                }, "s"),
                new Command("up", () => {
                    MovementManager.MoveEntity( this.Player, new Position(this.Player.position.x, this.Player.position.y - 1));
                }, "w"),
                new Command("left", () => {
                    MovementManager.MoveEntity( this.Player, new Position(this.Player.position.x - 1, this.Player.position.y));
                }, "a"),
                new Command("right", () => {
                    MovementManager.MoveEntity( this.Player, new Position(this.Player.position.x + 1, this.Player.position.y));
                }, "d"),
                new Command("position", () =>
                {
                    Program.DebugLog(this.Player.position.ToString());
                }, "p")
            };
        }

        public Command[] GetInteractionCommands()
        {

            return new Command[]
            {

                new Command("click", () =>
                {

                    Program.HookManager.CallHook("Click", HookManager.Groups.Input);
                    //gui elements
                    Position pos = InputController.GetMousePosition();
                    foreach (Window window in WindowManager.GetOpenWindows())
                    {

                        foreach (GuiElement element in window.guiElements)
                            if (GuiElement.IsInsideOf(pos, element))
                            {

                                Program.HookManager.CallHook("PreClick", HookManager.Groups.Window, element);
                                element.OnClick();
                                Program.HookManager.CallHook("Click", HookManager.Groups.Window, element);
                                break;
                            }
                    }

                    //must ignore cache and get the latest alive entities
                    List<Entity> list = EntityManager.GetVisibleEntities();

                    for (int i = 0; i < list.Count; i++)
                    {
                        Entity entity = list[i];

                        if (Entity.IsMouseOver(pos, entity))
                        {

                            int distance = entity.GetDistance(SceneManager.CurrentScene.player);

                            if(distance > MOUSE_RANGE  || distance == -1)
                            {
                                Program.DebugLog("Too far away");
                                break;
                            }

                            Program.HookManager.CallHook("PreClick", HookManager.Groups.Entity, entity);
                            entity.OnClick(this.Player);
                            Program.HookManager.CallHook("Click", HookManager.Groups.Entity, entity);
                            break;
                        }
                    }
                }, "q", ConsoleKey.Q),
                new Command("build", () =>
                {
                    //gui elements
                    Position pos = InputController.GetMousePosition();
                    Camera camera = EntityManager.GetMainCamera();

                    WorldChunks world = (WorldChunks)WorldManager.CurrentWorld;

                    Tile tile = new TileWater();
                    Tile thatTile;

                    if(!world.TryGetTile(camera.cameraPosition.x + pos.x - 1, camera.cameraPosition.y + pos.y - 1, out thatTile))
                        return;
                    else
                    {
                        world.SetTile(tile, camera.cameraPosition.x + pos.x - 1, camera.cameraPosition.y + pos.y - 1);
                    }
                }, "r", ConsoleKey.R)
            };
        }
    }
}
