using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Configuration;

namespace MailTest.TestApplication
{

    public partial class SendForm : Form
    {
        public SendForm()
        {
            InitializeComponent();
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            //textBox1.Text = "mail.dashinginfo.com";
            //textBox2.Text = "prj_test@dashinginfo.com";
            //textBox3.Text = "0804111";

            textBox1.Text = "Test 邮件服务";
            textBox2.Text = "邮件测试";

            textBox5.Text = ConfigurationManager.ConnectionStrings["SqlMain"].ConnectionString;
            textBox6.Text = "xiaochao.duan@dashinginfo.com";
            textBox7.Text = "feng.jiao@dashinginfo.com";
        }

        private void textBox4_MouseClick(object sender, MouseEventArgs e)
        {
            OpenFileDialog f = new OpenFileDialog();
            f.Filter = "所有文件(*.*)|*.*";
            if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox4.Text = f.FileName;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Dashinginfo.Practices.Library.Database.SqlDatabase db = Dashinginfo.Practices.Library.Database.SqlDatabasePool.Singleton.BorrowDBConnection();
            //MessageBox.Show(db.Connection.ConnectionString);
            string sendName = "Admin Dashing";
            string sendMail = textBox2.Text;
            if (textBox6.Text == "")
            {
                MessageBox.Show("发送地址不能为空");
                return;
            }

            string toUser = textBox6.Text;
            string ccUser = textBox7.Text;
            string subject = textBox1.Text;
            string content = textBox2.Text;
            Dictionary<string, byte[]> attachMent = new Dictionary<string, byte[]>();
            //attachMent.Add("pic1.png", File.ReadAllBytes(textBox4.Text));
            Dashinginfo.Practices.Mail.OutMailDB.SendMail(toUser, ccUser, subject, content, attachMent);
            Dashinginfo.Practices.Library.Database.SqlDatabasePool.Singleton.ReturnDBConnection(db);
            MessageBox.Show("Insert Success！");
        }
    }
}
