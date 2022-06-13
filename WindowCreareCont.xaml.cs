using System;
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
    /// Interaction logic for WindowCreareCont.xaml
    /// </summary>
    public partial class WindowCreareCont : Window
    {
        MySqlConnection con;
        public WindowCreareCont(MySqlConnection connection)
        {
            con = connection;
            InitializeComponent();
        }

        private void btnRegisterSubmit_Click(object sender, RoutedEventArgs e)
        {
            using (con)
            {
                try
                {
                    using (var cmd = new MySqlCommand("INSERT INTO `login` ( `UserName`, `Password`, `Type`) VALUES (@UserName, @Password, 'user')"))
                    {
                        cmd.Connection = con;
                        string pass = txtloginPassword.Password.ToString();
                        string passConfirm = txtloginPasswordConfirm.Password.ToString();
                        if (pass == passConfirm)
                        {
                            cmd.Parameters.AddWithValue("@Password", pass);
                        }
                        else {
                            MessageBox.Show("Parolele nu coincid!");
                        }
                        
                        
                       
                        cmd.Parameters.AddWithValue("@UserName", txtloginUsername.Text.ToString());
                       

                        con.Open();
                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            MessageBox.Show("Cont de utilizator creat!");
                            WindowLogin window = new WindowLogin();
                            window.Show();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Crearea contului a esuat!");
                        }
                        con.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error during insert: " + ex.Message);
                }
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            WindowLogin window = new WindowLogin();
            window.Show();
            this.Close();
        }
    }
}
