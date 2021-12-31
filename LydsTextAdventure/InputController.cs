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

            Program.HookManager.CallHook("InputStart", HookManager.Groups.Input);

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
                    Program.HookManager.CallHook("GetKey", HookManager.Groups.Input, key);
                    break;
                }
                else
                {

                    input.text += key.Key.ToString();
                    Program.HookManager.CallHook("InputAppended", HookManager.Groups.Input, input.text);
                }

                Program.HookManager.CallHook("KeysAppended", HookManager.Groups.Input, key);
                input.keys.Add(key);
            }


            InputController.IsRunning = false;
            InputController.LastKeyboardInput = input;
            Program.HookManager.CallHook("InputEnd", HookManager.Groups.Input, input);
            return input;
        }
    }
}
