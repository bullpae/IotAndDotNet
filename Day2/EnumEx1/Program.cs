using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnumEx1
{
    class Program
    {
        enum Gender { 미정, 남자, 여자 }

        static void Main(string[] args)
        {
            Gender gender;
            gender = Gender.남자;
            Console.WriteLine($"성별:{gender}");
        }
    }
}
