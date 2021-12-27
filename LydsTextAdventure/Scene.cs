using System.Collections.Generic;

namespace LydsTextAdventure
{
    abstract class Scene
    {

        public readonly string sceneName;
        private List<Command> sceneCommands;

        public Scene(string name, List<Command> commands = null)
        {

            this.sceneName = name;

            if (commands != null)
                if (this.sceneCommands != null)
                {

                    List<Command> _commands = new List<Command>(this.sceneCommands.Count + commands.Count);
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
            InputController.IsAwaitingInput = false;
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

            if (commands != null && commands.Count != 0)
            {

                List<Command> _commands = new List<Command>(this.sceneCommands.Count + commands.Count);
                _commands.AddRange(this.sceneCommands);
                _commands.AddRange(commands);
                this.sceneCommands = _commands;
            }

            foreach (Command command in this.sceneCommands)
                if (CommandManager.IsCommandUnique(command))
                    CommandManager.Add(command);

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
            foreach (Entity entity in EntityManager.GetEntitiesByType(typeof(Camera)))
            {

                Camera camera = (Camera)entity;
                camera.Draw(camera.position.x, camera.position.y, camera);
            }

            //then draw windows
            WindowManager.DrawWindows();
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

            //then update windows
            WindowManager.UpdateWindows();

            //cache alive and visible entities;
            EntityManager.CacheEntities();
        }
    }
}
