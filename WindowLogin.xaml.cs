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
        public WindowLogin()
        {
            InitializeComponent();
        }

        private void btnLoginSubmit_Click(object sender, RoutedEventArgs e)
        {
            ///conectare sql
            string connectionString = "Server=localhost;userid=root;password=;Database=crmagentie_db";
            MySqlConnection sqlCon = new MySqlConnection(connectionString);
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
                    MainWindow dashboard = new MainWindow();
                    dashboard.Show();
                    this.Close();
                }
                else {
                    MessageBox.Show("Wrong username or password!");
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
    }
}
