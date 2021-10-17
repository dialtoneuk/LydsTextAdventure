using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LydsTextAdventure
{

    class Program
    {

        public enum State : int
        {
            RUNNING,
            LOADING,
            SHUTDOWN
        }

        public static DebugLogger DebugLogger;

        private static string[] debugLog;
        private static int StackPosition = 0;


        public static Command LastCommand
        {
            get; private set;
        }

        private static int Tick = 0;
        //program state
        private static State _state = State.LOADING;

        static void Main(string[] args)
        {

            Console.SetBufferSize(152, 72);
            Console.SetWindowSize(152, 72);
            Console.ResetColor();
            Console.Title = "Lyds Text Adventure";
            Console.CursorVisible = false;

            CommandManager.AddDefaultCommands();
            ConsoleManager.DisableQuickEdit();
            DebugLogger.CreateExitEvent();

            //Create the buffer/viewable draw space
            Buffer.Create(150, 70);

            //adds the remote logger
#if DEBUG
            Program.DebugLogger = new DebugLogger();
            Program.DebugLog("connected to console log successfully");
            Program.RegisterDebugCommands();
            Program.debugLog = new string[1024];
#endif
            //register our scenes
            Scenes.RegisterScenes();
            //start test scene
            SceneManager.StartScene("sceneMenu");

            _state = State.RUNNING;

            //game loop
            while (!_state.Equals(State.SHUTDOWN))
            {

                Buffer.Clear();

                if (InputController.IsAwaitingInput && !InputController.IsRunning)
                {

                    Task.Factory.StartNew(() =>
                    {
                        InputController.KeyboardInput input = InputController.GetKeyboardInput();
                        Command command = CommandManager.GetCommand(input.text);
                        if (command == null)
                            Program.DebugLog("invalid command:" + input.text);
                        else
                        {
                            command.Execute();
                            Program.LastCommand = command;
                        }
                    });
                }

                //update then draw scene
                if (SceneManager.IsSceneActive())
                    SceneManager.UpdateScene();

                //takes all of our draw data and adds it to the buffer
                Buffer.PrepareBuffer();

                //Draws the buffer
                Buffer.DrawBuffer();


                //reset
                if (Program.Tick >= 8192)
                {

                    System.Console.Clear();
                    Program.DebugLog("screen cleaned");
                    Program.Tick = 0;
                }
                else
                    Program.Tick++;
            }

#if DEBUG
            Program.DebugLogger.WriteShutdown();
#endif
        }

        public static void SetState(State state)
        {

            Program._state = state;
        }

        public static void RegisterDebugCommands()
        {

            CommandManager.Register(new List<Command>()
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

        public static void DebugLog(string msg, string op = "general")
        {

            if (msg.Length.Equals(0))
                return;

            if (Program.DebugLogger is null)
                return;

            string str = string.Concat("[", op + ":" + Program.Tick + ":" + DateTime.Now.TimeOfDay + "] ", msg);

            if (Program.debugLog == null || StackPosition + 1 >= Program.debugLog.Length)
            {
                Program.StackPosition = 0;
                Program.debugLog = new string[1024];
            }

            Program.debugLog[Program.StackPosition++] = str;
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

            return Program.Tick;
        }
    }
}
