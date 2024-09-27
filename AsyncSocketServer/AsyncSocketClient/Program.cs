using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsyncSocketTCP;

namespace AsyncSocketClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            AsyncSocketTCPClient client = new AsyncSocketTCPClient();

            Console.WriteLine("*** Welcome to Async Socket CLient ***");
            Console.WriteLine("Enter Server IP Address:");

            string strIPAddress = Console.ReadLine();

            Console.WriteLine("Enter Port Number (0 - 65535): ");
            string strPortInput = Console.ReadLine();

            if(!client.SetServerIPAddress(strIPAddress) ||
                !client.SetPortNumber(strPortInput))
            {
                Console.WriteLine(
                    string.Format(
                        "IP Address or Port Number invalid - {0} - {1} - Press any key to exit",
                        strIPAddress, strPortInput));
                Console.ReadKey();
                return;
            }

            client.ConnectToServer();

            string strInputUser = null;

            do
            {
                strInputUser = Console.ReadLine();
                if (strInputUser.Trim() != "<EXIT>")
                {
                    client.SendToServer(strInputUser);
                }
                else if (strInputUser.Equals("<EXIT>"))
                {
                    client.CloseAndDisconnec();
                }
            } while (strInputUser != "<EXIT>");
        }
    }
}
