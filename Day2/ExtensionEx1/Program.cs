using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

static class FileUtil
{
    public static string GetFileName(string str)
    {
        //string filePath = @"C:\Program Files\My App\Run.exe";
        return str.Substring(str.LastIndexOf("\\") + 1);
    }
}

namespace ExtensionEx1
{
    class Program
    {

        static void Main(string[] args)
        {
            string s = "I love you!";
            Console.WriteLine($"단어 수:{GetNumOfWords(s)}");
            Console.WriteLine($"단어 수:{s.GetNumOfWords()}");
        }

        static int GetNumOfWords(string s)
        {
            return s.Split(' ').Length;
        }
    }

    static class MyUtil
    {
        public static int GetNumOfWords(this string s)
        {
            return s.Split(' ').Length;
        }
    }
}
