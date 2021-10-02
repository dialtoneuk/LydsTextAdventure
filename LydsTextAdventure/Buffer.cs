using System;
using System.Collections.Generic;
using System.Text;

namespace LydsTextAdventure
{
    public class Buffer
    {

        public enum Types
        {

            DRAW_BUFFER,
            ENTITY_BUFFER,
            GUI_BUFFER,
            WORLD_BUFFER
        }

        private static int cursorTop;
        private static int cursorLeft;

        private static int cursorSavedLeft;
        private static int cursorSavedTop;

        private static int Width;
        private static int Height;

        private static char[,] drawBuffer;
        private static char[,] processBuffer;
        private static char[,] entityBuffer;
        private static char[,] guiBuffer;
        private static char[,] worldBuffer;

        public static int WindowWidth { get => Buffer.Width;}
        public static int WindowHeight { get => Buffer.Height;}

        public static void Create(int width, int height)
        {

            Buffer.drawBuffer = new char[width, height];
            Buffer.processBuffer = new char[width, height];
            Buffer.entityBuffer = new char[width, height];
            Buffer.guiBuffer = new char[width, height];
            Buffer.worldBuffer = new char[width, height];

            Buffer.Width = width;
            Buffer.Height = height;
        }

        public static char ReadBuffer(int x, int y, Buffer.Types type)
        {

            return Buffer.GetBuffer(type)[x, y];
        }

        private static char[,] GetBuffer(Buffer.Types type)
        {

            switch (type)
            {

                case Buffer.Types.GUI_BUFFER:
                    return Buffer.guiBuffer;
                case Buffer.Types.ENTITY_BUFFER:
                    return Buffer.entityBuffer;
                case Buffer.Types.DRAW_BUFFER:
                    return Buffer.drawBuffer;
                case Buffer.Types.WORLD_BUFFER:
                    return Buffer.worldBuffer;
            }

            throw new ApplicationException("invalid buffer");
        }

        public static char[] ReadLine(int index, Buffer.Types type)
        {

            char[,] buffer = Buffer.GetBuffer(type);
       
            for (int y = 0; y < Buffer.Height; y++)
            {

                if (y != index)
                    continue;

                char[] line = new char[Buffer.Width];

                for(int x = 0; x < Buffer.Width; x++){
                    line[x] = buffer[x, y];
                }

                return line;
            }

            return new char[Buffer.Height];
        }

        public static void Clear()
        {

            Buffer.entityBuffer = new char[Buffer.Width, Buffer.Height];
            Buffer.guiBuffer = new char[Buffer.Width, Buffer.Height];
            Buffer.worldBuffer = new char[Buffer.Width, Buffer.Height];
        }

        public static int CursorTop()
        {

            return Buffer.cursorTop;
        }

        public static int CursorLeft()
        {

            return Buffer.cursorLeft;
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

        public static void SetLastPosition(bool keep=false)
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

            Buffer.Clear();

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

        public static void AddToBuffer(Buffer.Types type, char[,] data, int startx=0, int starty=0)
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

        public static void GenerateBuffer()
        {

            //clean process buffer
            for (int y = 0; y < Buffer.Height; y++)
            {
                for (int x = 0; x < Buffer.Width; x++)
                {

                    Buffer.processBuffer[x, y] = ' ';
                }
            }

            //do the world 
            for (int x = 0; x < Buffer.Width; x++)
            {

                for (int y = 0; y < Buffer.Height; y++)
                {

                    if (Buffer.worldBuffer[x, y] != '\0')
                        Buffer.processBuffer[x, y] = Buffer.worldBuffer[x, y];
                }
            }

            //do any entities
            for (int x = 0; x < Buffer.Width; x++)
            {

                for (int y = 0; y < Buffer.Height; y++)
                {

                    if (Buffer.entityBuffer[x, y] != '\0')
                        Buffer.processBuffer[x, y] = Buffer.entityBuffer[x, y];
                }
            }

            //do any gui elements
            for (int x = 0; x < Buffer.Width; x++)
            {

                for (int y = 0; y < Buffer.Height; y++)
                {

                    if (Buffer.guiBuffer[x, y] != '\0')
                        Buffer.processBuffer[x, y] = Buffer.guiBuffer[x, y];
                }
            }

            //add it to the draw buffer
            for (int x = 0; x < Buffer.Width; x++)
            {

                for (int y = 0; y < Buffer.Height; y++)
                {

                    Buffer.drawBuffer[x, y] = Buffer.processBuffer[x, y];
                }
            }
        }

        public static char[,] EntityBuffer()
        {

            return Buffer.entityBuffer;
        }
        public static char[,] GuiBuffer()
        {

            return Buffer.guiBuffer;
        }

        private static void WriteToBuffer(char[] input, Buffer.Types type, bool newline = false, bool returnCursor = false)
        {

            if (returnCursor)
                Buffer.SaveLastPosition();

            int x = Buffer.cursorLeft;

            foreach(char c in input){
                Buffer.GetBuffer(type)[Buffer.cursorLeft, Buffer.cursorTop] = c;

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

            if(y >= 0)
                Buffer.cursorTop = Math.Max(0, Math.Min(Buffer.Height - 1, y));
        }
    }
}
