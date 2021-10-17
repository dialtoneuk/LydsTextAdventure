using System;
using System.Collections.Generic;

namespace LydsTextAdventure
{
    class CommandManager
    {
        private static readonly List<Command> DefaultCommands = new List<Command>()
        {
            //exit command
            new Command("exit", () => {
                Program.SetState(Program.State.SHUTDOWN);
            }, "p", ConsoleKey.P),
            new Command("click", () =>
            {

                Position pos = InputController.GetMousePosition();
                foreach(Window window in WindowManager.GetOpenWindows())
                {

                    foreach(GuiElement element in window.guiElements)
                        if(GuiElement.IsInsideOf(pos, element))
                        {
                            element.OnClick();
                            break;
                        } 
                }

                foreach(Entity entity in EntityManager.GetVisibleEntities())
                {

                        if(Entity.IsMouseOver(pos, entity))
                        {
                            entity.OnClick();
                            break;
                        }               
                }
            }, "q", ConsoleKey.Q)
        };

        protected static List<Command> Commands = new List<Command>();

        public static void AddDefaultCommands()
        {

            CommandManager.Commands.AddRange(CommandManager.DefaultCommands);
        }

        public static List<Command> GetCommands()
        {

            return CommandManager.Commands;
        }

        public static void Add(Command command)
        {

            if (!CommandManager.IsCommandUnique(command))
                throw new ApplicationException("command is not unique: " + command.ToString());

            CommandManager.Commands.Add(command);
        }

        public static void Clear()
        {

            CommandManager.Commands.Clear();
            CommandManager.Commands.AddRange(CommandManager.DefaultCommands);
        }

        public static void Register(List<Command> commands)
        {

            foreach (Command command in commands)
                CommandManager.Add(command);
        }

        //returns true if a command is unique, meaning its command name and short name are not similar
        public static bool IsCommandUnique(Command command)
        {

            foreach (Command _command in CommandManager.Commands)
                if (command.IsSimilar(_command))
                    return false;

            return true;
        }

        //gets a command
        public static Command GetCommand(string search_key)
        {

            foreach (Command command in CommandManager.Commands)
                if (command.SearchTermMatches(search_key))
                    return command;

            return null;
        }

        public static Command GetCommandByConsoleKey(ConsoleKey key)
        {

            foreach (Command command in CommandManager.Commands)
                if (command.SearchKeyMatches(key))
                    return command;

            return null;
        }
    }
}
