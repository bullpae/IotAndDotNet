﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace DeviceWebApi
{
    public class MyHub2 : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }
    }
}