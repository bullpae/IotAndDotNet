using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DelegateEx2
{
    public delegate void TimerHandler();

    class MyTimer
    {
        private int interval;
        public event TimerHandler Tick;

        public MyTimer(int interval)
        {
            this.interval = interval;
        }

        public void Start()
        {
            for (int i = interval; i > 0; i--)
            {
                Console.WriteLine($"{i} 초 남았습니다.");
                Thread.Sleep(1000);
            }

            if (Tick != null)
                Tick();
        }
    }
}
