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

            Rectangle rect = (window == null ? this.GetRectangle() : window.GetRectangle());
            int _y = 0;
            for (int i = 0; i < strings.Count; i++)
            {

                try
                {
                    string str = this.strings[i];
                    if (str != null && _y < this.Height)
                        Surface.DrawText(x, y + _y++, str, rect);
                }
                catch
                {
                    Program.DebugLog("Error Drawing GUI Multiline");
                }

            }

            base.Draw(x, y, camera, window);
        }
    }
}
