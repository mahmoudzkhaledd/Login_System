using MySql.Data.MySqlClient;
namespace Login
{
    internal static class Program
    {
       // public static MySqlConnection sqlCOnn = new MySqlConnection("User Id=root;Password=;Host=localhost;Database=accounts;");
        public static MySqlConnection sqlCOnn = new MySqlConnection("Server=sql5.freemysqlhosting.net;Port=3306;Database=sql5503353;Uid=sql5503353;Pwd=reLfP9RUI3;");
      
        public static LoginWindow f1 = new LoginWindow();
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(f1);
        }
    }
}