﻿using Newtonsoft.Json;
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

                ReceiveInfo(e.AcceptSocket);
            }
           
            //throw new NotImplementedException();
        }

		private void ReceiveInfo(Socket socket)
        {
            var args = new SocketAsyncEventArgs();
            args.SetBuffer(new byte[1024], 0, 1024);
            args.Completed += DataArrived;
            args.AcceptSocket = socket;
            socket.ReceiveAsync(args);
        }

        private void DataArrived(object sender, SocketAsyncEventArgs e)
        {
            byte[] bytes = e.Buffer;
            string json = Encoding.Unicode.GetString(bytes, 0, e.BytesTransferred);

            AddLog(json);

            Action action = () => { RefreshUI(json); }; // 메인스레드를 컨트롤 해야하기 때문에 사용(Cross Thread error!)
            this.Invoke(action); // this(메인 스래드)에서 RefreshUI함수를 호출

            ReceiveInfo(e.AcceptSocket);
            //RceiveInfo(clientSocket);
			
			//throw new NotImplementedException();
		}

        private void RefreshUI(string json)
        {
			var info = JsonConvert.DeserializeObject<DeviceInfo>(json);
            lblTemp.Text = $"현재온도:{info.Temperature:0.00}도";
            lblHumidity.Text = $"현재습도:{info.Humidity:0.00}%";
            rdoOn.Checked = info.Power;
            rdoOff.Checked = !info.Power;
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

        private void PowerStateChanged(object sender, EventArgs e)
        {
            var control = new DeviceControl
            {
                DeviceId = "D001",
                Power = rdoOn.Checked
            };

            var args = new SocketAsyncEventArgs();
            string json = JsonConvert.SerializeObject(control);
            byte[] bytes = Encoding.Unicode.GetBytes(json);

            AddLog(json);

            args.SetBuffer(bytes, 0, bytes.Length);
            clientSocket.SendAsync(args);
        }
    }

    class DeviceInfo
    {
        public string DeviceId { get; set; }
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public bool Power { get; set; }
    }

    class DeviceControl
    {
        public string DeviceId { get; set; }
        public bool Power { get; set; }
    }
}
