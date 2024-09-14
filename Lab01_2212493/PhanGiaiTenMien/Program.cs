using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net.NetworkInformation;

namespace PhanGiaiTenMien
{
    internal class Program
    {
        static void GetHostInfo(string host)
        {
            try
            {
                IPHostEntry hostInfo = Dns.GetHostEntry(host);
                //Display host name
                Console.WriteLine("Ten mien: " + hostInfo.HostName);
                //Display list of OP address
                //Console.WriteLine("Dia chi IP: ");
                foreach (IPAddress ipaddr in hostInfo.AddressList)
                {
                    Console.WriteLine("Dia chi IP: " + ipaddr.ToString() + " ");
                    Console.WriteLine("Subnet mask: " + GetSubnetMaskFromCIDR(ipaddr, 24));
                    Console.WriteLine("Default Gateway: " + GetDefaultGateway());
                }
                Console.WriteLine();
            }
            catch (Exception) 
            {
                Console.WriteLine("Khong phan giai duoc ten mien: " + host + "\n");
            }
        }
        //static string GetSubnetMask(IPAddress ipAddress)
        //{
        //    // Example: Assuming a default subnet mask for IPv4
        //    if (ipAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
        //    {
        //        return "255.255.255.0"; // Default subnet mask for IPv4
        //    }
        //    else
        //    {
        //        // Handle IPv6 or other cases if needed
        //        return "Not applicable";
        //    }
        //}
        static string GetSubnetMaskFromCIDR(IPAddress ipAddress, int cidr)
        {
            if (ipAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                uint mask = 0xffffffff << (32 - cidr);
                return new IPAddress(BitConverter.GetBytes(mask).Reverse().ToArray()).ToString();
            }
            else
            {
                // Handle IPv6 or other cases if needed
                return "Khong phan giai duoc:";
            }
        }
        static string GetDefaultGateway()
        {
            var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces()
                .Where(n => n.OperationalStatus == OperationalStatus.Up)
                .Where(n => n.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                .SelectMany(n => n.GetIPProperties()?.GatewayAddresses)
                .Select(g => g?.Address)
                .FirstOrDefault();

            return networkInterfaces?.ToString() ?? "Khong phan giai duoc";
        }
        static void Main(string[] args)
        {
            
            foreach (string arg in args) 
            {
                Console.WriteLine("Phan giai ten mien: " + arg);
                GetHostInfo(arg);
            }
            Console.ReadKey();
        }
    }
}
