using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelegateEx2
{
    class Program
    {
        static void Main(string[] args)
        {
            MyTimer t1 = new MyTimer(10);
            t1.Tick += TurnOffGas;
            t1.Tick += Boom;
            t1.Tick += T1_Tick;
            t1.Start();
        }

        private static void T1_Tick()
        {
            Console.WriteLine("TEST!!");
            //throw new NotImplementedException();
        }

        static void TurnOffGas()
        {
            Console.WriteLine("가스불을 끕니다.");
        }

        static void Boom()
        {
            Console.WriteLine("시한 폭탄이 터집니다.");
        }
    }
}
