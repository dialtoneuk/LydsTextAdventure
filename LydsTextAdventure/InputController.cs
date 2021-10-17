using System;
using System.Collections.Generic;

namespace LydsTextAdventure
{
    public class InputController
    {

        public static bool IsTextInput = false;
        public static bool IsAwaitingInput = false;
        public static bool IsRunning = false;
        public static KeyboardInput LastKeyboardInput;

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

                InputController.IsRunning = true;
                ConsoleKeyInfo key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Escape)
                    continue;

                if (key.Key == ConsoleKey.Enter)
                    break;

                if (!IsTextInput)
                {
                    input.text = key.Key.ToString();
                    input.keys.Add(key);
                    break;
                }
                else
                {
                    input.text += key.Key.ToString();
                }

                input.keys.Add(key);
            }


            InputController.IsRunning = false;
            InputController.LastKeyboardInput = input;
            return input;
        }
    }
}
