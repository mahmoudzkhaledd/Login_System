using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
namespace Login
{
    public partial class PeopleList : Form
    {
        UserInfo info;
        MySqlCommand cmd = new MySqlCommand();
        DataTable t = new DataTable();
        int User_id = 0;
        string Name = "";
        string Friend_State = "";
        public PeopleList(UserInfo i)
        {
            cmd.Connection = Program.sqlCOnn;
            info = i;
            InitializeComponent();
            LoadData();
        }
        async void LoadData()
        {
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            cmd.CommandText = $"select user_id , user_name, State from(select user_id, user_name,User_Type ,State from users_accounts left outer join friends on First_User_ID = {info.ID} and Second_User_ID = user_id) g where User_Type != 'Admin';";
            t.Clear();
            t.Load(await cmd.ExecuteReaderAsync());
            
            for(int i = 0; i < t.Rows.Count; i++)
            {
                DataRow dr = t.Rows[i];

                if (dr[0].ToString() == info.ID.ToString())
                {
                    dr.Delete();
                    break;
                }
            }
            
            t.AcceptChanges();
            dataGridView1.DataSource = t;
            dataGridView1.Columns[0].Visible = false;

            if (dataGridView1.Rows.Count == 0)
            {
                button1.Enabled = false;
                button2.Enabled = false;
            }
            else
            {
                button1.Enabled = true;
                button2.Enabled = true;
            }

        }
        private void button3_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                cmd.CommandText = $"insert into friends(First_User_ID,Second_User_ID,State) values({info.ID},{User_id},'Pending'); insert into friends(First_User_ID,Second_User_ID,State) values({User_id},{info.ID},'Pending');";
                if (await cmd.ExecuteNonQueryAsync() != 0)
                    LoadData();
            }
            catch
            {
                MessageBox.Show("Error");
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                User_id = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
                Name = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                Friend_State = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                if (User_id != info.ID)
                {
                    button1.Enabled = true;
                    button2.Enabled = true;
                    if (Friend_State == "")
                    {
                        button1.Enabled = true;
                        button2.Enabled = false;
                    }
                    else if (Friend_State == "Accepted")
                    {
                        button1.Enabled = false;
                        button2.Enabled = true;
                    }
                    else if (Friend_State == "Pending")
                    {
                        button1.Enabled = false;
                        button2.Enabled = true;
                    }
                }
                else
                {
                    button1.Enabled = false;
                    button2.Enabled = false;

                }

            }

        }

        private async void button2_Click(object sender, EventArgs e)
        {
            DialogResult d = MessageBox.Show("Are you sure that you want to cancel request ?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (d == DialogResult.Yes)
            {
                cmd.CommandText = $"delete from friends where (First_User_ID = {info.ID} and Second_User_ID = {User_id} ) or (First_User_ID = {User_id} and Second_User_ID = {info.ID});";
                if (await cmd.ExecuteNonQueryAsync() != 0)
                {
                    cmd.CommandText = $"delete from messages where (msg_from = {info.ID} and msg_To = {User_id}) or (msg_from = {User_id} and msg_To = {info.ID});";
                    await cmd.ExecuteNonQueryAsync();
                    LoadData();
                }
                else
                    MessageBox.Show("Error");
            }
        }
        async void Search_People()
        {
            cmd.CommandText = $"select * from (select user_id, user_name ,State from users_accounts left outer join friends on First_User_ID = {info.ID} and Second_User_ID = user_id) g where g.user_name like '%{textBox1.Text}%';";
            t.Clear();
            t.Load(await cmd.ExecuteReaderAsync());
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Search_People();
            
        }
        private void button4_Click(object sender, EventArgs e)
        {
            Search_People();
        }
    }
}
