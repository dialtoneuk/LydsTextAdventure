using Microsoft.Win32.SafeHandles;
using System;
using System.Globalization;
using System.IO;
using static LydsTextAdventure.ConsoleManager;

namespace LydsTextAdventure
{
    public class Buffer
    {

        public enum Types
        {

            DRAW_BUFFER,
            ENTITY_BUFFER,
            GUI_BUFFER,
            WORLD_BUFFER,
            COLOUR_BUFFER,
            TOP_BUFFER
        }

        public static bool isReady = false;

        private static int cursorTop;
        private static int cursorLeft;
        private static SafeFileHandle handle;

        private static int cursorSavedLeft;
        private static int cursorSavedTop;

        private static int Width;
        private static int Height;

        private static char[,] drawBuffer;
        private static char[,] processBuffer;
        private static char[,] entityBuffer;
        private static char[,] guiBuffer;
        private static char[,] worldBuffer;
        private static char[,] topBuffer;
        private static ConsoleColor[,] colourBuffer;

        private static Hook fpsHook;

        private static int lastSecond = 0;

        public static int DrawnFrames
        {
            get;
            private set;
        } = 0;

        public static int FPS
        {
            get;
            private set;
        } = 0;

        public static int WindowWidth
        {
            get => Buffer.Width;
        }
        public static int WindowHeight
        {
            get => Buffer.Height;
        }

        public static void CreateHook()
        {

            int lastDrawnFrames = Buffer.DrawnFrames;
            if (fpsHook != null)
                throw new ApplicationException("fpsHook already initialized");

            //add a hook to basically update the FPS
            fpsHook = new Hook("ClockTick", HookManager.Groups.Global, (object[] obj) =>
            {
                Buffer.FPS = Buffer.DrawnFrames - lastDrawnFrames;
                lastDrawnFrames = Buffer.DrawnFrames;
            });
        }

        public static void Create(int width, int height)
        {

            Buffer.drawBuffer = new char[width, height];
            Buffer.processBuffer = new char[width, height];
            Buffer.entityBuffer = new char[width, height];
            Buffer.guiBuffer = new char[width, height];
            Buffer.worldBuffer = new char[width, height];
            Buffer.topBuffer = new char[width, height];
            Buffer.colourBuffer = new ConsoleColor[width, height];
            Buffer.handle = ConsoleManager.CreateFile("CONOUT$", 0x40000000, 2, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);
            Buffer.Width = width;
            Buffer.Height = height;
        }


        public static char ReadBuffer(int x, int y, Buffer.Types type)
        {

            return Buffer.GetBuffer(type)[x, y];
        }

        private static char[,] GetBuffer(Buffer.Types type)
        {

            return type switch
            {
                Buffer.Types.GUI_BUFFER => Buffer.guiBuffer,
                Buffer.Types.ENTITY_BUFFER => Buffer.entityBuffer,
                Buffer.Types.DRAW_BUFFER => Buffer.drawBuffer,
                Buffer.Types.WORLD_BUFFER => Buffer.worldBuffer,
                Buffer.Types.TOP_BUFFER => Buffer.topBuffer,
                _ => throw new ApplicationException("invalid buffer"),
            };
        }

        public static char[] ReadLine(int index, Buffer.Types type)
        {

            char[,] buffer = Buffer.GetBuffer(type);

            for (int y = 0; y < Buffer.Height; y++)
            {

                if (y != index)
                    continue;

                char[] line = new char[Buffer.Width];

                for (int x = 0; x < Buffer.Width; x++)
                {
                    line[x] = buffer[x, y];
                }

                return line;
            }

            return new char[Buffer.Height];
        }

        public static void Reset()
        {

            Buffer.entityBuffer = new char[Buffer.Width, Buffer.Height];
            Buffer.guiBuffer = new char[Buffer.Width, Buffer.Height];
            Buffer.worldBuffer = new char[Buffer.Width, Buffer.Height];
            Buffer.topBuffer = new char[Buffer.Width, Buffer.Height];
            Buffer.colourBuffer = new ConsoleColor[Buffer.Width, Buffer.Height];
        }

        public static int CursorTop()
        {

            return Buffer.cursorTop;
        }

        public static int CursorLeft()
        {

            return Buffer.cursorLeft;
        }

        public static void Write(char[] str, Buffer.Types type = Buffer.Types.ENTITY_BUFFER, ConsoleColor colour = ConsoleColor.White)
        {

            Buffer.WriteToBuffer(str, type, false, false, colour);
        }

        public static void Write(char str, Buffer.Types type = Buffer.Types.ENTITY_BUFFER, ConsoleColor colour = ConsoleColor.White)
        {

            Buffer.WriteToBuffer(new char[] { str }, type, false, false, colour);
        }

        public static void WriteLine(string str, Buffer.Types type = Buffer.Types.ENTITY_BUFFER, ConsoleColor colour = ConsoleColor.White)
        {

            Buffer.WriteToBuffer(str.ToCharArray(), type, false, false, colour);
        }

        public static void Write(char str, Buffer.Types type = Buffer.Types.ENTITY_BUFFER)
        {

            Buffer.Write(str.ToString(), type);
        }

        public static void Write(string str, Buffer.Types type = Buffer.Types.ENTITY_BUFFER)
        {

            Buffer.Write(str.ToCharArray(), type);
        }

        public static void Write(char[] str, Buffer.Types type = Buffer.Types.ENTITY_BUFFER)
        {

            Buffer.WriteToBuffer(str, type, false, true);
        }


        public static void WriteLine(char[] str, Buffer.Types type = Buffer.Types.ENTITY_BUFFER)
        {

            Buffer.WriteToBuffer(str, type, true, false);
        }

        public static void WriteLine(string str, Buffer.Types type = Buffer.Types.ENTITY_BUFFER)
        {

            Buffer.WriteLine(str.ToCharArray(), type);
        }

        public static void SaveLastPosition()
        {

            Buffer.cursorSavedTop = Buffer.cursorTop;
            Buffer.cursorSavedLeft = Buffer.cursorLeft;
        }

        public static void SetLastPosition(bool keep = false)
        {

            if (Buffer.cursorSavedLeft == -1 || Buffer.cursorSavedTop == -1)
                return;

            Buffer.cursorTop = Buffer.cursorSavedTop;
            Buffer.cursorLeft = Buffer.cursorSavedLeft;

            if (!keep)
            {
                Buffer.cursorSavedLeft = -1;
                Buffer.cursorSavedTop = -1;
            }
        }

        public static void CleanBuffer()
        {

            Buffer.Reset();

            for (int y = 0; y < Buffer.Height; y++)
            {
                for (int x = 0; x < Buffer.Width; x++)
                {

                    Buffer.drawBuffer[x, y] = '\0';
                }
            }

            for (int y = 0; y < Buffer.Height; y++)
            {
                for (int x = 0; x < Buffer.Width; x++)
                {

                    Buffer.processBuffer[x, y] = '\0';
                }
            }
        }

        public static void DrawBuffer()
        {

            if (lastSecond != Program.Clock)
            {
                lastSecond = Program.Clock;
            }

            //invalid
            if (Buffer.handle.IsInvalid)
                return;

            CharInfo[] buffer = new CharInfo[Buffer.Width * Buffer.Height];
            SmallRect rect = new SmallRect() { Left = 0, Top = 0, Right = (short)Buffer.Width, Bottom = (short)Buffer.Height };

            int len = 0;

            for (int y = 0; y < Buffer.Height; y++)
            {
                for (int x = 0; x < Buffer.Width; x++)
                {

                    buffer[len].Attributes = (short)(0 + (int)Buffer.colourBuffer[x, y]);
                    int code = (int)Buffer.drawBuffer[x, y];
                    buffer[len].Char.UnicodeChar = (ushort)code;
                    len++;
                }
            }

            if (!ConsoleManager.WriteConsoleOutputW(Buffer.handle, buffer,
                       new Coord() { X = (short)Buffer.Width, Y = (short)Buffer.Height },
                       new Coord() { X = 0, Y = 0 },
                       ref rect))
                return;

            //increment draws
            DrawnFrames++;
        }

        /**
        public static void DrawBuffer()
        {

            Console.SetCursorPosition(0, 0);


            for (int y = 0; y < Buffer.Height; y++)
            {

                char[] line = new char[Buffer.Width + 1];
                for (int x = 0; x < Buffer.Width; x++)
                {

                    line[x] = Buffer.drawBuffer[x, y];
                }

                line[Buffer.Width] = '\n';
                Console.Write(line);
            }
        }
        **/

        public static void AddToBuffer(Buffer.Types type, char[,] data, int startx = 0, int starty = 0)
        {

            for (int y = 0; y < data.GetLength(1); y++)
            {
                for (int x = 0; x < data.GetLength(0); x++)
                {

                    Buffer.SetCursorPosition(startx + x, starty + y);
                    Buffer.Write(data[x, y], type);
                }
            }
        }

        public static void AddToBuffer(Buffer.Types type, Camera.TempBuffer[,] data, int startx = 0, int starty = 0)
        {

            for (int y = 0; y < data.GetLength(1); y++)
            {
                for (int x = 0; x < data.GetLength(0); x++)
                {

                    Buffer.SetCursorPosition(startx + x, starty + y);
                    Buffer.Write(data[x, y].texture, type, data[x, y].colour);
                }
            }
        }

        public static void PrepareBuffer()
        {

            Buffer.isReady = false;

            //do the world 
            for (int x = 0; x < Buffer.Width; x++)
            {

                for (int y = 0; y < Buffer.Height; y++)
                {
                    Buffer.processBuffer[x, y] = ' ';

                    if (Buffer.worldBuffer[x, y] != '\0')
                        Buffer.processBuffer[x, y] = Buffer.worldBuffer[x, y];

                    if (Buffer.entityBuffer[x, y] != '\0')
                        Buffer.processBuffer[x, y] = Buffer.entityBuffer[x, y];

                    if (Buffer.topBuffer[x, y] != '\0')
                        Buffer.processBuffer[x, y] = Buffer.topBuffer[x, y];

                    if (Buffer.guiBuffer[x, y] != '\0')
                        Buffer.processBuffer[x, y] = Buffer.guiBuffer[x, y];
                    Buffer.drawBuffer[x, y] = Buffer.processBuffer[x, y];
                }
            }

            Buffer.isReady = true;
        }

        public static char[,] EntityBuffer()
        {

            return Buffer.entityBuffer;
        }
        public static char[,] GuiBuffer()
        {

            return Buffer.guiBuffer;
        }

        private static void WriteToBuffer(char[] input, Buffer.Types type, bool newline = false, bool returnCursor = false, ConsoleColor colour = ConsoleColor.White)
        {

            if (returnCursor)
                Buffer.SaveLastPosition();

            int x = Buffer.cursorLeft;

            foreach (char c in input)
            {
                Buffer.GetBuffer(type)[Buffer.cursorLeft, Buffer.cursorTop] = c;
                Buffer.colourBuffer[Buffer.cursorLeft, Buffer.cursorTop] = colour;

                if (Buffer.Width - 1 > Buffer.cursorLeft)
                    Buffer.cursorLeft++;
            }

            if (newline)
                Buffer.SetCursorPosition(x, Buffer.cursorTop + 1);
            else
                Buffer.SetCursorPosition(x, Buffer.cursorTop);

            if (returnCursor)
                Buffer.SetLastPosition();
        }

        public static void SetCursorPosition(int x, int y = -1)
        {

            Buffer.cursorLeft = Math.Max(0, Math.Min(Buffer.Width - 1, x));

            if (y >= 0)
                Buffer.cursorTop = Math.Max(0, Math.Min(Buffer.Height - 1, y));
        }
    }
}
