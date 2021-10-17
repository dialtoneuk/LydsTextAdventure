using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace LydsTextAdventure
{

    class Program
    {

        public enum State: int
        {
            RUNNING,
            LOADING,
            SHUTDOWN
        }

        private static readonly CommandManager CommandManager = new CommandManager();
        public static ConsoleLogger DebugLogger;

        private static string[] debugLog;
        private static int stackPosition = 0;


        public static Command LastCommand { get; private set; }

        private static int tick = 0;
        //program state
        private static State state = State.LOADING;
       
        static void Main(string[] args)
        {

            Console.SetBufferSize(152, 72);
            Console.SetWindowSize(152, 72);
            Console.ResetColor();
            Console.Title = "Lyds Text Adventure";
            Console.CursorVisible = false;

            ConsoleManager.DisableQuickEdit();
            ConsoleLogger.CreateExitEvent();

            //Create the buffer/viewable draw space
            Buffer.Create(150, 70);

            //adds the remote logger
#if DEBUG
            Program.DebugLogger = new ConsoleLogger();
            Program.DebugLog("connected to console log successfully");
            Program.RegisterDebugCommands();
            debugLog = new string[1024];
#endif
            //register our scenes
            Scenes.RegisterScenes();
            //start test scene
            SceneManager.StartScene("sceneMenu");

            state = State.RUNNING;

            //game loop
            while (!state.Equals(State.SHUTDOWN))
            {

                Buffer.Clear();

                if(InputController.isAwaitingInput && !InputController.isRunning)
                {

                    Task.Factory.StartNew((Action)(() =>
                    {
                        InputController.KeyboardInput input = InputController.GetKeyboardInput();
                        Command command = Program.CommandManager.GetCommand(input.text);
                        if (command == null)
                            Program.DebugLog("invalid command:" + input.text);
                        else
                        {
                            command.Execute();
                            Program.LastCommand = command;
                        }
                    }));
                }

                //update then draw scene
                if (SceneManager.IsSceneActive())
                    SceneManager.UpdateScene();

                //takes all of our draw data and adds it to the buffer
                Buffer.PrepareBuffer();

                //Draws the buffer
                Buffer.DrawBuffer();

                //tick up
                Program.tick++;

                //reset
                if (Program.tick > 8192)
                {

                    System.Console.Clear();
                    Program.tick = 0;
                }
            }

            //shutdown stuff here
        }

        public static void SetState(State state)
        {

            Program.state = state;
        }

        public static void RegisterDebugCommands()
        {

            Program.CommandManager.Register(new List<Command>()
            {
                new Command("clean_screen", () => {
                        System.Console.Clear();
                }, "z"),
                new Command("delete_entities", () => {
                    List<Entity> result = EntityManager.GetEntitiesByType(typeof(EntityMoving));

                    foreach(Entity ent in result)
                    {
                        ent.RemoveEntity();
                    }
                }, "m"),
                new Command("delete_current_world", () => {
                    WorldManager.DeleteWorld(WorldManager.CurrentWorld.id);
                }, "b"),
            });
        }

        public static void DebugLog(string msg, string op="general" )
        {

            if (msg.Length.Equals(0))
                return;

            if (Program.DebugLogger is null)
                return;

            string str = string.Concat("[", op + ":" + Program.tick + ":" + DateTime.Now.TimeOfDay + "] ", msg);

            if (Program.debugLog == null || stackPosition + 1 >= Program.debugLog.Length)
            {
                Program.stackPosition = 0;
                Program.debugLog = new string[1024];
            }
             
            Program.debugLog[Program.stackPosition++] = str;
            Program.DebugLogger.WriteLine(str);
        }

        public static string[] GetDebugLog()
        {

#if DEBUG
            string[] debugLog = (string[])Program.debugLog.Clone();
            Array.Reverse(debugLog);
            return (debugLog);
#else
            return new string[1];
#endif
        }

        public static int GetTick()
        {

            return Program.tick;
        }

        public static CommandManager GetCommandController()
        {

            return Program.CommandManager;
        }
    }
}
