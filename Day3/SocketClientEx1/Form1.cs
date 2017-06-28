using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SocketClientEx1
{
    public partial class Form1 : Form
    {
        Socket socket;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        private void AddLog(string log)
        {
            Action action = () => { richTextBoxLog.AppendText(log + "\n"); };
            this.Invoke(action);
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            var args = new SocketAsyncEventArgs();
            args.RemoteEndPoint = new IPEndPoint(IPAddress.Parse(txtServerIp.Text), Convert.ToInt32(txtServerPort.Text));
            args.Completed += Connected;
            socket.ConnectAsync(args);
        }

        private void Connected(object sender, SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success)
            {
                AddLog("연결되었습니다.");
            }
            else
            {
                AddLog("연결이 실패 되었습니다.");
            }
            //throw new NotImplementedException();
        }
    }
}
