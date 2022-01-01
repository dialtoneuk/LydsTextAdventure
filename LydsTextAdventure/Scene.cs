using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LydsTextAdventure
{
    public abstract class Scene
    {

        public readonly string sceneName;
        private List<Command> sceneCommands;
        private List<Entity> sceneCameras = new List<Entity>();

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

            Program.DebugLog("running before");

            this.Before();

            Program.DebugLog("loading commands");

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

            return;
        }

        public virtual void Before()
        {

            Program.DebugLog("scene before finished");
            //load assets and stuff here
            return;
        }

        public void FindSceneCameras()
        {

            this.sceneCameras = EntityManager.GetEntitiesByType(typeof(Camera));
        }

        public void AddSceneCamera(Camera camera)
        {

            foreach (Entity ent in this.sceneCameras)
                if (ent.id == camera.id)
                    throw new ApplicationException("invaid");

            this.sceneCameras.Add(camera);
        }

        public List<Entity> GetSceneCameras()
        {

            return this.sceneCameras;
        }

        public virtual void Draw()
        {

            //draws all the cameras in the scene
            List<Entity> list = this.sceneCameras;
            for (int i = 0; i < list.Count; i++)
            {
                Camera camera = (Camera)list[i];
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

            //only on even ticks
            if (Program.GetTick() % 2 == 0)
                //Update the world
                WorldManager.UpdateWorlds();

            //update entites
            EntityManager.UpdateEntities();

            //only on even ticks
            if (Program.GetTick() % 2 == 0)
                //then update windows
                WindowManager.UpdateWindows();

            if (Program.GetTick() % 512 == 0)
                //cache alive and visible entities
                EntityManager.CacheEntities();
        }
    }
}
