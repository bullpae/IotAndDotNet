using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DelegateEx1
{
    class Program
    {
        public delegate void Operation(int v1, int v2);

        static void Main(string[] args)
        {
            Operation o1 = new Operation(Add);
            o1(10, 20);

            Operation o2 = new Operation(Multiply);
            o2(10, 20);

            o2 += new Operation(Sub); // 리턴값이 없는 Delegate 경우만 추가가 가능하다.
            o2(10, 20);
        }

        static void Add(int v1, int v2)
        {
            Console.WriteLine($"SUM:{v1 + v2}");
        }

        static void Multiply(int v1, int v2)
        {
            Console.WriteLine($"SUM:{v1 * v2}");
        }

        static void Sub(int v1, int v2)
        {
            Console.WriteLine($"SUB:{v1 - v2}");
        }
    }
}
