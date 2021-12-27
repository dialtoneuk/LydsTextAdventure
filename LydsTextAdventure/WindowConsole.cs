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
            this.SetTitle("Console");
            this.Show();
            this.SetSize(80, 16);

            this.console.SetPosition(1, 1);
            this.console.DockType = GuiElement.Dock.FILL;

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

            base.Update();
        }

        public override bool IsVisible()
        {

            return base.IsVisible();
        }
    }
}
