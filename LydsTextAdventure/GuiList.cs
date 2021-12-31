using System;
using System.Collections.Generic;
using System.Text;

namespace LydsTextAdventure
{
    //like a group but will 
    class GuiList : GuiGroup
    {

        public bool isSubtractive = false;

        public override void Update()
        {

            int lastLargestWidth = 0;
            int count = 0;
            int maxCount;
            int x = this.position.x;
            int y = this.position.y;

            for (int i = 0; i < this.groupElements.Count; i++)
            {

                //height of the first element
                maxCount = (int)Math.Floor((float)(this.Height / (this.groupElements[0].Height)));
                GuiElement currentElement = this.groupElements[i];

                if (i > 0)
                    y = Math.Max(1, this.position.y + this.groupElements[i - 1].Height * count);

                if (count > maxCount)
                {
                    y = this.position.y;

                    if (isSubtractive)
                        x -= lastLargestWidth;
                    else
                        x += lastLargestWidth;

                    count = 0;
                }

                currentElement.SetPosition(x, y);

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

                if (currentElement.Width > lastLargestWidth)
                    lastLargestWidth = currentElement.Width;

                count++;
            }
        }
    }
}
