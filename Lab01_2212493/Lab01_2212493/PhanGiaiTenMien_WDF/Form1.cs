using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhanGiaiTenMien_WDF
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string tenMien = txtTenMien.Text;
            if(string.IsNullOrEmpty(tenMien) )
            {
                MessageBox.Show("Vui lòng nhập tên miền!!!");
                return; 
            }
            try
            {
                IPHostEntry hostInfo = Dns.GetHostEntry(tenMien);
                string kq = $"Tên miền: { hostInfo.HostName }\n";
                foreach (IPAddress ipaddr in hostInfo.AddressList)
                {
                    kq += $"\nĐịa chỉ IP: {ipaddr}\n";
                    kq += $"\nSubnet Mask: {GetSubnetMaskFromCIDR(ipaddr, 24)}\n";
                }
                kq += $"\nDefault Gateway: {GetDefaultGateway()}";
                txtKetQua.Text = kq;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không phân giải được tên miền: {tenMien}\nLỗi: {ex.Message}");
            } 
        }
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
    }
}
