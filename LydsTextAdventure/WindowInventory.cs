namespace LydsTextAdventure
{
    class WindowInventory : Window
    {

        protected GuiList group = new GuiList();
        protected Player player;

        public override void Initialize()
        {

            this.SetName("player_inventory");
            this.SetTitle("Player Inventory");
            this.SetSize(40, 32);
            this.Show();

            this.group.SetPosition(1, 1);
            this.group.DockType = GuiElement.Dock.FILL;

            //drawing and updating are donw by the group
            this.group.SetUpdateElements(true);
            this.group.SetDrawElements(true);

            this.RegisterElement(group);
        }

        public void SetPlayer(Player player)
        {
            this.player = player;
        }

        //TODO: Put this into a call or something which isn't everyframe but when stuff is updated???
        public override void Update()
        {


            if (player != null)
            {

                this.group.RemoveElements();

                foreach (Item item in this.player.Inventory.items)
                {

                    GuiItem _i = new GuiItem();
                    _i.SetItem(item);
                    this.group.AddElements(_i);
                }
            }

            base.Update();
        }
    }
}
