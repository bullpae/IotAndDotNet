using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyEx1
{
    class Program
    {
        static void Main(string[] args)
        {
            var s1 = new Student();
            s1.StudentNo = "S001";
            s1.Name = "Test";
            s1.Age = 1222;
        }
    }

    class Student
    {
        public string StudentNo { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
