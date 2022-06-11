using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;


namespace CRMAgentieImobiliara
{
    /// <summary>
    /// Interaction logic for WindowLogin.xaml
    /// </summary>
    public partial class WindowLogin : Window
    {
        string userId; 
        int userIdInt;
        public WindowLogin()
        {
            InitializeComponent();
        }

        private void btnLoginSubmit_Click(object sender, RoutedEventArgs e)
        {
            ///conectare sql
            string connectionString = "Server=localhost;userid=root;password=;Database=crmagentie_db";
            MySqlConnection sqlCon = new MySqlConnection(connectionString);
            MySqlCommand cmd;
            MySqlDataReader dr;
            try
            {
                if (sqlCon.State == ConnectionState.Closed)
                    sqlCon.Open();
                String query = "select count(1) from login where Username=@Username and Password=@Password";
                MySqlCommand sqlCmd = new MySqlCommand(query, sqlCon);
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.Parameters.AddWithValue("@Username", txtloginUsername.Text);
                sqlCmd.Parameters.AddWithValue("@Password", txtloginPassword.Password);
                int count = Convert.ToInt32(sqlCmd.ExecuteScalar());
                if (count == 1)
                {
                    cmd = new MySqlCommand("select * from login where `UserName`='"+txtloginUsername.Text+"'", sqlCon);
                    try
                    {
                        dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            userId=dr.GetString("UserId");
                            userIdInt = Convert.ToInt32(userId);
                            string tip = dr.GetString("Type");
                            if (tip == "administrator") userId = "`userID`";
                        }
                    }
                    catch(Exception ex) { MessageBox.Show(ex.Message); }
                    MainWindow dashboard = new MainWindow(userId, userIdInt);
                    dashboard.Show();
                    this.Close();
                }
                else {
                    MessageBox.Show("Nume de utilizator sau parola incorecte!");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally {
                sqlCon.Close();
            }
           
        }

        private void btnCreateAccount_Click(object sender, RoutedEventArgs e)
        {
            WindowCreareCont window = new WindowCreareCont();
            window.Show();
            this.Close();
        }
    }
}
