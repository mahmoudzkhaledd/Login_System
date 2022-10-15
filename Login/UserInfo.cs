using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
namespace Login
{
    public class UserInfo
    {
        public int ID = 0;
        public string FirstName = "";
        public string LastName = "";
        public string UserName = "";
        public string Email = "";
        public string Password = "";
        public string Phone = "";
        public string UserType = "";
        public UserInfo(MySqlDataReader r)
        {
            
            ID = Convert.ToInt32(r[0]);
            FirstName = r[1].ToString();
            LastName = r[2].ToString();
            UserName = r[3].ToString();
            Email = r[4].ToString();
            Password = r[5].ToString();
            UserType = r[6].ToString();
            r.Close();
        }
    }
}
