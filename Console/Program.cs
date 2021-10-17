using System;
using System.IO;
using System.IO.Pipes;
using System.Threading.Tasks;

namespace ConsoleLogger
{
    class Program
    {

        private static NamedPipeServerStream ServerStream;
        private static bool Running = true;
        private static bool Connected = false;

        private static int MsgCount = 0;
        private static int LastCount = 0;

        static void Main(string[] args)
        {

            reconnect:
            Program.ServerStream = new NamedPipeServerStream("console_log", PipeDirection.InOut, 1);

            Console.WriteLine(" SERVER WAITING FOR CONNECTION ");
            Task.Delay(60000).ContinueWith((state) =>
            {

                if (!Connected)
                {

                    Console.WriteLine("failed to connect after 60 seconds.. shutting down");
                    System.Environment.Exit(1);
                }
            });

            Program.ServerStream.WaitForConnection();
            Connected = true;

            try
            {

                Program.CheckForRecentMessages();

                // Read the request from the client. Once the client has
                // written to the pipe its security token will be available.
                while (Program.Running)
                {

                    if (!Program.ServerStream.IsConnected)
                        break;

                    var ss = new StreamString(Program.ServerStream);
                    string xmlReceive = ss.ReadString();

                    if (xmlReceive.Length.Equals(0))
                        continue;
                    else
                    {

                        ServerData data;
                        try
                        {
                            data = ServerData.Deserialize(xmlReceive);
                        }
                        catch (InvalidOperationException)
                        {

                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("invalid operation exception!");
                            Console.ForegroundColor = ConsoleColor.White;
                            continue;
                        }

                        if (data is null)
                            continue;

                        if (data.GetType() != typeof(ServerData))
                            continue;

                        if (data.flag == 1)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Game shutdown!");
                            Console.ForegroundColor = ConsoleColor.White;
                            break;
                        }
                           
                        if (data.message.Contains("general"))
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                        else if (data.message.Contains("window"))
                            Console.ForegroundColor = ConsoleColor.Yellow;
                        else if (data.message.Contains("camera"))
                            Console.ForegroundColor = ConsoleColor.Blue;
                        else if (data.message.Contains("input"))
                            Console.ForegroundColor = ConsoleColor.DarkBlue;
                        else if (data.message.Contains("world"))
                            Console.ForegroundColor = ConsoleColor.Green;
                        else if (data.message.Contains("entity"))
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                        else if (data.message.Contains("scene"))
                            Console.ForegroundColor = ConsoleColor.DarkCyan;


                        Console.WriteLine(data.message);
                        Console.ResetColor();

                        MsgCount++;
                    }

                }
            }
            // Catch the IOException that is raised if the pipe is broken
            // or disconnected.
            catch (IOException e)
            {

                Console.WriteLine("ERROR: {0}", e.Message);
            }

            Console.WriteLine("Awaiting reconnect...");
            
            if(Program.ServerStream.IsConnected)
                Program.ServerStream.Close();

            goto reconnect;
        }

        public static void CheckForRecentMessages()
        {

            //check every 60 seconds
            Task.Delay(60000).ContinueWith((state) =>
            {

                if (MsgCount != 0 && MsgCount == LastCount)
                {

                    Console.WriteLine("no message in the last 60 seconds... Closing connection");

                    if (Program.ServerStream.IsConnected)
                        Program.ServerStream.Close();

                    return;
                }

                LastCount = MsgCount;
                Program.CheckForRecentMessages();
            });
        }
    }
}
