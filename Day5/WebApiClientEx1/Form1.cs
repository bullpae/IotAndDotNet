using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebApiClientEx1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //RefreshUI();
        }

        private async void RefreshUI()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:53746/api/");
                //client.BaseAddress = new Uri("http://192.168.137.1:53746/api/");
                var response = await client.GetAsync("Student");

                if (response.IsSuccessStatusCode)
                {
                    var studentList = await response.Content.ReadAsAsync<List<Student>>();
                    dataGridView1.DataSource = studentList;
                }
            }
        }

        private void btnViewStudentList_Click(object sender, EventArgs e)
        {
            RefreshUI();
        }
    }

    class Student
    {
        public string StudentNo { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
