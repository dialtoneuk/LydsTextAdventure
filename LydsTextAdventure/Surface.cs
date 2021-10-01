using System;
using System.Collections.Generic;
using System.Text;

namespace LydsTextAdventure
{
    public class Surface
    {

        private static Camera currentCameraContext;

        public static void SetCameraContext(ref Camera camera)
        {

            Surface.currentCameraContext = camera;
        }

        public static void WriteOver(int x, int y, string str)
        {

            Surface.WriteOver(x, y, str.ToCharArray());
        }
        public static void WriteOver(int x, int y, char[] chars)
        {

            int lastx = Console.CursorLeft;
            int lasty = Console.CursorTop;

            Console.SetCursorPosition(0, y);
            Console.Write(Surface.blankChars(Console.WindowWidth));

            Console.SetCursorPosition(x, y);
            Console.Write(chars);

            Console.SetCursorPosition(lastx, lasty);
        }

        public static Position GetCenter()
        {

            return new Position(Console.WindowHeight / 2, Console.WindowWidth / 2);
        }

        public static void Write(int x, int y, string str)
        {

            Surface.Write(x, y, str.ToCharArray());
        }

        public static void Write(int x, int y, char[] chars)
        {

            int lastx = Console.CursorLeft;
            int lasty = Console.CursorTop;

            Console.SetCursorPosition(x, y);
            Console.Write(chars);

            Console.SetCursorPosition(lastx, lasty);
        }

        public static void WriteSurface(int x, int y, string str)
        {

            if (Surface.currentCameraContext == null)
                throw new ApplicationException("need to call SetCameraContext");

            char[] chars = str.ToCharArray();
            char[] dchars;
   
            int lastx = Console.CursorLeft;
            int lasty = Console.CursorTop;

            if (y > Surface.currentCameraContext.height - 1)
                return;

            Console.SetCursorPosition(x, y);

            if (x + chars.Length > Surface.currentCameraContext.width)
            {

                int a = (x + chars.Length) - Surface.currentCameraContext.width;
                dchars = new char[chars.Length - a];
                Array.Copy(chars, dchars, chars.Length - a);
                Console.Write(dchars);
            } 
            else
                Console.Write(chars);
                     
            Console.SetCursorPosition(lastx, lasty);
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
