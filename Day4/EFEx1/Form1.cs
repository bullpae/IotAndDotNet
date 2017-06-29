using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EFEx1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            RefreshGrid();
        }

        private void RefreshGrid()
        {
            using (var db = new DBContext())
            {
                //dataGridView1.DataSource = db.Students.ToList();
                //LINQ를 이용한
                var students = from p in db.Students
                               orderby p.Name 
                               select p;
                dataGridView1.DataSource = students.ToList();
            }
        }

        private void RefreshGrid(string condition)
        {
            using (var db = new DBContext())
            {
                //dataGridView1.DataSource = db.Students.ToList();
                //LINQ를 이용한
                var students = from p in db.Students
                               where p.Name.IndexOf(condition) >= 0
                               orderby p.Name
                               select p;
                dataGridView1.DataSource = students.ToList();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var student = new Student()
            {
                StudentNo = txtStudentNo.Text,
                Name = txtName.Text,
                Age = Convert.ToInt32(txtAge.Text)
            };

            using (var db = new DBContext())
            {
                db.Students.Add(student);
                db.SaveChanges(); // DB에 대한 변경사항을 DB에 반영

                txtStudentNo.Text = "";
                txtName.Text = "";
                txtAge.Text = "";

                RefreshGrid();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            using (var db = new DBContext())
            {
                // 삭제할 학생 객체의 참조값을 얻어냄
                var student = db.Students.SingleOrDefault(p => p.StudentNo == txtStudentNo.Text);

                if (student != null)
                {
                    db.Students.Remove(student);
                    db.SaveChanges();

                    RefreshGrid();
                }

            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtStudentNo.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            txtName.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            txtAge.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            using (var db = new DBContext())
            {
                var student = db.Students.SingleOrDefault(p => p.StudentNo == txtStudentNo.Text);
                student.Name = txtName.Text;
                student.Age = Convert.ToInt32(txtAge.Text);
                db.SaveChanges();

                RefreshGrid();
             }
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            RefreshGrid(txtName.Text);
        }
    }
}
