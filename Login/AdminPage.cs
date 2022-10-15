using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using MySql.Data.MySqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Login
{
    public partial class AdminPage : Form
    {

        MySqlCommand cmd = new MySqlCommand();
        DataTable d = new DataTable();
        string MyName = "";
        public AdminPage(string usn)
        {
            cmd.Connection = Program.sqlCOnn;
            InitializeComponent();
            label9.Text = "Welcome " + usn;
            MyName = usn;
            label9.Location = new Point(panel1.Width / 2 - label9.Width / 2, label9.Location.Y);
            
            LoadData();
            if (dataGridView1.Rows.Count == 0)
            {
                ClearSelection();
            }
            radioButton2.Checked = true;
        }
        void ClearSelection()
        {
            User_ID.Text = "";
            First_Name.Text = "";
            Last_Name.Text = "";
            User_Name.Text = "";
            Email.Text = "";
            Password.Text = "";
            Phone.Text = "";
            UserType.Text = ""; 
            button2.Enabled = false;
            button5.Enabled = false;
        }
        private async void button1_Click(object sender, EventArgs e)
        {
            cmd.CommandText = $"update users_accounts set first_name = '{First_Name.Text}',lasst_name = '{Last_Name.Text}',user_name = '{User_Name.Text}',email = '{Email.Text}',password = '{Password.Text}',phone = '{Phone.Text}',User_Type = '{UserType.Text}' where user_id = {User_ID.Text};";
            try {await cmd.ExecuteNonQueryAsync(); } catch (Exception) { }
            LoadData();
        }
        async void LoadData()
        {
            cmd.CommandText = "Select * from users_accounts";
            
            
            d.Load(await cmd.ExecuteReaderAsync());
            dataGridView1.DataSource = d;
        }
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                User_ID.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                First_Name.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                Last_Name.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                User_Name.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                Email.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
                Password.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
                Phone.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();
                UserType.Text = dataGridView1.CurrentRow.Cells[7].Value.ToString();
                if(User_Name.Text == MyName)
                {
                    button2.Enabled = false;
                }
                else
                {
                    button2.Enabled = true;
                    
                }
            }
            
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            try {
                DialogResult dr = MessageBox.Show($"Are you sure you want to delete {dataGridView1.CurrentRow.Cells[3].Value} ?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dr == DialogResult.Yes)
                {
                    cmd.CommandText = $"delete from friends where (First_User_ID = {User_ID.Text}) or (Second_User_ID = {User_ID.Text});";
                    try {await cmd.ExecuteNonQueryAsync(); } catch (Exception) { }

                    cmd.CommandText = $"delete from messages where (msg_from = {User_ID.Text}) or (msg_To = {User_ID.Text});";
                    try { await cmd.ExecuteNonQueryAsync(); } catch (Exception) { }

                    cmd.CommandText = $"delete from users_accounts where user_id = {User_ID.Text}";
                    try { await cmd.ExecuteNonQueryAsync(); } catch (Exception) { }
                    LoadData();
                    if (dataGridView1.Rows.Count - 1 > 0)
                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].Selected = true;
                    if (dataGridView1.Rows.Count == 0)
                    {
                        ClearSelection();
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.sqlCOnn.Close();
            Dispose();
            Application.Exit();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Program.sqlCOnn.Close();
            Dispose();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            Program.f1.Show();
            Dispose();
        }

        private async void button5_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure that you want to delete all data","Delete All data", MessageBoxButtons.YesNo,MessageBoxIcon.Warning);
            if (dr == DialogResult.Yes)
            {
                cmd.CommandText = "truncate table friends;";
               await cmd.ExecuteNonQueryAsync();

                cmd.CommandText = "truncate table messages;";
                await cmd.ExecuteNonQueryAsync();

                cmd.CommandText = "delete from users_accounts;";
                await cmd.ExecuteNonQueryAsync();

                LoadData();
                ClearSelection();
                Program.f1.Show();
                Dispose();
            }
        }

        private async void textBox1_TextChanged(object sender, EventArgs e)
        {
            if(radioButton2.Checked == true)
            {
                cmd.CommandText = $"select * from users_accounts where user_name like '%{textBox1.Text}%';";
                d.Clear();
                d.Load(await cmd.ExecuteReaderAsync());
            }
            else if (radioButton1.Checked == true)
            {
                cmd.CommandText = $"select * from users_accounts where user_id like '%{textBox1.Text}%';";
                d.Clear();
                d.Load(await cmd.ExecuteReaderAsync());
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Convert.ToInt32(e.KeyChar) == 13)
            {
                e.Handled = true;
                dataGridView1.Select();
            }
        }
    }
}
