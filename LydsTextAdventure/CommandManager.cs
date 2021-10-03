﻿using System;
using System.Collections.Generic;
using System.Text;

namespace LydsTextAdventure
{
    class CommandManager
    {

        protected List<Command> defaultCommands = new List<Command>()
        {
            //exit command
            new Command("break", () => {
           
                 System.Environment.Exit(0);
            }),
            new Command("commands", () =>
            {
                //log commands to printer here
            })
        };

        protected List<Command> commands = new List<Command>();

        public CommandManager()
        {

            this.commands.AddRange(defaultCommands);
        }

        public void Add(Command command)
        {

            if (!this.IsCommandUnique(command))
                 throw new ApplicationException("command is not unique: " + command.ToString());

            this.commands.Add(command);
        }
        
        public void Clear()
        {

            this.commands.Clear();
            this.commands.AddRange(defaultCommands);
        }

        public void Register(List<Command> commands)
        {

            foreach(Command command in commands)
            {
                this.Add(command);
            }
        }

        //returns true if a command is unique, meaning its command name and short name are not similar
        public bool IsCommandUnique(Command command)
        {

            foreach (Command _command in this.commands)
                if (command.IsSimilar(_command))
                    return false;

            return true;
        }

        //gets a command
        public Command GetCommand(string search_key)
        {

            foreach (Command command in this.commands)
                if (command.SearchTermMatches(search_key))
                    return command;

            return null;
        }
    }
}