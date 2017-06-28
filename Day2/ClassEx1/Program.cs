using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassEx1
{
    class Program
    {
        static void Main(string[] args)
        {
            //Student s1 = new Student();
            var s1 = new Student(); // var 키워드는 지역변수만 가능
            s1.studentNo = "S001";
            s1.name = "홍길동";
            s1.age = 8;
            s1.DisplayInfo();

            Student s2 = new Student()
            {
                // Ctrl + space 로 멤버변수 출력
                studentNo = "S002",
                name = "감자",
                age = 100
            };
            s2.DisplayInfo();
        }
    }

    class Student
    {
        public string studentNo;
        public string name;
        public int age;

        public void DisplayInfo ()
        {
            Console.WriteLine($"학번:{studentNo}");
            Console.WriteLine($"이름:{name}");
            Console.WriteLine($"나이:{age}");
        }
    }
}
