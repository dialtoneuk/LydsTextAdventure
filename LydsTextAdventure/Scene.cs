﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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

        public virtual void Destroy()
        {

            WorldManager.ClearWorlds();
            EntityManager.DestroyAllEntities();
        }

        public override string ToString()
        {

            return this.sceneName;
        }

        protected virtual List<Command> LoadCommands()
        {

            return null;
        }

        public void Load()
        {

            Program.DebugLog("base load called");

            List<Command> commands = this.LoadCommands();

            if(commands!=null && commands.Count != 0 )
            {

                List<Command> _commands = new List<Command>(this.sceneCommands.Count + commands.Count);
                _commands.AddRange(this.sceneCommands);
                _commands.AddRange(commands);
                this.sceneCommands = _commands;
            }

            foreach (Command command in this.sceneCommands)
                if (Program.GetCommandController().IsCommandUnique(command))
                    Program.GetCommandController().Add(command);

            this.Before();

            return;
        }

        public virtual void Before()
        {

            Program.DebugLog("scene before finished");
            //load assets and stuff here
            return;
        }

        public virtual void Draw()
        {

            //draws all the cameras in the scene
            foreach(Entity entity in EntityManager.GetEntitiesByType(typeof(Camera)))
            {

                Camera camera = (Camera)entity;
                camera.Draw(camera.position.x, camera.position.y, camera);
            }
        }

        public virtual void Start()
        {

            Program.DebugLog("base start called");

            return;
        }

        public virtual void Update()
        {

            //Update the world
            WorldManager.UpdateWorlds();

            //then update entities
            EntityManager.UpdateEntities();

            //recache visible and alive entities (helps performance with multiple cameras)
            if(Program.GetTick() % 248 == 0)
            {
                EntityManager.GetVisibleEntities(true); //caches which helps with multiple cameras
                EntityManager.GetAliveEntities(true); //caches
            }
        }
    }
}
