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
        string connectionString = "Server=localhost;userid=root;password=;Database=crmagentie_db";
        string userId; 
        int userIdInt;
        MySqlConnection con;
        public WindowLogin()
        {
            InitializeComponent();
            con = new MySqlConnection(connectionString);
        }

        private void btnLoginSubmit_Click(object sender, RoutedEventArgs e)
        {  
            
            MySqlCommand cmd;
            MySqlDataReader dr;
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                String query = "select count(1) from login where Username=@Username and Password=@Password";
                cmd = new MySqlCommand(query, con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Username", txtloginUsername.Text);
                cmd.Parameters.AddWithValue("@Password", txtloginPassword.Password);
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                if (count == 1)
                {
                    cmd = new MySqlCommand("select * from login where `UserName`='"+txtloginUsername.Text+"'", con);
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
                    con.Close();
                    MainWindow dashboard = new MainWindow(userId, userIdInt, con);
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
                con.Close();
            }
           
        }

        private void btnCreateAccount_Click(object sender, RoutedEventArgs e)
        {
            WindowCreareCont window = new WindowCreareCont(con);
            window.Show();
            this.Close();
        }
    }
}
