using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LydsTextAdventure
{
    public class InputController
    {

        public static bool isTextInput = false;
        public static bool isAwaitingInput = false;
        public static bool isRunning = false;
        public static KeyboardInput lastKeyboardInput;

        public struct KeyboardInput
        {
            public string text;
            public List<ConsoleKeyInfo> keys;
        }

        public static Position GetMousePosition()
        {

            return ConsoleManager.GetMousePosition();
        }

        public static KeyboardInput GetKeyboardInput()
        {

            KeyboardInput input;
            input.text = "";
            input.keys = new List<ConsoleKeyInfo>();

            while (true)
            {

                InputController.isRunning = true;
                ConsoleKeyInfo key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Escape)
                    continue;

                if (key.Key == ConsoleKey.Enter)
                    break;

                if (!isTextInput)
                {
                    input.text = key.Key.ToString();
                    input.keys.Add(key);
                    break;
                } else {
                    input.text = input.text + key.Key.ToString();
                }

                input.keys.Add(key);
            }


            InputController.isRunning = false;
            InputController.lastKeyboardInput = input;
            return input;
        }
    }
}
