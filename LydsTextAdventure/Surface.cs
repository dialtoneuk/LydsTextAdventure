using System;
using System.Collections.Generic;
using System.Text;

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
            Buffer.Write(Surface.blankChars(Buffer.WindowWidth), Buffer.Types.GUI_BUFFER);

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
            Buffer.Write(chars);
            Buffer.SetLastPosition();
        }

        public static void DrawText(int x, int y, string str, Camera camera)
        {


            char[] chars = str.ToCharArray();
            char[] dchars;

            Buffer.SetCursorPosition(x, y);

            if (x + chars.Length > camera.width)
            {

                int a = (x + chars.Length) - ( camera.width );

                if (a >= chars.Length)
                    a = 0;

                dchars = new char[chars.Length - a];
                Array.Copy(chars, dchars, chars.Length - a);
                Buffer.Write(dchars, Buffer.Types.GUI_BUFFER);
            } 
            else
                Buffer.Write(chars, Buffer.Types.GUI_BUFFER);
        }

        public static char[] blankChars(int length)
        {
            char[] buffer = new char[length];

            for(int i = 0; i < length; i++)
            {
                buffer[i] = ' ';
            }

            return buffer;
        }
    }
}
