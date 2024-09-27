using AsyncSocketTCP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AsyncSocketServer
{
    public partial class Form1 : Form
    {
        AsyncSocketTCPServer mServer;
        public Form1()
        {
            InitializeComponent();
            mServer = new AsyncSocketTCPServer();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            mServer.StartListeningForIcomingConnection();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            mServer.StopServer();   
        }

        private void txtText_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            mServer.SendToAll(txtText.Text.Trim());
        }
    }
}
