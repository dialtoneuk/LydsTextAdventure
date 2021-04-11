using System;
using System.IO;
using System.IO.Pipes;

namespace ConsoleLogger
{
    class Program
    {

        private static NamedPipeServerStream serverStream;
        private static bool running = true;

        static void Main(string[] args)
        {

            Program.serverStream = new NamedPipeServerStream("console_log", PipeDirection.InOut, 1);

            Console.WriteLine(" SERVER WAITING FOR CONNECTION ");

            Program.serverStream.WaitForConnection();

            try
            {
                // Read the request from the client. Once the client has
                // written to the pipe its security token will be available.
                while (Program.running)
                {

                    var ss = new StreamString(Program.serverStream);
                    string xmlReceive = ss.ReadString();

                    if (xmlReceive.Length.Equals(0))
                        Environment.Exit(0);
                    else
                    {

                        ServerData data;
                        try
                        {
                            data = ServerData.Deserialize(xmlReceive);
                        }
                        catch(InvalidOperationException)
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

                        Console.WriteLine(data.message);
                    }

                }
            }
            // Catch the IOException that is raised if the pipe is broken
            // or disconnected.
            catch (IOException e )
            {

                Console.WriteLine("ERROR: {0}", e.Message);
            }

            Console.WriteLine("ended");
            Program.serverStream.Close();

            while(Console.ReadKey().Key != ConsoleKey.Escape )
            {};
        }
    }
}
