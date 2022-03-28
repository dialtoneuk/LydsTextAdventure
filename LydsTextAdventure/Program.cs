using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LydsTextAdventure
{

    class Program
    {


        public const int MAX_TICK = 8912 * 8912;
        public const int DEBUGGER_LOG_SIZE = 2048;
        public const string INITIAL_SCENE = "sceneMenu";
        public const bool TURBO_MODE = false;

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
        public static HookManager HookManager = new HookManager();

        public static Command LastCommand
        {
            get; private set;
        }

        private static int Tick = 0;
        public static int Clock
        {
            get;
            private set;
        }
        //program state
        public static State ProgramState
        {
            get;
            private set;
        }

        static void Main(string[] args)
        {
            ProgramState = State.LOADING;

            Console.Title = "Lyds Text Adventure";

            //setup buffer size
            Console.SetBufferSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            //Create the buffer/viewable draw space
            Buffer.Create(Console.LargestWindowWidth - 2, Console.LargestWindowHeight - 2);

            //reset colours and disable quick edit
            Console.ResetColor();
            ConsoleManager.DisableQuickEdit();

            //clear the console and create new debug logger instance
            Program.Clear();
            Program.DebugLogger = new DebugLogger(Program.DEBUGGER_LOG_SIZE);
            DebugLogger.CreateExitEvent();

            //load game
            Program.LoadGame();

            //start menu scene
            SceneManager.StartScene(INITIAL_SCENE);

            //start gameloop
            Program.GameLoop();
        }

        private static void LoadGame()
        {
            //add default commands
            CommandManager.AddDefaultCommands();

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
        }

        private static void ReintroduceInputThread()
        {

            //get the input
            if (InputController.IsAwaitingInput && !InputController.IsRunning)
            {

                Task.Run(() =>
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
        }

        private static void GameLoop()
        {

            if (ProgramState == State.RUNNING)
                throw new ApplicationException("state already running");

            //set state to running
            ProgramState = State.RUNNING;

            //start in a new thread
            Task.Factory.StartNew(() =>
            {



                //game loop
                while (!ProgramState.Equals(State.SHUTDOWN))
                {

                    Program.Tick++;

                    //check dat input thread
                    Program.ReintroduceInputThread();

                    //update then draw scene
                    if (SceneManager.IsSceneActive() && SceneManager.isReadyToDraw)
                    {

                        if (TURBO_MODE && Program.GetTick() % 10 == 0)
                            Buffer.Clear();

                        if (!TURBO_MODE && Program.GetTick() % 2 == 0)
                            Buffer.Clear();

                        SceneManager.DrawScene();
                        SceneManager.isReadyToDraw = false;
                    }
#if DEBUG
                    //send alive to console
                    if (Program.Tick % 1024 == 0)
                        Program.DebugLog("check alive");

                    //send console messages every 1 ticks as if its to fast it will throw exception
                    if (Program.Tick % 1 == 0 && Program.DebugLogger.StackPosition > Program.DebugLogger.LastPosition)
                    {
                        //increments the post position by one if it is less than the stack position and sends that message to the console
                        Program.DebugLogger.WriteLine(Program.DebugLogger.DebugLog[Program.DebugLogger.LastPosition++]);
                    }

#endif
                    //reset tick if we are over 8192
                    if (Program.Tick >= Program.MAX_TICK)
                    {
                        Program.DebugLog("screen cleaned");
                        Program.Tick = 0;
                    }

                    if (Buffer.isReady)
                    {
                        Buffer.DrawBuffer();
                        Buffer.isReady = false;
                    }

                    if (!TURBO_MODE)
                        Thread.Sleep(1);
                }

                //shutdown if we escape the loop
                Program.Shutdown();
            });

            //start secondary thread for our updates.
            Task.Factory.StartNew(() =>
            {

                //game loop
                while (!ProgramState.Equals(State.SHUTDOWN))
                {

                    //update then draw scene
                    if (SceneManager.IsSceneActive() && !SceneManager.isReadyToDraw)
                    {

                        SceneManager.UpdateScene();
                        //update the scene, once the update hook is called, the application will then draw
                        Task.Run(SceneManager.ThreadedUpdateScene);
                    }

                    if (!Buffer.isReady && SceneManager.isReadyToDraw)
                        Buffer.PrepareBuffer();
                }

                Program.Shutdown();
            });

            //creates fps hook
            Buffer.CreateHook();

            Program.StartClock();

            //sleep this thread
            Thread.Sleep(Timeout.Infinite);
        }

        private static void StartClock()
        {

            if (!ProgramState.Equals(State.SHUTDOWN))
                Task.Delay(1000).ContinueWith((task) =>
                {

                    HookManager.CallHook("ClockTick", HookManager.Groups.Global);

                    //increment clock
                    Program.Clock++;

                    Program.StartClock();
                });
        }

        public static void Stop()
        {

            ProgramState = State.SHUTDOWN;
        }

        public static void Shutdown()
        {

#if DEBUG
            Program.DebugLogger.WriteShutdown();
#endif

            System.Environment.Exit(0);
        }

        public static void Clear()
        {

            System.Console.Clear();
            Console.ForegroundColor = (ConsoleColor)(new Random().Next(0, 15));

            if (Console.ForegroundColor == ConsoleColor.Black)
                Console.ForegroundColor = ConsoleColor.White;
        }

        public static void SetState(State state)
        {

            Program.ProgramState = state;
        }

        public static void RegisterDebugCommands()
        {

            CommandManager.Register(new List<Command>()
            {
                new Command("clean_screen", () => {
                        Program.Clear();
                }, "m")
            });
        }

        public static void DebugLog(string msg, string op = "general")
        {

            if (msg.Length.Equals(0))
                return;

            string str = string.Concat("[", op + ":" + Program.Tick + ":" + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "] ", msg);

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
            }
            catch
            {
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
