using System;
using System.Collections.Generic;
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
        private static bool taskRunning = false;

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool ReadConsoleOutputCharacter(
            IntPtr hConsoleOutput,
            [Out] StringBuilder lpCharacter,
            uint length,
            COORD bufferCoord,
            out uint lpNumberOfCharactersRead);

        [StructLayout(LayoutKind.Sequential)]
        public struct COORD
        {
            public short X;
            public short Y;
        }

        public static char ReadCharacterAt(int x, int y)
        {
            IntPtr consoleHandle = GetStdHandle(-11);
            if (consoleHandle == IntPtr.Zero)
            {
                return '\0';
            }
            COORD position = new COORD
            {
                X = (short)x,
                Y = (short)y
            };
            StringBuilder result = new StringBuilder(1);
            uint read = 0;
            if (ReadConsoleOutputCharacter(consoleHandle, result, 1, position, out read))
            {
                return result[0];
            }
            else
            {
                return '\0';
            }
        }

        //our input loop task
        public static void InputTask()
        {

            Input.taskRunning = true;
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
                    Program.DebugLog("command not found " + userInput, "input");
                else
                {

                    input.lastCommand = command;
                    Program.SetState(Program.State.RUNNING);
                    Input.taskRunning = false;
                    return;
                }
            }

            Input.taskRunning = false;
            input.lastCommand = null;
            return;
        }

        public static bool IsTaskRunning()
        {

            return Input.taskRunning;
        }

        public bool IsAwaitingInput()
        {

            return (this.awaitingInput);
        }

        public Command GetCommand()
        {

            return this.lastCommand;
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

        public void ToggleAwaitingInput()
        {

            this.awaitingInput = !this.awaitingInput;
            Program.DebugLog("toggled input", "input");
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
