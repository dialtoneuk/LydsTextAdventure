using System;

namespace LydsTextAdventure
{
    public class Surface
    {

        public static void WriteOver(int x, int y, string str)
        {

            Surface.WriteOver(x, y, str.ToCharArray());
        }
        public static void WriteOver(int x, int y, char[] chars)
        {

            Buffer.SaveLastPosition();

            Buffer.SetCursorPosition(0, y);
            Buffer.Write(Surface.BlankChars(Buffer.WindowWidth), Buffer.Types.GUI_BUFFER);

            Buffer.SetCursorPosition(Math.Max(0, x), Math.Max(0, y));
            Buffer.Write(chars, Buffer.Types.GUI_BUFFER);

            Buffer.SetLastPosition();
        }

        public static Position GetCenter()
        {

            return new Position(Buffer.WindowHeight / 2, Buffer.WindowWidth / 2);
        }

        public static void Write(int x, int y, string str)
        {

            Surface.Write(x, y, str.ToCharArray());
        }

        public static void Write(int x, int y, char[] chars)
        {

            Buffer.SaveLastPosition();
            Buffer.SetCursorPosition(Math.Max(0, x), Math.Max(0, y));
            Buffer.Write(chars, Buffer.Types.GUI_BUFFER);
            Buffer.SetLastPosition();
        }

        public static void DrawText(int x, int y, string str, Rectangle rectangle)
        {


            char[] chars = str.ToCharArray();
            char[] dchars;

            Buffer.SetCursorPosition(x, y);

            if (x + chars.Length > rectangle.Width)
            {

                int a = (x + chars.Length) - (rectangle.Width);

                if (a >= chars.Length)
                    a = 0;

                dchars = new char[chars.Length - a];
                Array.Copy(chars, dchars, chars.Length - a);
                Buffer.Write(dchars, Buffer.Types.GUI_BUFFER);
            }
            else
                Buffer.Write(chars, Buffer.Types.GUI_BUFFER);
        }

        public static void DrawBox(int x, int y, int w, int h, Rectangle rectangle = null, Buffer.Types type = Buffer.Types.GUI_BUFFER)
        {

            int rw = Buffer.WindowWidth;
            int rh = Buffer.WindowHeight;

            if (rectangle != null)
            {
                rw = rectangle.Width;
                rh = rectangle.Height;
            }

            Buffer.SetCursorPosition(x, y);

            for (int ix = 0; ix < w; ix++)
            {

                if (x + ix > rw)
                    continue;

                for (int iy = 0; iy < h; iy++)
                {

                    Buffer.SetCursorPosition(x + ix, y + iy);

                    if (y + iy > rh)
                        continue;

                    if (ix == 0 || ix == w - 1)
                        Buffer.Write("|", type);
                    else if (iy == 0 || iy == h - 1)
                        Buffer.Write("-", type);
                    else
                        Buffer.Write(' ', type);
                }
            }
        }

        public static char[] BlankChars(int length)
        {
            char[] buffer = new char[length];

            for (int i = 0; i < length; i++)
            {
                buffer[i] = ' ';
            }

            return buffer;
        }
    }
}
