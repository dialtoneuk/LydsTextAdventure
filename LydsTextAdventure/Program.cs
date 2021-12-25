using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LydsTextAdventure
{

    class Program
    {


        public const int MAX_TICK = 8912;
        public const int DEBUGGER_LOG_SIZE = 2048;

#if DEBUG
        public const bool DEBUGGER_CONNECT_LOGGER = true;
#else
        public const bool DEBUGGER_CONNECT_LOGGER = false;
#endif

        public enum State : int
        {
            RUNNING,
            LOADING,
            SHUTDOWN
        }

        public static DebugLogger DebugLogger;

        public static Command LastCommand
        {
            get; private set;
        }

        private static int Tick = 0;
        //program state
        private static State _state = State.LOADING;

        static void Main(string[] args)
        {

            Console.SetBufferSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
  
            Console.ResetColor();
            Console.Title = "Lyds Text Adventure";

            CommandManager.AddDefaultCommands();
            ConsoleManager.DisableQuickEdit();

            Program.DebugLogger = new DebugLogger(Program.DEBUGGER_LOG_SIZE);
           
            DebugLogger.CreateExitEvent();

            //Create the buffer/viewable draw space
            Buffer.Create(Console.LargestWindowWidth - 2, Console.LargestWindowHeight - 2);

            //adds the remote logger
#if DEBUG

            try
            {
                if (Program.DEBUGGER_CONNECT_LOGGER)
                    Program.DebugLogger.ConnectDebugLogger();
            }
            catch
            {

                Program.DebugLog("failed to connect to debugger");
            }

            Program.DebugLog("connected to console log successfully");
#endif
            //register our scenes
            Scenes.RegisterScenes();
            //start test scene
            SceneManager.StartScene("sceneMenu");

            _state = State.RUNNING;

            //game loop
            while (!_state.Equals(State.SHUTDOWN))
            {

                Console.CursorVisible = false;
                Buffer.Clear();

                if (InputController.IsAwaitingInput && !InputController.IsRunning)
                {

                    Task.Factory.StartNew(() =>
                    {
                        InputController.KeyboardInput input = InputController.GetKeyboardInput();
                        Command command = CommandManager.GetCommand(input.text);

                        if (command == null)
                        {

                            if (input.keys.Count == 0)
                                return;

                            command = CommandManager.GetCommandByConsoleKey(input.keys[0].Key);

                            if (command == null)
                            {
                                Program.DebugLog("invalid command:" + input.text);
                                return;
                            }
                      
                        }

                        command.Execute();
                        Program.LastCommand = command;
                    });
                }

                //update then draw scene but only on even ticks (this helps with smoothness)
                if (SceneManager.IsSceneActive())
                    SceneManager.UpdateScene();

                //Draw the scene
                SceneManager.DrawScene();
                //takes all of our draw data and adds it to the buffer
                Buffer.PrepareBuffer();
                //Draws the buffer for the next frame
                Buffer.DrawBuffer();
     
#if DEBUG

                if (Program.Tick % 1024 == 0)
                    Program.DebugLog("check alive");

                //send console messages every 1 ticks as if its to fast it will throw exception
                if (Program.Tick % 1 == 0 && Program.DebugLogger.StackPosition > Program.DebugLogger.LastPosition)      {
                    //increments the post position by one if it is less than the stack position and sends that message to the console
                    Program.DebugLogger.WriteLine(Program.DebugLogger.DebugLog[Program.DebugLogger.LastPosition++]);
                }
                 
#endif

                //reset tick if we are over 8192
                if (Program.Tick >= Program.MAX_TICK)
                {

                    System.Console.Clear();
                    Program.DebugLog("screen cleaned");
                    Program.Tick = 0;
                }
                else
                    Program.Tick++;
            }

#if DEBUG
            //Program.DebugLogger.WriteShutdown();
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
                }, "m")
            });
        }

        public static void DebugLog(string msg, string op = "general")
        {

            if (msg.Length.Equals(0))
                return;

            string str = string.Concat("[", op + ":" + Program.Tick + ":" + DateTime.Now.TimeOfDay + "] ", msg);

            try
            {
                if (Program.DebugLogger.DebugLog == null || Program.DebugLogger.StackPosition + 1 >= Program.DebugLogger.DebugLog.Length)
                {
                    Program.DebugLogger.StackPosition = 0;
                    Program.DebugLogger.LastPosition = 0;
                    Program.DebugLogger.DebugLog = new string[Program.DEBUGGER_LOG_SIZE];
                }

                //put it onto the stack
                Program.DebugLogger.DebugLog[Program.DebugLogger.StackPosition++] = str;
            } catch {
                   //dont crash cuz we can't log
            }
        }

        public static string[] GetDebugLog()
        {

            string[] debugLog = (string[])DebugLogger.DebugLog.Clone();
            Array.Reverse(debugLog);
            return (debugLog);
        }

        public static int GetTick()
        {

            return Program.Tick;
        }
    }
}
