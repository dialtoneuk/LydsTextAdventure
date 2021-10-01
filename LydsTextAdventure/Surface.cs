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

        public static void EndCameraContext()
        {

            Surface.currentCameraContext = null;
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

            Console.SetCursorPosition(Math.Max(0, x), Math.Max(0, y));
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

            Console.SetCursorPosition(Math.Max(0, x), Math.Max(0, y));
            Console.Write(chars);

            Console.SetCursorPosition(lastx, lasty);
        }

        //requires current camera context
        public static void DrawText(int x, int y, string str)
        {

            if (Surface.currentCameraContext == null)
                throw new ApplicationException("need to call SetCameraContext");

            char[] chars = str.ToCharArray();
            char[] dchars;
   
            int lastx = Console.CursorLeft;
            int lasty = Console.CursorTop;
            int padding = 3;

            if (Surface.currentCameraContext.IsDrawingTitle())
                padding++;

            if (y > Surface.currentCameraContext.height)
                return;

            Console.SetCursorPosition(Math.Max(0, x), Math.Max(padding, y));

            if (x + chars.Length > Surface.currentCameraContext.width)
            {

                int a = (x + chars.Length) - ( Surface.currentCameraContext.width + 1 );

                if (a >= chars.Length)
                    a--;

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
