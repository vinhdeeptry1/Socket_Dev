using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThucThiVaGiaiThichTieuTrinh
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //MyThreadClass mtc1 = new MyThreadClass("Day la tieu trinh thu 1: ");
            //Thread thread1 = new Thread(new ThreadStart(mtc1.runMyThread));
            //thread1.Start();

            //MyThreadClass mtc2 = new MyThreadClass("Day la tieu trinh thu 2: ");
            //Thread thread2 = new Thread(new ThreadStart(mtc2.runMyThread));
            //thread2.Start();

            //Console.ReadKey();

            if (args.Length != 1 || string.IsNullOrWhiteSpace(args[0]))
                throw new ArgumentException("Loi roi, khong chay duoc dau");
            int serverPort = Int32.Parse(args[0]);
            TcpListener listener = new TcpListener(System.Net.IPAddress.Any, serverPort);
            ILogger logger = new ConsoleLogger();
            listener.Start();

            for (; ; )
            {
                try
                {
                    Socket client = listener.AcceptSocket();
                    EchoProtocol protocol = new EchoProtocol(client, logger);
                    Thread thread = new Thread(new ThreadStart(protocol.handleclient));
                    thread.Start();
                    logger.writeEntry("Created and started Thread: " + thread.GetHashCode());
                }
                catch (IOException e)
                {
                    logger.writeEntry("Error: " + e.Message);
                }
            }
        }
    }
}
