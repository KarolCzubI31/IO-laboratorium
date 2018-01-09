using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IOLab1
{
    class Program
    {
        public static Object thisLock = new Object();
        static void Main(string[] args)
        {
            ThreadPool.QueueUserWorkItem(ThreadProc, new object[] { "s" });
            ThreadPool.QueueUserWorkItem(ThreadProc, new object[] { "c", 1 });
            ThreadPool.QueueUserWorkItem(ThreadProc, new object[] { "c", 2 });
            ThreadPool.QueueUserWorkItem(ThreadProc, new object[] { "c", 3 });
            ThreadPool.QueueUserWorkItem(ThreadProc, new object[] { "c", 4 });
            ThreadPool.QueueUserWorkItem(ThreadProc, new object[] { "c", 5 });
            Thread.Sleep(8000);
        }

        static void ThreadProc(Object stateInfo)
        {
            var mess = ((object[])stateInfo)[0];
            String code = (String)mess;
            if (code.Equals("s"))
            {
                TcpListener server = new TcpListener(IPAddress.Any, 2048);
                server.Start();
                while (true)
                {
                    TcpClient clientServ = server.AcceptTcpClient();
                    ThreadPool.QueueUserWorkItem(ThreadProcSub, new object[] { clientServ });
                }
            }
            else if (code.Equals("c"))
            {
                var nr = ((object[])stateInfo)[1];
                TcpClient client = new TcpClient();
                client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2048));
                byte[] message = new ASCIIEncoding().GetBytes("wiadomosc" + nr.ToString());
                client.GetStream().Write(message, 0, message.Length);
                byte[] buffer = new byte[1024];
                int a = client.GetStream().Read(buffer, 0, 1024);
                String messageRec = new ASCIIEncoding().GetString(buffer, 0, a);
                lock (thisLock)
                {
                    writeConsoleMessage("c Otrzymalem wiadomosc " + messageRec, ConsoleColor.Green);
                }
            }
        }

        static void ThreadProcSub(Object stateInfo)
        {
            TcpClient clientServ = (TcpClient)((object[])stateInfo)[0];
            byte[] buffer = new byte[1024];
            int a = clientServ.GetStream().Read(buffer, 0, 1024);
            String message = new ASCIIEncoding().GetString(buffer, 0, a);
            lock (thisLock)
            {
                writeConsoleMessage("s Otrzymalem wiadomosc " + message, ConsoleColor.Red);
            }
            clientServ.GetStream().Write(buffer, 0, a);
            clientServ.Close();
        }

        static void writeConsoleMessage(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}