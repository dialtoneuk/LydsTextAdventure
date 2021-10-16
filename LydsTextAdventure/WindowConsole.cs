using System;
using System.Collections.Generic;
using System.Text;

namespace LydsTextAdventure
{
    public class WindowConsole : Window
    {

        protected Player player = null;

        //gui elements
        protected GuiMultiline console = new GuiMultiline();

        public override void Initialize()
        {

            this.SetName("console");

            this.console.SetPosition(1, 1);
            this.console.SetSize(79, 14);

            this.SetTitle("Console");
            this.Show();
            this.SetSize(80, 16);

            this.RegisterElements(this.console);
        }

        public void SetPlayer(Player player)
        {

            this.player = player;
        }

        public override void Draw()
        {


        }

        public override void Update()
        {

            string[] strs = Program.GetDebugLog();
            this.console.Clear();

            foreach (string str in strs)
                this.console.AddText(str);
        }

        public override bool IsVisible()
        {

            return base.IsVisible();
        }
    }
}
