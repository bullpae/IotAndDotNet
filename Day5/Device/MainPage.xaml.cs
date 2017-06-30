using Sensors.Dht;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Sockets;
using System.Runtime.InteropServices.WindowsRuntime;
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

// 빈 페이지 항목 템플릿은 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 에 문서화되어 있습니다.

namespace Device
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

        private async void SendDeviceInfo(double temperture, double humidity, bool power)
        {
            // 서버에 연결된 상태에서 수행되는 코드
            var info = new DeviceLog
            {
                Temperature = temperture,
                Humidity = humidity,
                Power = power
            };

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://iotandrasp.azurewebsites.net/api/");
                await client.PostAsync<DeviceLog>("Device", info, new JsonMediaTypeFormatter());
            }
        }

       
    
        private void elLED_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            ToggleLED();
        }

        
    }

    class DeviceLog
    {
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
