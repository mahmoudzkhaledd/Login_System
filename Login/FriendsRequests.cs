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
    public partial class FriendsRequests : Form
    {
        UserInfo Info;
        int Selected_User_Id = 0;
        string Selected_User_Name = "";
        MySqlCommand cmd = new MySqlCommand();
        public FriendsRequests(UserInfo i)
        {
            cmd.Connection = Program.sqlCOnn;
            Info = i;
            InitializeComponent();
            LoadData();
        }
        async void LoadData()
        {
            listBox1.DataSource = null;
            listBox1.Items.Clear();
            cmd .CommandText = $"select user_id , user_name from users_accounts join  friends on user_id = Second_User_ID and State = 'Pending' and First_User_ID = {Info.ID}; ";
            DataTable t = new DataTable();
            t.Load(await cmd.ExecuteReaderAsync());
            listBox1.DataSource = t;
            listBox1.DisplayMember = "user_name";
            listBox1.ValueMember = "user_id";
            if(listBox1.Items.Count == 0)
            {
                button1.Enabled = false;
                button2.Enabled = false;
            }else
            {
                button1.Enabled = true;
                button2.Enabled = true;
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            Dispose();
        }
        private void listBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                if (listBox1.SelectedValue.ToString() != "System.Data.DataRowView")
                {
                    Selected_User_Id = Convert.ToInt32(listBox1.SelectedValue.ToString());
                    Selected_User_Name = listBox1.Text;
                }
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            cmd .CommandText = $"update friends set State ='Accepted' where (First_User_ID = {Info.ID} and Second_User_ID = {Selected_User_Id}) or (First_User_ID = {Selected_User_Id} and Second_User_ID = {Info.ID});";
            if(await cmd.ExecuteNonQueryAsync() != 0)
                LoadData();
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            DialogResult d = MessageBox.Show("Are you sure that you want to reject request ?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (d == DialogResult.Yes)
            {
                cmd.CommandText = $"delete from friends where (First_User_ID = {Info.ID} and Second_User_ID = {Selected_User_Id} ) or (First_User_ID = {Selected_User_Id} and Second_User_ID = {Info.ID});";
                if (await cmd.ExecuteNonQueryAsync() != 0)
                {
                    cmd.CommandText = $"delete from messages where (msg_from = {Info.ID} and msg_To = {Selected_User_Id}) or (msg_from = {Selected_User_Id} and msg_To = {Info.ID});";
                    await cmd.ExecuteNonQueryAsync();
                    LoadData();
                }
                else
                    MessageBox.Show("Error");
            }
        }
    }
}
