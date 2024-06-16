using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenPop.Mime;
using OpenPop.Pop3;

namespace Bai3
{
    public partial class Form1 : Form
    {
        private Pop3Client pop3Client;
        private List<OpenPop.Mime.Message> messages;

        public Form1()
        {
            InitializeComponent();
        }

        private void btLogin_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

            string server = "pop.gmail.com"; 
            int port = 995; 
            string username = tbEmail.Text;
            string password = tbPassword.Text; 
            int limit = 5; 

            pop3Client = new Pop3Client();

            try
            {
                pop3Client.Connect(server, port, true); 
                pop3Client.Authenticate(username, password);
                int messageCount = pop3Client.GetMessageCount(); 
                int startIndex = messageCount - limit + 1; 
                int endIndex = messageCount; 

               
                dataGridView1.Columns.Add("Subject", "Subject");
                dataGridView1.Columns.Add("From", "From");
                dataGridView1.Columns.Add("Date", "Date");
                dataGridView1.Columns["Subject"].ReadOnly = true;
                dataGridView1.Columns["From"].ReadOnly = true;
                dataGridView1.Columns["Date"].ReadOnly = true;

                // Lặp qua các email từ vị trí bắt đầu đến vị trí kết thúc
                for (int i = endIndex; i >= startIndex; i--)
                {
                    OpenPop.Mime.Message message = pop3Client.GetMessage(i);
                    string subject = message.Headers.Subject; 
                    string from = message.Headers.From.Address; 
                    DateTime date = message.Headers.DateSent; 

                    // Thêm thông tin của email vào DataGridView
                    dataGridView1.Rows.Add(subject, from, date);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message); 
            }
            finally
            {
                if (pop3Client != null && pop3Client.Connected)
                    pop3Client.Disconnect();
            }
        }

        private void cb_show_pass_CheckedChanged(object sender, EventArgs e)
        {
            
                if (cb_show_pass.Checked)
                {
                tbPassword.PasswordChar = '\0';
                }
                else
                {
                tbPassword.PasswordChar = '*';
                }
            
        }

      
    }
}