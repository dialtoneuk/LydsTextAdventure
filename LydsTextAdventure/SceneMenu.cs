using System;
using System.Collections.Generic;
using System.Text;

namespace LydsTextAdventure
{
    class SceneMenu : Scene
    {

        public SceneMenu(string name, List<Command> commands = null ) : base( name, commands )
        {}

        protected override List<Command> LoadCommands()
        {

            return new List<Command>(){
                new Command("test", () => {
                    Console.WriteLine("test");
                })
            };
        }
    }
}
