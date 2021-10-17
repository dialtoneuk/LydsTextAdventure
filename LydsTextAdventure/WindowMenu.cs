namespace LydsTextAdventure
{
    public class WindowMenu : Window
    {

        //gui elements
        protected GuiButton newGame = new GuiButton();
        protected GuiButton settings = new GuiButton();
        protected GuiButton exitGame = new GuiButton();
        protected GuiMultiline menuText = new GuiMultiline();

        public override void Initialize()
        {

            this.SetName("menu");

            this.newGame.SetPosition(2, 2);
            this.newGame.SetSize(15, 5);
            this.newGame.SetText("New Game");
            this.newGame.SetOnClick((button) =>
            {

                SceneManager.EndScene();
                SceneManager.StartScene("sceneGame");
            });

            this.settings.SetPosition(2, 8);
            this.settings.SetSize(15, 5);
            this.settings.SetText("Settings");

            this.exitGame.SetPosition(2, 14);
            this.exitGame.SetSize(15, 5);
            this.exitGame.SetText("Exit Game");
            this.exitGame.SetOnClick((button) =>
            {

                Program.SetState(Program.State.SHUTDOWN);
            });

            this.menuText.SetPosition(18, 2);
            this.menuText.SetSize(60, 18);

            this.menuText.AddText("Welcome!");
            this.menuText.AddText(" Press [q] to click on a button.");
            this.menuText.AddText("");
            this.menuText.AddText("Written by Llydia Cross 2021");

            this.SetTitle("Main Menu");
            this.Show();
            this.SetSize(60, 20);

            this.SetPosition(1,1);

            //various buttons
            this.RegisterElements(newGame, exitGame, settings, menuText);
        }
    }
}
