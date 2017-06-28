using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefEx1
{
    class Program
    {
        static void Main(string[] args)
        {
            int v = 10;
            Increment(v);
            Console.WriteLine($"v:{v}");
            Increment(ref v);
            Console.WriteLine($"v:{v}");

            //int r = 50;
            //GetRandomNum(ref r);
            //Console.WriteLine($"r:{r}");
            
            int r;
            GetRandomNum(out r);
            Console.WriteLine($"r:{r}");
        }

        static void Increment(int v)
        {
            v++;
            Console.WriteLine($"v:{v}");
        }

        static void Increment(ref int v)
        {
            v++;
            Console.WriteLine($"v:{v}");
        }

        //static void GetRandomNum(ref int r)
        //{
        //    r = new Random().Next(100) + 1;
        //}

        static void GetRandomNum(out int r)
        {
            //Console.WriteLine($"r:{r}"); // error 발생 생성된 값이 없기 때문
            r = new Random().Next(100) + 1;
        }
    }
}
