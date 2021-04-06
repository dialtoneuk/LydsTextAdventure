using System;
using System.Collections.Generic;
using System.Text;

namespace LydsTextAdventure
{
    class Input
    {

        private ConsoleKey breakProgram = ConsoleKey.End;
        private ConsoleKeyInfo currentKey;
        private bool textInput = true;
        private bool awaitingInput = true;
        private Command lastCommand;

        //our input loop task
        public static void InputTask()
        {

            Input input = Program.GetInput();

            while ( input.awaitingInput || input.textInput || !(input.currentKey = input.GetKey()).Key.Equals(input.breakProgram))
            {

                Program.SetState(Program.State.AWAITING_INPUT);

                string userInput;
                if (input.textInput)
                    userInput = input.GetLine();
                else
                    userInput = input.GetReadKey();

                Command command = Program.GetCommands().GetCommand(userInput);

                if (command == null)
                    Console.WriteLine("command not found {0}", userInput);
                else
                {

                    input.lastCommand = command;
                    Program.SetState(Program.State.RUNNING);
                    return;
                }
            }

            input.lastCommand = null;
            return;
        }

        public bool isAwaitingInput()
        {

            return (this.awaitingInput);
        }

        public Command GetCommand()
        {

            return this.lastCommand;
        }

        public void ToggleTextInput()
        {

            this.textInput = !this.textInput;
        }

        public void ToggleAwaitingInput()
        {

            this.awaitingInput = !this.awaitingInput;
        }


        public string GetReadKey()
        {

            return (currentKey.Key.ToString());
        }

        //adds the missing key along with the line and makes it lower
        private string GetLine()
        {

            return (Console.ReadLine());
        }

        private ConsoleKeyInfo GetKey()
        {

            return (Console.ReadKey());
        }
    }
}
