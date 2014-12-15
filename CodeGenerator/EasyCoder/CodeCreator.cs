using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Data.SqlClient;
using System.Configuration;

namespace EasyCoder
{
    public partial class CodeCreator : Form
    {
        private string connectionString = "";
        private string dbName = "";
        public CodeCreator()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MaximizeBox = false;
        }
        private void CodeCreator_Load(object sender, EventArgs e)
        {
            this.textBox1.Text = ConfigurationManager.AppSettings["NameSpaces"];
            //加载数据库类型
            this.comboBox1.Items.AddRange(new object[] {
            DbType.SQL,
            DbType.Oracle,
            DbType.MySQL}); 
        }


        //查看列表
        private void button2_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            int selected = this.comboBox1.SelectedIndex; 
            if (selected != -1 && this.cbbdatabase.SelectedItem != null)
            {
                if (selected == Convert.ToInt32(DbType.SQL))
                {
                    //TO DO SQL
                    IDbAccess sql = new SqlAccess();
                    string sqlText = " SELECT A.NAME,B.NAME OWNER FROM SYSOBJECTS A LEFT JOIN SYSUSERS B ON A.UID = B.UID  WHERE A.TYPE = 'U' AND A.NAME<>'dtproperties'  ORDER BY A.NAME";
                    this.connectionString = @"Data Source=" + this.txtip.Text + ";Initial Catalog=" +this.cbbdatabase.SelectedItem.ToString()+ ";User ID=" + this.txtuserid.Text
                       + ";Password=" + this.txtpassword.Text;
                    sql.ConnectionString = this.connectionString;
                    dt = sql.Query(sqlText);  
                }
                else if (selected == Convert.ToInt32(DbType.Oracle))
                {
                    //TO DO ORACLE                     
                    OracleAccess ora = new OracleAccess();
                    string sqlText = @"select table_name as NAME,owner from all_tables where OWNER='" + this.txtuserid.Text.ToUpper() + "'";
                    this.connectionString = @"Data Source=" + this.txtip.Text + ";User ID=" + this.txtuserid.Text
                       + ";Password=" + this.txtpassword.Text;
                    dt = ora.ExecuteDataTableSql(sqlText, connectionString);
                } 
                else if (selected == Convert.ToInt32(DbType.MySQL))
                {
                    //TO DO MYSQL                     
                    IDbAccess mysql = new MySQLAccess();
                    string sqlText = "SHOW tables";
                    this.connectionString = @"Server=" + this.txtip.Text + ";Port=3336;User ID=" + this.txtuserid.Text
                        + ";Password=" + this.txtpassword.Text + ";DATABASE=" + this.cbbdatabase.SelectedItem.ToString(); 
                    mysql.ConnectionString = this.connectionString;
                    dt = mysql.Query(sqlText);
                    dt.Columns[0].ColumnName = "NAME";
                }
                DataView dv = new DataView(dt);
                dv.RowFilter = "NAME not like 'Mail%' and Name <> 'log'";
                this.checkedListBox1.DataSource = dv; 
                checkedListBox1.DisplayMember = "NAME";
            }

        } 

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.checkedListBox1.DataSource = null;

        }

        //全选
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox1.Checked == true)
            {
                for (int i = 0; i < this.checkedListBox1.Items.Count; i++)
                {
                    this.checkedListBox1.SetItemCheckState(i, CheckState.Checked);
                }
            }
            else if (this.checkBox1.Checked == false)
            {
                for (int i = 0; i < this.checkedListBox1.Items.Count; i++)
                {
                    this.checkedListBox1.SetItemCheckState(i, CheckState.Unchecked);
                }
            }

        } 

        //生成文件
        private void button1_Click(object sender, EventArgs e)
        {
            int selected = this.comboBox1.SelectedIndex;
            this.label6.Visible = false;
            this.progressBar1.Visible = true;
            this.progressBar1.Value = 0;
            for (int i = 0; i < 90; i++)
            {
                System.Threading.Thread.Sleep(30);
                this.progressBar1.Value += 1;
            }
            int jj = this.progressBar1.Value;
            string tablename = "";
            string namespacename = this.textBox1.Text;
            for (int i = 0; i < this.checkedListBox1.CheckedItems.Count; i++)
            {
                DataRowView dv = ((DataRowView)this.checkedListBox1.CheckedItems[i]);
                tablename += dv["NAME"].ToString() + ",";
            }

            tablename = tablename.TrimEnd(',');
            if (selected == Convert.ToInt32(DbType.SQL))
            {
                //TO DO SQL
                ICreator sql = new SqlCreator();
                this.dbName = this.cbbdatabase.SelectedItem.ToString();
                string ownerName = "";
                sql.CreateAllCode(this.connectionString, this.dbName, tablename, namespacename, ownerName);
            }
            else if (selected == Convert.ToInt32(DbType.Oracle))
            {
                //TO DO ORACLE   
                this.dbName = this.cbbdatabase.SelectedItem.ToString();
                ICreator oracle = new OracleCreator();
                List<string> ownerNames = new List<string>();
                ownerNames = this.connectionString.Split(';').ToList();
                string ownerName = ownerNames[1].Replace("User ID=", "").ToUpper();
                oracle.CreateAllCode(this.connectionString, this.dbName, tablename, namespacename, ownerName);
            }
            else if (selected == Convert.ToInt32(DbType.MySQL))
            {
                //TO DO MySQL   
                this.dbName = this.cbbdatabase.SelectedItem.ToString();
                ICreator mysql = new MySQLCreator();
                this.dbName = this.cbbdatabase.SelectedItem.ToString();
                string ownerName = ""; 
                mysql.CreateAllCode(this.connectionString, this.dbName, tablename, namespacename, ownerName);
            }
            for (int i = 90; i < 100; i++)
            {
                System.Threading.Thread.Sleep(20);
                this.progressBar1.Value += 1;
            }
            if (this.progressBar1.Value == 100)
            {
                this.progressBar1.Visible = false;
                this.label6.Visible = true;
            }
        }
        
        //选择数据库
        private void cbbdatabase_Click(object sender, EventArgs e)
        {
            this.cbbdatabase.Items.Clear();
            DataTable dt = null;
            int selected = this.comboBox1.SelectedIndex;
            if (selected != -1)
            {
                if (selected == Convert.ToInt32(DbType.SQL))
                {
                    //TO DO SQL
                    IDbAccess sql = new SqlAccess();
                    string sqlText = @"USE master
                                       SELECT name FROM  dbo.sysdatabases ORDER BY name;";
                    this.connectionString = @"Data Source=" + this.txtip.Text + ";" + "User ID=" + this.txtuserid.Text
                        + ";Password=" + this.txtpassword.Text;
                    sql.ConnectionString = this.connectionString;
                    dt = sql.Query(sqlText);
                }
                else if (selected == Convert.ToInt32(DbType.Oracle))
                { 
                    //TO DO ORACLE
                    OracleAccess ora = new OracleAccess();
                    string sqlText = @"select sys_context('userenv', 'db_name') AS NAME from dual";
                    this.connectionString = @"Data Source=" + this.txtip.Text + ";" + "User ID=" + this.txtuserid.Text
                        + ";Password=" + this.txtpassword.Text;
                    dt = ora.ExecuteDataTableSql(sqlText, connectionString);
                }
                else if (selected == Convert.ToInt32(DbType.MySQL))
                {
                    //TO DO MySQL
                    IDbAccess mysql = new MySQLAccess();
                    string sqlText = @"show databases";
                    this.connectionString = @"Server=" + this.txtip.Text + ";Port=3336;" + "User ID=" + this.txtuserid.Text
                        + ";Password=" + this.txtpassword.Text;
                    mysql.ConnectionString = this.connectionString;
                    dt = mysql.Query(sqlText);
                    dt.Columns[0].ColumnName = "name";
                }  
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    this.cbbdatabase.Items.Add(dt.Rows[i]["name"].ToString());
                } 
            } 
        }  
       
    }
}
