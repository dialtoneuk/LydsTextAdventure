using System;
using System.IO;
using System.IO.Pipes;
using System.Security.Principal;
using System.Threading;

namespace ConsoleLogger
{
    class Program
    {

        private static NamedPipeServerStream serverStream;
        private static bool running = true;

        static void Main(string[] args)
        {

            Program.serverStream = new NamedPipeServerStream("console_log", PipeDirection.InOut);

            Console.WriteLine(" SERVER WAITING FOR CONNECTION ");
            Program.serverStream.WaitForConnection();

            try
            {
                // Read the request from the client. Once the client has
                // written to the pipe its security token will be available.
                string line;
                while ( Program.running )
                {

                    StreamString ss = new StreamString(Program.serverStream);
                    line = ss.ReadString();

                    if (line.Length is 0)
                        Environment.Exit(0);
                    else
                        Console.WriteLine(line);
                }
            }
            // Catch the IOException that is raised if the pipe is broken
            // or disconnected.
            catch (IOException e )
            {

                Console.WriteLine("ERROR: {0}", e.Message);
            }

            Program.serverStream.Close();

            while(Console.ReadKey().Key != ConsoleKey.Escape )
            {};
        }
    }
}
