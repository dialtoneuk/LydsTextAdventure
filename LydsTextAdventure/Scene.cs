﻿using System;
using System.Collections.Generic;
using System.Text;

namespace LydsTextAdventure
{
    abstract class Scene
    {

        public readonly string sceneName;
        private List<Command> sceneCommands;

        public Scene(string name, List<Command> commands=null)
        {

            this.sceneName = name;

            if (commands != null)
                if (this.sceneCommands != null)
                {

                    List<Command> _commands = new List<Command>(sceneCommands.Count + commands.Count);
                    _commands.AddRange(this.sceneCommands);
                    _commands.AddRange(commands);
                    this.sceneCommands = _commands;
                }
                else
                    this.sceneCommands = commands;
            else
                if (this.sceneCommands == null)
                    this.sceneCommands = new List<Command>();
        }

        
        protected virtual List<Command> LoadCommands()
        {

            return null;
        }

        public void Load()
        {

            List<Command> commands = this.LoadCommands();

            if(commands!=null && commands.Count != 0 )
            {

                List<Command> _commands = new List<Command>(this.sceneCommands.Count + commands.Count);
                _commands.AddRange(this.sceneCommands);
                _commands.AddRange(commands);
                this.sceneCommands = _commands;
            }

            foreach (Command command in this.sceneCommands)
                if (Program.GetCommands().IsCommandUnique(command))
                    Program.GetCommands().Add(command);

            this.Before();

            return;
        }

        public virtual void Before()
        {

            //load assets and stuff here
            return;
        }


        public virtual void Start()
        {

            return;
        }

        public virtual void Update()
        {


            return;
        }
    }
}