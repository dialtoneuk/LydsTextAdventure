#define DEBUG

using System;
using System.Threading.Tasks;

namespace LydsTextAdventure
{

    class Program
    {

        public enum State: int
        {
            AWAITING_INPUT,
            RUNNING,
            LOADING,
            SHUTDOWN
        }

        //create our classes
        private static readonly Input input = new Input();
        private static readonly Commands commands = new Commands();
        private static ConsoleLogger logger;

        //program state
        private static State programState = State.LOADING;
       
        static void Main(string[] args)
        {

            //adds the remote logger
#if DEBUG
            Program.logger = new ConsoleLogger();
            Program.DebugLog("connected to console log successfully");
#endif

            //start test scene
            SceneManager.StartScene("menuScene");

            //game loop
            while (!programState.Equals(State.SHUTDOWN))
            {

                Console.SetCursorPosition(0, 0);

                //if we are awaiting input, lets get it
                if (Program.input.IsAwaitingInput() && !Input.IsTaskRunning())
                    Task.Factory.StartNew(Input.InputTask);

                if (Program.input.GetCommand() != null)
                    if (!Program.input.GetCommand().Execute())
                        throw new ApplicationException("must be true");
                    else
                        Program.input.ClearCommand();

                //update then draw scene
                if (SceneManager.IsSceneActive())
                    SceneManager.UpdateScene();
            }
        }

        public static void DebugLog(string msg, string op="general" )
        {

            if (msg.Length.Equals(0))
                return;

            if (Program.logger is null)
                return;

            Program.logger.WriteLine( string.Concat( "[", op, "] ", msg));
        }

        public static void SetState(State state)
        {

            Program.programState = state;
        }

        public static Commands GetCommands()
        {

            return Program.commands;
        }

        public static Input GetInput()
        {

            return Program.input;
        }
    }
}
