using System.IO;
using System.IO.Pipes;
using System.Security.Principal;

namespace LydsTextAdventure
{

    class ConsoleLogger
    {

        private readonly NamedPipeClientStream pipe;


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

                ServerData data = new ServerData
                {
                    message = msg
                };

                StreamString ss = new StreamString(this.pipe);
                ss.WriteString(ServerData.Serialize(data));
            }
            catch(IOException)
            {

                return;
            }
        }
    }
}
