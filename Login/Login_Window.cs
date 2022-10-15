using MySql.Data.MySqlClient;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks;

namespace Login
{
    public partial class LoginWindow : Form
    {
        async void connect()
        {
           await Program.sqlCOnn.OpenAsync();

            textBox1.Enabled = true;
            textBox2.Enabled = true;
            button1.Enabled = true;
            textBox1.Select();
        }
        public LoginWindow()
        {
            InitializeComponent();

            textBox1.Enabled = false;
            textBox2.Enabled = false;
            button1.Enabled = false;

            label3.Parent = pictureBox1;
            label3.Location = new Point(Width / 2 - label3.Width / 2, label3.Location.Y);
            panel1.Parent = pictureBox1;
            panel1.Location = new Point(Width / 2 - panel1.Width / 2, Height / 2 - panel1.Height / 2);
            button1.Location = new Point(panel1.Width / 2 - button1.Width / 2,button1.Location.Y+5);
            linkLabel1.Location = new Point(panel1.Width / 2 - linkLabel1.Width / 2, linkLabel1.Location.Y);
            connect();
        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CreateAccount f2 = new CreateAccount();
            f2.Show();
            this.Hide();
            textBox1.Clear();
            textBox2.Clear();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.sqlCOnn.Close();
        }

       async void Login()
        {

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = Program.sqlCOnn;
            cmd.CommandText = $"Select * from users_accounts where user_name = '{textBox1.Text}'";
            await Task.Run(() => cmd.ExecuteNonQuery());
            MySqlDataReader r = cmd.ExecuteReader();
            if (r.Read())
            {
                if (r[5].ToString() == textBox2.Text)
                {
                    if (r[7].ToString() == "Client")
                    {
                        UserPage p = new UserPage(new UserInfo(r));
                        r.Close();
                        p.Show();
                        this.Hide();

                    }
                    else
                    {
                        String x = r[3].ToString();
                        r.Close();
                        AdminPage Ad = new AdminPage(x);
                        Ad.Show();
                        this.Hide();
                    }
                    textBox1.Clear();
                    textBox2.Clear();
                }
            }
            else
            {
                MessageBox.Show("User Name or password is incorrect", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox1.Select();
            }
            r.Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Login();
            
            textBox1.Select();
        }

        

        private  void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(Convert.ToInt32(e.KeyChar) == 13)
            {
                e.Handled = true;
                Login();

            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Convert.ToInt32(e.KeyChar) == 13)
            {
                e.Handled = true;
                textBox2.Select();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void todoCheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            textBox2.UseSystemPasswordChar = !textBox2.UseSystemPasswordChar;
        }
    }
}