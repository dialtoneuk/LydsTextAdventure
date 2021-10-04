using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
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
        private Command successCommand;
        private static bool taskRunning = false;

        private static string userInput = "";

        //our input loop task
        public static void InputTask()
        {

            Input.taskRunning = true;
            Input input = Program.GetInputController();
 
            while ( input.awaitingInput || input.textInput || !(input.currentKey = input.GetKey()).Key.Equals(input.breakProgram))
            {

                Program.SetState(Program.State.AWAITING_INPUT);

                ConsoleKey key = input.GetKey().Key;

                if (input.textInput)
                {

                    if(key != ConsoleKey.Enter)
                    {
                        userInput = userInput + key.ToString();
                        Program.DebugLog(userInput);
                        continue;
                    } else if(key == ConsoleKey.Backspace){
                        userInput = userInput.Substring(0, userInput.Length - 1);
                        continue;
                    } 
                }                   
                else
                    userInput = key.ToString();

                userInput = userInput.ToLower();
                Command command = Program.GetCommandController().GetCommand(userInput);

                if (command == null)
                {
                    Program.DebugLog("command not found " + userInput, "input");
                    userInput = "";
                }
                else
                {

                    input.lastCommand = command;
                    input.successCommand = command;
                    Program.SetState(Program.State.RUNNING);
                    Input.taskRunning = false;
                    return;
                }
            }

            Input.taskRunning = false;
            input.lastCommand = null;
            return;
        }

        public static string GetString()
        {

            return Input.userInput;
        }

        public static bool IsTaskRunning()
        {

            return Input.taskRunning;
        }

        public bool IsAwaitingInput()
        {

            return (this.awaitingInput);
        }

        public Command GetLastCommand()
        {

            return this.lastCommand;
        }

        public Command GetLastSuccessfulCommand()
        {

            return this.successCommand;
        }

        public void ClearCommand()
        {

            this.lastCommand = null;
        }

        public void ToggleTextInput()
        {

            this.textInput = !this.textInput;
            Program.DebugLog("toggled text input", "input");
        }

        public void SetTextInput(bool input)
        {

            this.textInput = input;
        }

        public void SetAwaitingInput(bool input)
        {

            this.awaitingInput = input;
        }

        public void ToggleAwaitingInput()
        {

            this.awaitingInput = !this.awaitingInput;
            Program.DebugLog("toggled input", "input");
        }

        private ConsoleKeyInfo GetKey()
        {

            return (Console.ReadKey(true));
        }
    }
}
