using System;
using System.Net.Mail;
using System.Windows.Forms;
using MailKit;
using MailKit.Net.Imap;
using MimeKit;

namespace Bai6
{
    public partial class MainScreen : Form
    {
        public MainScreen()
        {
            InitializeComponent();
        }
        ImapClient client = new ImapClient();

        private void btn_Login_Click(object sender, EventArgs e)
        {
            txt_IMAP.Enabled = false;
            txt_Mat_Khau.Enabled = false;
            txt_Port_IMAP.Enabled = false;
            txt_Port_SMTP.Enabled = false;
            txt_SMTP.Enabled = false;
            txt_Tai_Khoan.Enabled = false;
            btn_Login.Visible = false;
            btn_Logout.Visible = true;
            btn_Refresh.Visible = true;
            btn_Send.Visible = true;
            string server = txt_IMAP.Text;
            int port = Convert.ToInt32(txt_Port_IMAP.Text);
            string username = txt_Tai_Khoan.Text;
            string password = txt_Mat_Khau.Text;
            client.Connect(server, port, true); 
            client.Authenticate(username, password); 
            IMailFolder inbox = client.Inbox;
            inbox.Open(FolderAccess.ReadOnly);
            for (int i = 0; i < 15; i++) 
            {
                MimeMessage message = inbox.GetMessage(i);
                ListViewItem mail = new ListViewItem();
                mail.Tag = message;
                mail.Text = i.ToString();
                mail.SubItems.Add(message.From.ToString());
                mail.SubItems.Add(message.Subject);
                mail.SubItems.Add(message.Date.ToString());
                lv_mails.Items.Add(mail);
            }
        }

        private void btn_Logout_Click(object sender, EventArgs e)
        {
            client.Disconnect(true);
            txt_IMAP.Enabled = true;
            txt_Mat_Khau.Enabled = true;
            txt_Port_IMAP.Enabled = true;
            txt_Port_SMTP.Enabled = true;
            txt_SMTP.Enabled = true;
            txt_Tai_Khoan.Enabled = true;
            btn_Login.Visible = true;
            btn_Logout.Visible = false;
            btn_Refresh.Visible = false;
            btn_Send.Visible = false;
            lv_mails.Items.Clear();
        }

        private void btn_Send_Click(object sender, EventArgs e)
        {
            SMTP_Client_Info client = new SMTP_Client_Info(txt_SMTP.Text, Convert.ToInt32(txt_Port_SMTP.Text), txt_Tai_Khoan.Text, txt_Mat_Khau.Text);
            Send new_mail = new Send();
            new_mail.Tag = client;
            new_mail.ShowDialog();
        }

        private void btn_Refresh_Click(object sender, EventArgs e)
        {
            lv_mails.Items.Clear();
            IMailFolder inbox = client.Inbox;
            inbox.Open(FolderAccess.ReadOnly);
            for (int i = 0; i < 20; i++) //inbox.Count
            {
                MimeMessage message = inbox.GetMessage(i);
                ListViewItem mail = new ListViewItem();
                mail.Tag = message;
                mail.Text = i.ToString();
                mail.SubItems.Add(message.From.ToString());
                mail.SubItems.Add(message.Subject);
                mail.SubItems.Add(message.Date.ToString());
                lv_mails.Items.Add(mail);
            }
        }

        private void lv_mails_Click(object sender, EventArgs e)
        {
            Mails item = new Mails();
            item.Text = lv_mails.SelectedItems[0].SubItems[2].Text;
            item.Tag = lv_mails.SelectedItems[0].Tag;
            item.ShowDialog();
        }
    }
}