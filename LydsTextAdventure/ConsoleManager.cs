using Microsoft.Win32.SafeHandles;
using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;

namespace LydsTextAdventure
{


    /*
     * This is where a collection of console related functions are living, these basically
     * interact directly with Windows to do the stuff we want to do
     */
    public class ConsoleManager
    {

        const uint ENABLE_QUICK_EDIT = 0x0040;
        const int STD_INPUT_HANDLE = -10;

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern SafeFileHandle CreateFile(
        string fileName,
        [MarshalAs(UnmanagedType.U4)] uint fileAccess,
        [MarshalAs(UnmanagedType.U4)] uint fileShare,
        IntPtr securityAttributes,
        [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
        [MarshalAs(UnmanagedType.U4)] int flags,
        IntPtr template);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool WriteConsoleOutputW(
          SafeFileHandle hConsoleOutput,
          CharInfo[] lpBuffer,
          Coord dwBufferSize,
          Coord dwBufferCoord,
          ref SmallRect lpWriteRegion);

        [StructLayout(LayoutKind.Sequential)]
        public struct Coord
        {
            public short X;
            public short Y;

            public Coord(short X, short Y)
            {
                this.X = X;
                this.Y = Y;
            }
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct SmallRect
        {
            public short Left;
            public short Top;
            public short Right;
            public short Bottom;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct CharUnion
        {
            [FieldOffset(0)] public ushort UnicodeChar;
            [FieldOffset(0)] public byte AsciiChar;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct CharInfo
        {
            [FieldOffset(0)] public CharUnion Char;
            [FieldOffset(2)] public short Attributes;
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(IntPtr hwnd, out Rectangle lpRect);

        [DllImport("kernel32.dll")]
        public static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

        [DllImport("kernel32.dll")]
        static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();

        //sets the console window handle
        private static readonly IntPtr ConsoleWindow = GetConsoleWindow();

        [StructLayout(LayoutKind.Sequential)]
        public struct Rectangle
        {
            public int Left;        // x position of upper-left corner
            public int Top;         // y position of upper-left corner
            public int Right;       // x position of lower-right corner
            public int Bottom;      // y position of lower-right corner
        }

        //gets the correct x/y relative to the tile/char
        public static Position GetMousePosition(int fontWidth = 10, int fontSize = 16)
        {

            //first get the cursor position
            Point point = new Point();
            ConsoleManager.GetCursorPos(ref point);

            //then get the consoles
            Position console = ConsoleManager.GetConsolePosition();
            //cursors
            Position cursor = new Position(point.X, point.Y);
            return new Position((int)Math.Floor((decimal)((cursor.x - console.x) / fontWidth)), (int)Math.Floor((decimal)((cursor.y - (console.y + 2)) / fontSize) - 1));
        }

        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(ref Point lpPoint);

        //this disables the consoles select feature which basically pauses our game for no reason
        public static void DisableQuickEdit()
        {

            //need to get the std input handle instead of the consoles window handle
            IntPtr consoleHandle = GetStdHandle(STD_INPUT_HANDLE);

            // get current console mode
            uint consoleMode;
            if (!GetConsoleMode(consoleHandle, out consoleMode))
            {
                throw new ApplicationException("invalid");
            }

            // Clear the quick edit bit in the mode flags
            consoleMode &= ~ENABLE_QUICK_EDIT;

            // set the new mode
            if (!SetConsoleMode(consoleHandle, consoleMode))
            {
                // ERROR: Unable to set console mode
                throw new ApplicationException("invalid");
            }
        }

        public static IntPtr GetWindowHandle()
        {

            return ConsoleManager.ConsoleWindow;
        }

        public static Position GetConsolePosition()
        {

            Rectangle rectangle;
            ConsoleManager.GetWindowRect(ConsoleManager.ConsoleWindow, out rectangle);
            return new Position(rectangle.Left, rectangle.Top);
        }
    }
}
