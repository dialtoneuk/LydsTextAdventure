using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LydsTextAdventure
{

    class Program
    {

        public enum State: int
        {
            AWAITING_INPUT,
            RUNNING,
            LOADING,
            SHUTDOWN
        }

        //create our classes
        private static readonly Input input = new Input();
        private static readonly Commands commands = new Commands();
        private static ConsoleLogger logger;

        private static int tick = 0;
        //program state
        private static State programState = State.LOADING;
       
        static void Main(string[] args)
        {

            //setwindow size
            Console.SetWindowSize(160, 72);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;

            Console.Title = "Lyds Text Adventure";

            //Create the buffer/viewable draw space
            Buffer.Create(128, 64);


            //adds the remote logger
#if DEBUG
            Program.logger = new ConsoleLogger();
            Program.DebugLog("connected to console log successfully");
            Program.AddDebugCommands();
#endif

            //register our scenes
            Scenes.RegisterScenes();
            //start test scene
            SceneManager.StartScene("sceneMenu");

            //game loop
            while (!programState.Equals(State.SHUTDOWN))
            {

                Buffer.Clear();

                //if we are awaiting input, lets get it
                if (Program.input.IsAwaitingInput() && !Input.IsTaskRunning())
                    Task.Factory.StartNew(Input.InputTask);

                if (Program.input.GetCommand() != null)
                    if (!Program.input.GetCommand().Execute())
                        throw new ApplicationException("must be true");
                    else
                        Program.input.ClearCommand();

                //update then draw scene
                if (SceneManager.IsSceneActive())
                    SceneManager.UpdateScene();

                //generate buffer
                Buffer.GenerateBuffer();

                //forever counts
                Program.tick++;

                if (Program.tick > 4086)
                    Program.tick = 0;

                //Draw it
                Task.Factory.StartNew(Buffer.DrawBuffer).Wait();
            }
        }

        public static void AddDebugCommands()
        {

            Program.commands.Register(new List<Command>()
            {
                new Command("clean_screen", () => {
                        Console.Clear();
                }, "z"),
                new Command("delete_entities", () => {
                    List<Entity> result = EntityManager.GetEntitiesByType(typeof(EntityMoving));

                    foreach(Entity ent in result)
                    {
                        ent.RemoveEntity();
                    }
                }, "m"),
            });
        }


        public static void DebugLog(string msg, string op="general" )
        {

            if (msg.Length.Equals(0))
                return;

            if (Program.logger is null)
                return;

            Program.logger.WriteLine( string.Concat( "[", op + ":" + DateTime.Now + ":" + Program.tick, "] ", msg));
        }

        public static void SetState(State state)
        {

            Program.programState = state;
        }

        public static int GetTick()
        {

            return Program.tick;
        }

        public static Commands GetCommandController()
        {

            return Program.commands;
        }

        public static Input GetInputController()
        {

            return Program.input;
        }
    }
}
