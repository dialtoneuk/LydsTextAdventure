using System;
using System.Collections.Generic;
using System.Text;

namespace LydsTextAdventure
{
    public class WindowPlayerStatistics : Window
    {

        protected Player player = null;

        //gui elements
        protected GuiLabel health = new GuiLabel();
        protected GuiLabel playerPosition = new GuiLabel();
        protected GuiLabel name = new GuiLabel();
        protected GuiLabel input = new GuiLabel();
        protected GuiLabel lastCommand = new GuiLabel();
 
        public override void Initialize()
        {

            this.health.SetPosition(1, 2);
            this.playerPosition.SetPosition(1, 3);
            this.name.SetPosition(1, 4);
            this.input.SetPosition(1, 5);
            this.lastCommand.SetPosition(1, 6);

            this.SetSize(40, 8);
            this.SetTitle("Player Statistics");
            this.Show();

            this.RegisterElements(this.health, this.name, this.playerPosition, this.input, this.lastCommand);
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
            
            if(this.player!=null)
            {

                this.health.SetText("hp: " + this.player.GetHealth().ToString());
                this.playerPosition.SetText("position: " + this.player.position.ToString());
                this.name.SetText("name: " + this.player.GetName());
            }

            this.input.SetText("mouse pos: " + ConsoleManager.GetMousePosition());

            Command cmd = Program.GetInputController().GetLastSuccessfulCommand();
            if ( cmd != null )
                this.lastCommand.SetText("last executed: " + cmd.ToString());
        }


        public override bool IsVisible()
        {

            return base.IsVisible();
        }
    }
}
