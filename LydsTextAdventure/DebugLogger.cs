using System.IO;
using System.IO.Pipes;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace LydsTextAdventure
{

    class DebugLogger
    {

        public string[] DebugLog;
        public int StackPosition = 0;

        private NamedPipeClientStream pipe;

        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);

        public delegate bool EventHandler(CtrlType sig);
        private static EventHandler Handler;

        public string lastMessage = "";

        public enum CtrlType
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }

        private static bool OnExit(DebugLogger.CtrlType sig)
        {

#if DEBUG
            Program.DebugLogger.WriteShutdown();
#endif

            return true;
        }

        public static void CreateExitEvent()
        {

            DebugLogger.Handler += new EventHandler(OnExit);
            SetConsoleCtrlHandler(DebugLogger.Handler, true);
        }


        public void ConnectDebugLogger()
        {

            this.pipe = new NamedPipeClientStream(".", "console_log", PipeDirection.InOut, PipeOptions.None, TokenImpersonationLevel.Impersonation);
            this.pipe.Connect(20);
        }

        public void WriteLine(string msg)
        {

            if (this.pipe is null)
                return;

            if (!this.pipe.IsConnected)
                return;

            try
            {

        
                if (this.lastMessage == null)
                    this.lastMessage = msg;
                else
                    if (this.lastMessage == msg)
                    return;

                ServerData data = new ServerData
                {
                    message = msg,
                    flag = 0
                };

                StreamString ss = new StreamString(this.pipe);
                ss.WriteString(ServerData.Serialize(data));
            }
            catch (IOException)
            {

                return;
            }
        }

        public void WriteShutdown()
        {

            if (this.pipe is null)
                return;

            if (!this.pipe.IsConnected)
                return;

            try
            {

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
