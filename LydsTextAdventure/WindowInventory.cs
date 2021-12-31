using System;
using System.Collections.Generic;
using System.Text;

namespace LydsTextAdventure
{
    class WindowInventory : Window
    {

        protected GuiGroup group = new GuiGroup();
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

                int count = 0;

                foreach (Item item in this.player.Inventory.items)
                {

                    GuiItem _i = new GuiItem();

                    //look one ahead and see if it is out of the current windows rectangle
                    if (this.GetRectangle(false).IsInsideRectangle(this.position.x, this.position.y + ((count + 1) * _i.Height)))
                    {

                        _i.SetItem(item);
                        this.group.AddElements(_i);
                        count++;
                    }
                    else
                    {

                        //if it is, put this label here saying some items are hidden
                        GuiLabel label = new GuiLabel();
                        label.SetText("-[Items Hidden]"); //looks nice
                        this.group.AddElements(label);
                        break;
                    }

                }
            }

            base.Update();
        }
    }
}
