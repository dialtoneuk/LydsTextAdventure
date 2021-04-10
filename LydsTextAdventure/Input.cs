using System;
using System.Collections.Generic;
using System.Text;

namespace LydsTextAdventure
{
    class Input
    {

        private ConsoleKey breakProgram = ConsoleKey.End;
        private ConsoleKeyInfo currentKey;
        private bool textInput = false;
        private bool awaitingInput = false;
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
                    userInput = input.GetKey().Key.ToString();

                Command command = Program.GetCommands().GetCommand(userInput);

                if (command == null)
                    Program.WriteLine("command not found " + userInput, "input");
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

            Program.WriteLine("awaiting text input", "input");
            this.textInput = !this.textInput;
        }

        public void ToggleAwaitingInput()
        {

            Program.WriteLine("awaiting input", "input");
            this.awaitingInput = !this.awaitingInput;
        }

        //adds the missing key along with the line and makes it lower
        private string GetLine()
        {

            return (Console.ReadLine());
        }

        private ConsoleKeyInfo GetKey()
        {

            return (Console.ReadKey(true));
        }
    }
}
