using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AsyncEx1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private int LongWork(int v)
        {
            int result = 0;

            for (int i = 1; i < v; i++)
            {
                result += i;
                Thread.Sleep(10);
            }

            return result;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int result = LongWork(1000); // 10초 동안 작업
            textBox1.Text = result.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var bw = new BackgroundWorker();
            bw.DoWork += Bw_DoWork; // 비동기적 로직을 작성하는 메소드를 등록
            bw.RunWorkerCompleted += Bw_RunWorkerCompleted;
            bw.RunWorkerAsync();
        }

        private void Bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            textBox1.Text = e.Result.ToString();
            //throw new NotImplementedException();
        }

        private void Bw_DoWork(object sender, DoWorkEventArgs e)
        {
            int result = LongWork(1000);
            // UI 처리 작업이 많은 작업이 있을 경우 에러가난다.(디버그로 돌리면 가끔 에러가 난다)
            e.Result = result;
            //throw new NotImplementedException();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Thread t1 = new Thread(ThreadWork);
            t1.Start();
        }

        private void ThreadWork()
        {
            int result = LongWork(1000);
            Action action = () =>
            {
                textBox1.Text = result.ToString();
            };

            this.Invoke(action);
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            int result = await Task.Factory.StartNew(() => { return LongWork(1000); }); // 새로운 스래드에서 생성함
            // 메인 스래드에서 수행하는 UI를 새로운 스래드에서 처리할 경우 에러 발생
            textBox1.Text = result.ToString(); // 메인 스래드에서 수행함
        }
    }
}
