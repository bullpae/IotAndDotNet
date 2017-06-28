using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JsonSerializationEx1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            

        }

        private void btnSerialize_Click(object sender, EventArgs e)
        {
            var student = new Student
            {
                StudentNo = txtStudentNo.Text,
                Name = txtName.Text,
                Age = Convert.ToInt32(txtAge.Text)
            };

            string json = JsonConvert.SerializeObject(student);

            txtJson.Text = json;
        }

        private void btnDeserialize_Click(object sender, EventArgs e)
        {
            var student = JsonConvert.DeserializeObject<Student>(txtJson.Text);

            txtStudentNo.Text = student.StudentNo;
            txtName.Text = student.Name;
            txtAge.Text = student.Age.ToString();
        }
    }

    class Student
    {
        public string StudentNo { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
