using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadEx1
{
    class Program
    {
        static void Main(string[] args)
        {
            var m1 = new MyClass { Name = "A" };
            var m2 = new MyClass { Name = "B" };
            var m3 = new MyClass { Name = "C" };
            var m4 = new MyClass { Name = "D" };

            //m1.DoSomething();
            //m2.DoSomething();
            //m3.DoSomething();
            //m4.DoSomething();

            //var t1 = new Thread(m1.DoSomething);
            //var t2 = new Thread(m2.DoSomething);
            //var t3 = new Thread(m3.DoSomething);
            //var t4 = new Thread(m4.DoSomething);

            var t1 = new Thread(m1.Increment);
            var t2 = new Thread(m2.Increment);
            var t3 = new Thread(m3.Increment);
            var t4 = new Thread(m4.Increment);

            t1.Start();
            t2.Start();
            t3.Start();
            t4.Start();

            // Thread 가 다 끝날 때 까지 대기
            t1.Join();
            t2.Join();
            t3.Join();
            t4.Join();

            Console.WriteLine($"Value:{MyClass.Value}");
        }
    }

    class MyClass
    {
        public static int Value;
        public static object myLock = new object();
        public string Name { get; set; }

        public void DoSomething()
        {
            for(int i = 0; i < 10; i++)
            {
                Console.Write(Name);
                Thread.Sleep(100);
            }
        }

        public void Increment()
        {
            for (int i = 0; i < 1000000; i++)
            {
                lock (myLock)
                {
                    Value++;
                }
            }
        }
    }
}

