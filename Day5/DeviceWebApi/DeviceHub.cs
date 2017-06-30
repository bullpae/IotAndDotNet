using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace DeviceWebApi
{
    public class DeviceHub : Hub
    {
        public void SendDeviceInfo(DeviceLog log)
        {
            Clients.Others.DisplayState(log);
        }

        public void ControlDevice(DeviceControl control)
        {
            Clients.Others.ControlDevice(control);
        }
    }

    public class DeviceControl
    {
        public bool Power { get; set; }
    }
}