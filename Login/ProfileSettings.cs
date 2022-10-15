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
    public partial class ProfileSettings : Form
    {
        UserInfo MyInfo;
        MySqlCommand cmd = new MySqlCommand();
        async void StartConnection()
        {
            cmd.CommandText = $"select first_name  ,lasst_name  , user_name , email ,password , phone from users_accounts where user_id = {MyInfo.ID} ;";
            try
            {
                MySqlDataReader r =(MySqlDataReader) await cmd.ExecuteReaderAsync();
                if (r.Read())
                {
                    FirsrName.Text = r[0].ToString();
                    LastName.Text = r[1].ToString();
                    UserName.Text = r[2].ToString();
                    Email.Text = r[3].ToString();
                    Pass.Text = r[4].ToString();
                    phone.Text = r[5].ToString();
                }
                r.Close();
            }
            catch{}
        }
        public ProfileSettings(ref UserInfo i)
        {
            cmd.Connection = Program.sqlCOnn;
            MyInfo = i;
            InitializeComponent();
            StartConnection();
        }
        bool check()
        {
            if (FirsrName.Text == "" || LastName.Text == "" || UserName.Text == "" || Email.Text == "" || Pass.Text == "" || phone.Text == "")
                return false;
            return true;
        }
        async void SaveChanges()
        {
            DialogResult d = MessageBox.Show("Do you want to save changes", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (d == DialogResult.Yes)
            {
                cmd.CommandText = $"update users_accounts set first_name = '{FirsrName.Text}',lasst_name = '{LastName.Text}',user_name = '{UserName.Text}',email = '{Email.Text}',password = '{Pass.Text}',phone = '{phone.Text}'where user_id = {MyInfo.ID};";
                try
                {
                    if (check())
                    {
                        if (await cmd.ExecuteNonQueryAsync() != 0)
                        {
                            MessageBox.Show("Saved Successfully!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            MyInfo.UserName = UserName.Text;
                            MyInfo.FirstName = FirsrName.Text;
                            MyInfo.LastName = LastName.Text;
                            MyInfo.Email = Email.Text;
                            MyInfo.Password = Pass.Text;
                            MyInfo.Phone = phone.Text;
                            Dispose();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Dont let any field empy", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch
                {
                    MessageBox.Show("The user name is already taken", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            SaveChanges();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Pass.UseSystemPasswordChar = !Pass.UseSystemPasswordChar;
        }
    }
}
