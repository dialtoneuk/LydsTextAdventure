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

        private static int width;
        private static int height;

        private static char[,] drawBuffer;
        private static char[,] processBuffer;
        private static char[,] entityBuffer;
        private static char[,] guiBuffer;
        private static char[,] worldBuffer;

        public static void Create(int width, int height)
        {

            Buffer.drawBuffer = new char[width, height];
            Buffer.processBuffer = new char[width, height];
            Buffer.entityBuffer = new char[width, height];
            Buffer.guiBuffer = new char[width, height];
            Buffer.worldBuffer = new char[width, height];

            Buffer.width = width;
            Buffer.height = height;
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
       
            for (int y = 0; y < Buffer.height; y++)
            {

                if (y != index)
                    continue;

                char[] line = new char[Buffer.width];

                for(int x = 0; x < Buffer.width; x++){
                    line[x] = buffer[x, y];
                }

                return line;
            }

            return new char[Buffer.height];
        }

        public static void Clear()
        {

            Buffer.entityBuffer = new char[Buffer.width, Buffer.height];
            Buffer.guiBuffer = new char[Buffer.width, Buffer.height];
            Buffer.worldBuffer = new char[Buffer.width, Buffer.height];
        }

        public static int WindowWidth()
        {

            return Buffer.width;
        }

        public static int WindowHeight()
        {

            return Buffer.height;
        }

        public static int CursorTop()
        {

            return Buffer.cursorTop;
        }

        public static int CursorLeft()
        {

            return Buffer.cursorLeft;
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

        private static void SaveLastPosition()
        {

            Buffer.cursorSavedTop = Buffer.cursorTop;
            Buffer.cursorSavedLeft = Buffer.cursorLeft;
        }

        private static void SetLastPosition(bool keep=false)
        {

            if (Buffer.cursorSavedLeft == -1 || Buffer.cursorSavedTop == -1)
                throw new ApplicationException("invalid saved position");

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

            for (int y = 0; y < Buffer.height; y++)
            {
                for (int x = 0; x < Buffer.width; x++)
                {

                    Buffer.drawBuffer[x, y] = '\0';
                }
            }

            for (int y = 0; y < Buffer.height; y++)
            {
                for (int x = 0; x < Buffer.width; x++)
                {

                    Buffer.processBuffer[x, y] = '\0';
                }
            }
        }

        public static void DrawBuffer()
        {

            Console.SetCursorPosition(0, 0);

            for (int y = 0; y < Buffer.height; y++)
            {

                char[] line = new char[Buffer.width + 1];
                for (int x = 0; x < Buffer.width; x++)
                {

                    line[x] = Buffer.drawBuffer[x, y];
                }

                line[Buffer.width] = '\n';
                Console.Write(line);
            }
        }

        public static void GenerateBuffer()
        {

            //clean process buffer
            for (int y = 0; y < Buffer.height; y++)
            {
                for (int x = 0; x < Buffer.width; x++)
                {

                    Buffer.processBuffer[x, y] = '\0';
                }
            }

            //do the world 
            for (int x = 0; x < Buffer.width; x++)
            {

                for (int y = 0; y < Buffer.height; y++)
                {

                    if (Buffer.worldBuffer[x, y] != '\0')
                        Buffer.processBuffer[x, y] = Buffer.worldBuffer[x, y];
                }
            }

            //do any entities
            for (int x = 0; x < Buffer.width; x++)
            {

                for (int y = 0; y < Buffer.height; y++)
                {

                    if (Buffer.entityBuffer[x, y] != '\0')
                        Buffer.processBuffer[x, y] = Buffer.entityBuffer[x, y];
                }
            }

            //do any gui elements
            for (int x = 0; x < Buffer.width; x++)
            {

                for (int y = 0; y < Buffer.height; y++)
                {

                    if (Buffer.guiBuffer[x, y] != '\0')
                        Buffer.processBuffer[x, y] = Buffer.guiBuffer[x, y];
                }
            }

            //add it to the draw buffer
            for (int x = 0; x < Buffer.width; x++)
            {

                for (int y = 0; y < Buffer.height; y++)
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

            int x = 0;

            if (newline)
                x = Buffer.cursorLeft;

            foreach(char c in input){
                Buffer.GetBuffer(type)[Buffer.cursorLeft, Buffer.cursorTop] = c;
                Buffer.SetCursorPosition(Buffer.cursorLeft + 1, Buffer.cursorTop);
            }

            if (newline)
                Buffer.SetCursorPosition(x, Buffer.cursorTop + 1);
            
            if (returnCursor)
                Buffer.SetLastPosition();
        }

        public static void SetCursorPosition(int x, int y = -1)
        {

            Buffer.cursorLeft = Math.Max(0, Math.Min(Buffer.width - 1, x));

            if(y >= 0)
                Buffer.cursorTop = Math.Max(0, Math.Min(Buffer.height - 1, y));
        }
    }
}
