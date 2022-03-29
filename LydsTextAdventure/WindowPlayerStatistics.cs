namespace LydsTextAdventure
{
    public class WindowPlayerStatistics : Window
    {

        protected Player player = null;

        protected GuiGroup group = new GuiGroup();

        //gui elements
        protected GuiLabel health = new GuiLabel();
        protected GuiLabel stanima = new GuiLabel();
        protected GuiLabel playerPosition = new GuiLabel();
        protected GuiLabel name = new GuiLabel();
        protected GuiLabel lastCommand = new GuiLabel();

        public override void Initialize()
        {

            this.SetName("player_statistics");
            this.SetTitle("Player Statistics");
            this.SetSize(40, 9);
            this.BorderColour = System.ConsoleColor.Red;
            this.Show();

            this.group.SetPosition(1, 1);
            this.group.DockType = GuiElement.Dock.FILL;

            //register them
            this.RegisterElements(this.group, this.health, this.stanima, this.name, this.playerPosition, this.lastCommand);
            //add it to the group
            this.group.AddElements(this.guiElements.ToArray());
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

            if (this.player != null)
            {

                string stub = "";
                for (int i = 0; i < this.player.Health / 5; i++)
                    stub += "+";

                this.health.SetText("Health [" + this.player.GetHealth().ToString() + "] " + stub);

                stub = "";
                for (int i = 0; i < this.player.stanima / 10; i++)
                    stub += "-";

                this.stanima.SetText("Stanima [" + this.player.stanima.ToString() + "] " + stub);
                this.name.SetText("Adventurer: " + this.player.Name);
                this.playerPosition.SetText(this.player.position.ToString());
            }

            Command cmd = Program.LastCommand;
            if (cmd != null)
                this.lastCommand.SetText("last executed: " + cmd.ToString());

            base.Update();
        }


        public override bool IsVisible()
        {

            return base.IsVisible();
        }
    }
}
