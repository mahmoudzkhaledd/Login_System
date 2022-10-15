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
    public partial class UserPage : Form
    {
        public UserInfo MyInfo;
        DataTable tF = new DataTable();
        DataTable t = new DataTable();
        MySqlCommand cmd = new MySqlCommand();
        private async void button5_Click_1(object sender, EventArgs e)
        {
            DialogResult r = MessageBox.Show("Are you sure that you want to delete all messages ? ", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (r == DialogResult.Yes)
            {
                try
                {
                    cmd.CommandText = $"delete from messages where (msg_from = {MyInfo.ID} and msg_To = {listBox1.SelectedValue}) or (msg_from = {listBox1.SelectedValue} and msg_To = {MyInfo.ID}) ;";
                    if (await cmd.ExecuteNonQueryAsync() != 0)
                        MessageLstBox.Items.Clear();
                }
                catch {}
            }
            Welcome.Location = new Point(Welcome.Parent.Width / 2 - Welcome.Width / 2, Welcome.Parent.Height / 2 - Welcome.Height / 2);
        }
        public UserPage(UserInfo info)
        {
            cmd.Connection = Program.sqlCOnn;
            InitializeComponent();
            MyInfo = info;
            listBox1.DataSource = tF;
            LoadFriendList();
        }

        async void LoadFriendList()
        {
            Welcome.Text = "Welcome " + MyInfo.UserName;
            //listBox1.DataSource = null;
            //listBox1.Items.Clear();
            cmd.CommandText = $"select f.Second_User_ID , a.user_name from users_accounts a join friends f on f.second_User_ID = a.user_id and first_User_ID ={MyInfo.ID} and State = 'Accepted';";
            
            tF.Clear();
            tF.Load(await cmd.ExecuteReaderAsync());
            
            listBox1.DisplayMember = "user_name";
            listBox1.ValueMember = "Second_User_ID";
            if (listBox1.Items.Count > 0)
                listBox1.SelectedIndex = 0;
            if (MessageLstBox.Items.Count > 0)
                MessageLstBox.SelectedIndex = MessageLstBox.Items.Count - 1;
            textBox2.Text = "";
            Welcome.Location = new Point(Welcome.Parent.Width / 2 - Welcome.Width / 2, Welcome.Parent.Height / 2 - Welcome.Height / 2);
        }
        async void SelectItem()
        {
            MessageLstBox.Items.Clear();
            if (listBox1.SelectedValue != null)
            {
                cmd.CommandText = $"select msg_from, msg_content ,msg_To from messages m where (msg_from = {listBox1.SelectedValue.ToString()} or msg_To = {listBox1.SelectedValue.ToString()}) and (msg_To = {MyInfo.ID} or msg_from = {MyInfo.ID}) ;";
                t.Clear();
                t.Load(await cmd.ExecuteReaderAsync());
                for (int i = 0; i < t.Rows.Count; i++)
                {
                    if (t.Rows[i][0].ToString() == MyInfo.ID.ToString())
                        MessageLstBox.Items.Add("You : " + t.Rows[i][1].ToString());
                    else
                        MessageLstBox.Items.Add(listBox1.Text + " : " + t.Rows[i][1].ToString());
                }
            }
            if (MessageLstBox.Items.Count > 0)
                MessageLstBox.SelectedIndex = MessageLstBox.Items.Count - 1;
            textBox1.Select();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Program.f1.Show();
            Dispose();
        }

        private void UserPage_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.sqlCOnn.Close();
            Dispose();
            Application.Exit();
        }

        async void SendMessage()
        {
            if (textBox1.Text != "")
            {
                int prev = 0;
                int currentMsgID = 0;

                cmd.CommandText = "select max(msg_id) from messages;";
                MySqlDataReader re;
                re = (MySqlDataReader)cmd.ExecuteReader();
                try
                {
                    if (re.Read())
                        currentMsgID = re.GetInt32(0);
                }
                catch { }
                re.Close();

                if (listBox1.SelectedValue != null)
                {
                    cmd.CommandText = $"insert into messages(msg_id,msg_from,msg_To,msg_content,State) values({currentMsgID + 1},{MyInfo.ID},{listBox1.SelectedValue.ToString()},'{textBox1.Text}','Sent');";
                   
                    if (await cmd.ExecuteNonQueryAsync() != 0)
                        MessageLstBox.Invoke((MethodInvoker)delegate { MessageLstBox.Items.Add("You : " + textBox1.Text); });
                   
                    textBox1.Text = "";
                    MessageLstBox.ScrollAlwaysVisible = true;
                    if (MessageLstBox.Items.Count > 0)
                        MessageLstBox.SelectedIndex = MessageLstBox.Items.Count - 1;
                }
            }
            textBox1.Select();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            SendMessage();
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (listBox1.SelectedItem != null)
                {
                    if (listBox1.Items.Count > 0 && listBox1.SelectedValue.ToString() != "System.Data.DataRowView")
                    {
                        SelectItem();
                    }
                }
                else MessageLstBox.Items.Clear();
            }
            catch (Exception ess){}
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Convert.ToInt32(e.KeyChar) == 13)
            {
                e.Handled = true;
                SendMessage();
            }
        }

        private void listBox1_ControlAdded(object sender, ControlEventArgs e)
        {
            MessageBox.Show(e.ToString());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            PeopleList p = new PeopleList(MyInfo);
            p.ShowDialog();
            LoadFriendList();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FriendsRequests f = new FriendsRequests(MyInfo);
            f.ShowDialog();
            LoadFriendList();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            LoadFriendList();
            SelectItem();
            textBox2.Text = "";
        }

        private void MessageLstBox_Click(object sender, EventArgs e)
        {
            LoadFriendList();
            SelectItem();
            
        }
        private void button7_Click(object sender, EventArgs e)
        {
            ProfileSettings p = new ProfileSettings(ref MyInfo);
            p.ShowDialog();
            LoadFriendList();
            Welcome.Location = new Point(Welcome.Parent.Width / 2 - Welcome.Width / 2, Welcome.Parent.Height / 2 - Welcome.Height / 2);
        }
        async void Search_People()
        {
            cmd.CommandText = $"select * from (select f.Second_User_ID , a.user_name from users_accounts a join friends f on f.Second_User_ID = a.user_id and First_User_ID = {MyInfo.ID} and State = 'Accepted') g where g.user_name like '%{textBox2.Text}%';";
            tF.Clear();
            tF.Load(await cmd.ExecuteReaderAsync());
            SelectItem();
            textBox2.Select();
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            Search_People();
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Convert.ToInt32(e.KeyChar) == 13)
            {
                e.Handled = true;
                textBox1.Select();
            }
        }
    }
}
