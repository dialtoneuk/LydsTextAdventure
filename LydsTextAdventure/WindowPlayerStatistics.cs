namespace LydsTextAdventure
{
    public class WindowPlayerStatistics : Window
    {

        protected Player player = null;

        protected GuiGroup group = new GuiGroup();

        //gui elements
        protected GuiLabel health = new GuiLabel();
        protected GuiLabel armor = new GuiLabel();
        protected GuiLabel playerPosition = new GuiLabel();
        protected GuiLabel name = new GuiLabel();
        protected GuiLabel input = new GuiLabel();
        protected GuiLabel lastCommand = new GuiLabel();

        public override void Initialize()
        {

            this.SetName("player_statistics");
            this.SetTitle("Player Statistics");
            this.SetSize(40, 8);
            this.Show();

            this.group.SetPosition(1, 1);
            this.group.DockType = GuiElement.Dock.FILL;

            //register them
            this.RegisterElements(this.group, this.health, this.armor, this.name, this.playerPosition, this.input, this.lastCommand);
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

                this.health.SetText("hp: " + this.player.GetHealth().ToString());
                this.armor.SetText("armor: XXXXXX");
                this.playerPosition.SetText("position: " + this.player.position.ToString());
                this.name.SetText("name: " + this.player.GetName());
            }

            this.input.SetText("mouse pos: " + ConsoleManager.GetMousePosition());

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
