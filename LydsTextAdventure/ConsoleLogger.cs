using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Security.Principal;
using System.Text;

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


            if (this.pipe is null)
                return;

            if (!this.pipe.IsConnected)
                return;

            var ss = new StreamString(this.pipe);
            ss.WriteString(msg);
        }
    }
}
