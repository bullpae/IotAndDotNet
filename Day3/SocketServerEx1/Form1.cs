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

namespace SocketServerEx1
{
    public partial class Form1 : Form
    {
        Socket serverSocket;
        Socket clientSocket;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        private void Accept()
        {
            var e = new SocketAsyncEventArgs();

            e.Completed += ClientAccepted;
            serverSocket.AcceptAsync(e);
        }

        private void ClientAccepted(object sender, SocketAsyncEventArgs e)
        {
            // 클라이언트가 연결 요청을 하여 연결된 상태
            if (e.SocketError == SocketError.Success)
            {
                clientSocket = e.AcceptSocket;
                AddLog("클라이언트와 연결되었습니다.");
            }
           
            //throw new NotImplementedException();
        }

        private void AddLog(string log)
        {
            Action action = () => { richTextBoxLog.AppendText(log + "\n"); };
            this.Invoke(action);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            var endPoint = new IPEndPoint(IPAddress.Any, Convert.ToInt32(txtPortNo.Text));
            serverSocket.Bind(endPoint);
            serverSocket.Listen(10); // 동시 처리 수 설정
            Accept();

            AddLog("서비스가 시작되었습니다.");
        }
    }
}
