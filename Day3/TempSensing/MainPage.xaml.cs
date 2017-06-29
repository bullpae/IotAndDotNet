using Newtonsoft.Json;
using Sensors.Dht;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// 빈 페이지 항목 템플릿에 대한 설명은 https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x412에 나와 있습니다.

namespace TempSensing
{
    /// <summary>
    /// 자체적으로 사용하거나 프레임 내에서 탐색할 수 있는 빈 페이지입니다.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        IDht dht;
        GpioPin dataPin;
        GpioPin ledPin;
        GpioPin swPin;
        DispatcherTimer tmrRead;
        bool isOn = true;

        Socket socket;

        private double oldTemperture;
        private double oldHumidity;

        public MainPage()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            dataPin = GpioController.GetDefault().OpenPin(4);
            dht = new Dht11(dataPin, GpioPinDriveMode.Input);
            tmrRead = new DispatcherTimer();
            tmrRead.Interval = TimeSpan.FromMilliseconds(1000);
            tmrRead.Tick += TmrRead_Tick;
            tmrRead.Start();

            ledPin = GpioController.GetDefault().OpenPin(5);
            ledPin.Write(GpioPinValue.High);
            //ledPin.Write(GpioPinValue.Low);
            ledPin.SetDriveMode(GpioPinDriveMode.Output);

            swPin = GpioController.GetDefault().OpenPin(6);
            swPin.SetDriveMode(GpioPinDriveMode.Input);
            swPin.ValueChanged += SwPin_ValueChanged;
            swPin.DebounceTimeout = TimeSpan.FromMilliseconds(30); // 0.03 sec 이내 변화시

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        private async void SwPin_ValueChanged(GpioPin sender, GpioPinValueChangedEventArgs args)
        {
            if (args.Edge == GpioPinEdge.FallingEdge)
            {
                // switch down
                //ToggleLED();
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { ToggleLED(); });
            }
            //throw new NotImplementedException();
        }

        private void ToggleLED()
        {
            if (ledPin.Read() == GpioPinValue.High)
            {
                Debug.WriteLine("Stop!!");
                isOn = false;
                ledPin.Write(GpioPinValue.Low);                
                elLED.Fill = new SolidColorBrush(Colors.Black);
            }
            else
            {
                Debug.WriteLine("Start!!");
                isOn = true;
                ledPin.Write(GpioPinValue.High);
                elLED.Fill = new SolidColorBrush(Colors.Red);
            }

            SendDeviceInfo(oldTemperture, oldHumidity, ledPin.Read() == GpioPinValue.High);
        }

        private void AddLog(string log)
        {
            Debug.WriteLine(log);
            //Action action = () => { rtbLog.text .AppendText(log + "\n"); };
            //this.Invoke(action);
        }

        private async void TmrRead_Tick(object sender, object e)
        {
            var reading = await dht.GetReadingAsync();

            if (reading.IsValid == true)
            {
                if (isOn)
                {
                    if (oldTemperture != reading.Temperature || oldHumidity != reading.Humidity)
                    {
                        // 온도나 습도가 변경된 경우
                        SendDeviceInfo(reading.Temperature,
                            reading.Humidity,
                            ledPin.Read() == GpioPinValue.High);

                        oldHumidity = reading.Humidity;
                        oldTemperture = reading.Temperature;
                    }
                    
                    tbTemp.Text = $"현재온도:{reading.Temperature:0.0}도";
                    tbHumidity.Text = $"현재습도:{reading.Humidity:0.0}%";
                    Debug.WriteLine($"현재온도:{reading.Temperature:#.0}도, 현재습도:{reading.Humidity:#.0}% LED 상태:{isOn}");
                }
            }
            //throw new NotImplementedException();
        }

        private void SendDeviceInfo(double temperture, double humidity, bool power)
        {
            
            if(!socket.Connected)
            {
                // 연결이 안된경우 연결 시도
                var args = new SocketAsyncEventArgs();
                args.RemoteEndPoint = new IPEndPoint(IPAddress.Parse("192.168.137.1"), 11000);
                args.Completed += Connected;
                socket.ConnectAsync(args);

                return;
            }

            // 서버에 연결된 상태에서 수행되는 코드
            var info = new DeviceInfo
            {
                DeviceId = $"D001",
                Temperature = temperture,
                Humidity = humidity,
                Power = power
            };

            string json = JsonConvert.SerializeObject(info); // 전송하기 위해 직렬화
            byte[] bytes = Encoding.Unicode.GetBytes(json); // 전송하기 위해 바이트 배열로 변환

            var args2 = new SocketAsyncEventArgs();
            args2.SetBuffer(bytes, 0, bytes.Length);
            socket.SendAsync(args2);
        }

        private void Connected(object sender, SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success)
            {
                AddLog("연결되었습니다.");

                SendDeviceInfo(oldTemperture,
                            oldHumidity,
                            ledPin.Read() == GpioPinValue.High);

                ReceiveControlPacket();
            }
            else
            {
                AddLog("연결이 실패 되었습니다.");
            }
            //throw new NotImplementedException();
        }

        private void ReceiveControlPacket()
        {
            var args = new SocketAsyncEventArgs();
            args.SetBuffer(new byte[1024], 0, 1024);
            args.Completed += DataArrived;
            socket.ReceiveAsync(args);
        }

        private async void DataArrived(object sender, SocketAsyncEventArgs e)
        {
            string json = Encoding.Unicode.GetString(e.Buffer, 0, e.BytesTransferred);
            var control = JsonConvert.DeserializeObject<DeviceControl>(json);

            AddLog(json);

            if (control != null)
            {
                bool currentPower = ledPin.Read() == GpioPinValue.High;
                if (currentPower != control.Power)
                {
                    //ToggleLED();// 에러 발생!! 메인 스래드에 있는 UI를 건들기때문
                    await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { ToggleLED(); });
                }
            }

            ReceiveControlPacket(); // 계속 받기 위해 대기
            //throw new NotImplementedException();
        }

        private void elLED_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            ToggleLED();
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
