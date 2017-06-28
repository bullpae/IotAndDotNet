using Newtonsoft.Json;
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
    //            timer1.Enabled = true;
				//timer1.Tick += Timer1_Tick;
    //            timer1.Start();
            }
            else
            {
                AddLog("연결이 실패 되었습니다.");
            }
            //throw new NotImplementedException();
        }

		private void Timer1_Tick(object sender, EventArgs e)
		{
			SendDeviceInfo(); // 1초에 한번씩 전송하게 타이머 셋팅
			//throw new NotImplementedException();
		}

		private void timer1_Tick(object sender, EventArgs e)
        {
            SendDeviceInfo(); // 1초에 한번씩 전송하게 타이머 셋팅
        }

        private void SendDeviceInfo()
        {
            var info = new DeviceInfo
            {
                DeviceId = $"D00{new Random().Next(9)}",
                Temperature = new Random().NextDouble() * 40,
                Humidity = new Random().NextDouble() * 120,
                Power = new Random().Next(2) == 1
            };

            string json = JsonConvert.SerializeObject(info);
            byte[] bytes = Encoding.Unicode.GetBytes(json); // 전송하기 위해 바이트 배역로 변환

            var args = new SocketAsyncEventArgs();
            args.SetBuffer(bytes, 0, bytes.Length);
            socket.SendAsync(args);
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            AddLog("전송을 시작합니다.");
            timer1.Enabled = true;
			timer1.Tick += Timer1_Tick;
			timer1.Start();
        }
    }

    class DeviceInfo
    {
        public string DeviceId { get; set; }
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public bool Power { get; set; }
    }
}
