using System;
using System.Collections.Generic;
using System.Text;

namespace LydsTextAdventure
{
    class WindowMap : Window
    {


        protected Player player;

        public override void Initialize()
        {

            this.SetName("world_map");
            this.SetTitle("World Map");
            this.SetSize(80, 16);
            this.BorderColour = System.ConsoleColor.Yellow;
            this.BorderDouble = false;
            this.Show();
        }
    }
}
