using System;
using System.Collections.Generic;
using System.Text;

namespace LydsTextAdventure
{
    public class GuiGroup : GuiElement
    {

        private bool shouldDraw = false;
        private bool shouldUpdate = false;
        protected List<GuiElement> groupElements = new List<GuiElement>();

        public void AddElements(params GuiElement[] elements)
        {

            foreach (GuiElement element in elements)
            {

                if (element.Equals(this))
                    continue;

                element.Group = this;
                element.Parent = this;
                element.Window = this.Window;
                groupElements.Add(element);
            }
        }

        public void SetDrawElements(bool shouldDraw)
        {

            this.shouldDraw = shouldDraw;
        }

        public void SetUpdateElements(bool shouldUpdate)
        {

            this.shouldUpdate = shouldUpdate;
        }

        public void RemoveElements()
        {

            this.groupElements.Clear();
        }

        public override void Update()
        {


            for (int i = 0; i < this.groupElements.Count; i++)
            {

                GuiElement currentElement = this.groupElements[i];
                int y = 0;

                if (i != 0)
                    y += Math.Max(1, this.groupElements[i - 1].Height * i);

                currentElement.SetPosition(this.position.x, this.position.y + y);

                if (this.shouldUpdate)
                {

                    currentElement.Update();

                    //is hovering over
                    if (GuiElement.IsInsideOf(InputController.GetMousePosition(), currentElement))
                    {

                        currentElement.isHovering = true;
                        currentElement.OnHover();
                    }
                    else
                    {
                        currentElement.isHovering = false;
                    }
                }
            }

            base.Update();
        }

        public override void Draw(int x, int y, Camera camera = null, Window window = null)
        {

            if (this.shouldDraw)
                for (int i = 0; i < this.groupElements.Count; i++)
                    this.groupElements[i].Draw(window.position.x + this.groupElements[i].position.x, window.position.y + this.groupElements[i].position.y, camera, window);


            base.Draw(x, y, camera, window);
        }
    }
}
