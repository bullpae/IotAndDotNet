using Sensors.Dht;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
        }

        private void SwPin_ValueChanged(GpioPin sender, GpioPinValueChangedEventArgs args)
        {
            if (args.Edge == GpioPinEdge.FallingEdge)
            {
                // switch down
                ToggleLED();
            }
            //throw new NotImplementedException();
        }

        private void ToggleLED()
        {
            if (ledPin.Read() == GpioPinValue.High)
            {
                ledPin.Write(GpioPinValue.Low);
                //tmrRead.Stop();
                ToggleTempSensor();
            }
            else
            {
                ledPin.Write(GpioPinValue.High);
                //tmrRead.Start();
                ToggleTempSensor();
            }
        }

        private async void ToggleTempSensor()
        {
            await Task.Factory.StartNew(() => 
            {
                if (ledPin.Read() == GpioPinValue.High)
                {
                    Debug.WriteLine("Start!!");
                    isOn = false;
                }
                else
                {
                    Debug.WriteLine("Stop!!");
                    isOn = true;
                }
            }); // 새로운 스래드에서 생성함

            //if (!isOn)
            //{
            //    Debug.WriteLine("Stop!!");
            //    tmrRead.Stop();
            //}
            //else
            //{
            //    Debug.WriteLine("Start!!");
            //    tmrRead.Start();
            //}
        }

        private async void TmrRead_Tick(object sender, object e)
        {
            var reading = await dht.GetReadingAsync();

            if (reading.IsValid == true)
            {
                if (isOn)
                {
                    Debug.WriteLine($"현재온도:{reading.Temperature}도, 현재습도:{reading.Humidity}");
                }
                else
                {

                }
            }
            //throw new NotImplementedException();
        }
    }
}
