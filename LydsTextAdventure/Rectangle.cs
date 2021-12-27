namespace LydsTextAdventure
{
    public class Rectangle
    {

        public int Height
        {
            get;
        }
        public int Width
        {
            get;
        }

        public int StartX
        {
            get;
        }


        public int StartY
        {
            get;
        }


        public Rectangle(int w, int h, int startx = 0, int starty = 0)
        {

            this.Height = h;
            this.Width = w;

            this.StartX = startx;
            this.StartY = starty;
        }


        public bool IsInsideRectangle(int x, int y)
        {

            return (x >= StartX && x <= StartX + Width && y >= StartY && y <= StartY + Height);
        }
    }
}
