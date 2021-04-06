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

        //program state
        private static State programState = State.LOADING;

        static void Main(string[] args)
        {

            SceneManager.GetScene("menuScene").Load();
           
            while(!programState.Equals(State.SHUTDOWN))
            {

                Program.SetState(Program.State.RUNNING);

                if(Program.input.isAwaitingInput())
                {

                    Task inputTask = Task.Factory.StartNew(Input.InputTask);

                    //wait until we have a valid command
                    inputTask.Wait();

                    //if it isnt null, executes
                    if (Program.input.GetCommand() != null)
                        if (!Program.input.GetCommand().Execute())
                            throw new ApplicationException("must be true");
                }
            }
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
