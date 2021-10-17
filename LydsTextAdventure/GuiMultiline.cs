using System.Collections.Generic;

namespace LydsTextAdventure
{
    public class GuiMultiline : GuiElement
    {

        protected List<string> strings = new List<string>();

        public GuiMultiline()
        {

            //Default size
            this.SetSize(32, 32);
        }

        public void AddText(string str)
        {

            this.strings.Add(str);
        }

        public void Clear()
        {

            this.strings.Clear();
        }

        public override void Draw(int x, int y, Camera camera = null, Window window = null)
        {

            int _y = 0;
            foreach (string str in this.strings)
                if (str != null && _y < this.Height)
                    Surface.DrawText(x, y + _y++, str, this.GetRectangle());

            base.Draw(x, y, camera, window);
        }
    }
}
