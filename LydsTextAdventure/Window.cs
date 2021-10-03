using System;
using System.Collections.Generic;
using System.Text;

namespace LydsTextAdventure
{
    public class Window
    {

        public readonly Position position = new Position(0, 0);
        public readonly List<GuiElement> guiElements = new List<GuiElement>();

        public string title = "Default Window";
        public readonly string id = Guid.NewGuid().ToString();
        public int index = 0;

        public Window(string title="")
        {

            if (title != "")
                this.title = title;
        }

        public override string ToString()
        {

            return this.id + ":" + this.title + "[" + this.index + "]";
        }

        public void SetIndex(int index)
        {

            this.index = index;
        }

        public virtual void Initialize()
        {



        }

        public void RegisterElement(GuiElement element)
        {

            guiElements.Add(element);
        }


        public virtual void Draw()
        {


        }
    }
}
