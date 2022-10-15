using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using MySql.Data.MySqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Login
{
    public partial class CreateAccount : Form
    {
        int current = 0;
        MySqlCommand cmd = new MySqlCommand();
        string ImgLoc = "";
        async void StartConnection()
        {
            cmd.CommandText = "Select max(user_id) from users_accounts;";
            await cmd.ExecuteNonQueryAsync();
            MySqlDataReader r =(MySqlDataReader)await cmd.ExecuteReaderAsync();
            if (r.Read())
            {
                try { current = r.GetInt32(0); } catch { current = 0; }
            }
            r.Close();
        }
        public CreateAccount()
        {
            InitializeComponent();
            //panel1.BackColor = Color.FromArgb(100,Color.White);
            pictureBox2.BackColor = Color.White;
            UserType.SelectedIndex = 0;
            panel1.Parent = pictureBox1;
            panel1.Location = new Point(Width/2-panel1.Width/2, Height / 2 - panel1.Height / 2);
            button1.Location = new Point(panel1.Width/2-button1.Width/2,button1.Location.Y);
            label7.Parent = pictureBox1;
            label7.Location = new Point(Width/2-label7.Width/2,label7.Location.Y);
            cmd.Connection = Program.sqlCOnn;
            StartConnection();
        }
        async void Create_Account()
        {
            if (FirsrName.Text == "" || LastName.Text == "" || UserName.Text == "" || Email.Text == "" || phone.Text == "")
            {
                MessageBox.Show("Please Enter Full Data", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                cmd.CommandText = $"insert into users_accounts(user_id,first_name,lasst_name,user_name,email,password,phone,User_Type) values({++current},'{FirsrName.Text}','{LastName.Text}','{UserName.Text}','{Email.Text}','{Pass.Text}','{phone.Text}','{UserType.Text}');";
                await cmd.ExecuteNonQueryAsync();
                MessageBox.Show("Created Successfully", "successful operation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Program.f1.Show();
                Program.f1.textBox1.Select();
                Dispose();
            }
            catch (Exception)
            {
                MessageBox.Show("The user name is already exists please try again with different user name", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Create_Account();
        }

        private void LastName_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {

        }

        private void UserName_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
                Email.Select();
        }

        private void Email_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
                Pass.Select();
        }

        private void Pass_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
                phone.Select();
        }
        private void phone_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
                UserType.Select();
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.sqlCOnn.Close();
            Dispose();
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Program.f1.Show();
            Dispose();
        }

        private void FirsrName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Convert.ToInt32(e.KeyChar) == Convert.ToInt32(Keys.Enter) || Convert.ToInt32(e.KeyChar) == Convert.ToInt32(Keys.Tab))
            {
                e.Handled = true;
                LastName.Select();
            }
        }

        private void LastName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Convert.ToInt32(e.KeyChar) == Convert.ToInt32(Keys.Enter) || Convert.ToInt32(e.KeyChar) == Convert.ToInt32(Keys.Tab))
            {
                e.Handled = true; 
                UserName.Select();
            }
        }

        private void UserName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Convert.ToInt32(e.KeyChar) == Convert.ToInt32(Keys.Enter) || Convert.ToInt32(e.KeyChar) == Convert.ToInt32(Keys.Tab))
            {
                e.Handled = true;
                Email.Select(); 
            }
        }

        private void Email_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Convert.ToInt32(e.KeyChar) == Convert.ToInt32(Keys.Enter) || Convert.ToInt32(e.KeyChar) == Convert.ToInt32(Keys.Tab))
            {
                e.Handled = true;
                Pass.Select();
            }
        }

        private void Pass_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Convert.ToInt32(e.KeyChar) == Convert.ToInt32(Keys.Enter) || Convert.ToInt32(e.KeyChar) == Convert.ToInt32(Keys.Tab))
            {
                e.Handled = true;
                phone.Select();
            }
        }

        private void phone_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Convert.ToInt32(e.KeyChar) == Convert.ToInt32(Keys.Enter) || Convert.ToInt32(e.KeyChar) == Convert.ToInt32(Keys.Tab))
            {
                e.Handled = true;
                Create_Account();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog ChooseImg = new OpenFileDialog();
            ChooseImg.Filter = "png files (*.png)|*.png|jpg files(*.jpg)|*.jpg";
            if (ChooseImg.ShowDialog() == DialogResult.OK)
            {
                pictureBox2.ImageLocation = ChooseImg.FileName;
                ImgLoc = ChooseImg.FileName;
            }
        }

        
    }
}
