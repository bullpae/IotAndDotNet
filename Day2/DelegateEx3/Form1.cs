using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DelegateEx3
{
    public partial class Form1 : Form
    {
        public delegate bool FilterHandler(Student s);

        List<Student> list = new List<Student>();

        public Form1()
        {
            InitializeComponent();

            list.Add(new Student { StudentNo = "S001", Name = "1", Age = 21 });
            list.Add(new Student { StudentNo = "S002", Name = "2", Age = 22 });
            list.Add(new Student { StudentNo = "S003", Name = "3", Age = 23 });
            list.Add(new Student { StudentNo = "S004", Name = "4", Age = 24 });
            list.Add(new Student { StudentNo = "S005", Name = "5", Age = 25 });
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = list;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //dataGridView1.DataSource = FilterByName();
            dataGridView1.DataSource = FilterByName(Condition1);
            // 익명 대리자
            dataGridView1.DataSource = FilterByName(delegate (Student s) { return s.Name.IndexOf(textBox1.Text) >= 0; });
            // 람다
            dataGridView1.DataSource = FilterByName(s => s.Name.IndexOf(textBox1.Text) >= 0);
        }

        private bool Condition1(Student s)
        {
            return s.Name.IndexOf(textBox1.Text) >= 0;
        }

        List<Student> FilterByName(FilterHandler filter)
        {
            List<Student> filteredList = new List<Student>();

            foreach (var s in list)
            {
                if (filter(s))
                {
                    filteredList.Add(s);
                }
            }

            return filteredList;
        }

        List<Student> FilterByName()
        {
            List<Student> filteredList = new List<Student>();

            foreach (Student s in list)
            {
                if (s.Name.IndexOf(textBox1.Text) >= 0)
                {
                    filteredList.Add(s);
                }
            }

            return filteredList;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = FilterByName(s => s.Age >= Convert.ToInt32(textBox2.Text));
        }
    }

    public class Student
    {
        public string StudentNo { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
