using System.IO;
using System.IO.Pipes;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace LydsTextAdventure
{

    class ConsoleLogger
    {

        private readonly NamedPipeClientStream pipe;

        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);

        public delegate bool EventHandler(CtrlType sig);
        private static EventHandler _handler;

        public string lastMessage = "";

        public enum CtrlType
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }

        private static bool OnExit(ConsoleLogger.CtrlType sig)
        {

#if DEBUG
            Program.DebugLogger.WriteShutdown();
#endif

            return true;
        }

        public static void CreateExitEvent()
        {

            ConsoleLogger._handler += new EventHandler(OnExit);
            SetConsoleCtrlHandler(ConsoleLogger._handler, true);
        }


        public ConsoleLogger()
        {

            this.pipe = new NamedPipeClientStream(".", "console_log", PipeDirection.InOut, PipeOptions.None, TokenImpersonationLevel.Impersonation);
        }


        public void Start()
        { 

            this.pipe.Connect();
        }

        public void WriteLine(string msg)
        {

            try
            {


                if (this.pipe is null)
                    return;

                if (!this.pipe.IsConnected)
                    this.pipe.Connect();

                if (lastMessage == null)
                    lastMessage = msg;
                else
                    if (lastMessage == msg)
                    return;

                ServerData data = new ServerData
                {
                    message = msg,
                    flag = 0
                };

                StreamString ss = new StreamString(this.pipe);
                ss.WriteString(ServerData.Serialize(data));
            }
            catch(IOException)
            {

                return;
            }
        }

        public void WriteShutdown()
        {

            try
            {


                if (this.pipe is null)
                    return;

                if (!this.pipe.IsConnected)
                    throw new System.Exception("invalid");

                ServerData data = new ServerData
                {
                    message = "shutdown",
                    flag = 1
                };

                StreamString ss = new StreamString(this.pipe);
                ss.WriteString(ServerData.Serialize(data));
            }
            catch (IOException)
            {

                return;
            }
        }
    }
}
