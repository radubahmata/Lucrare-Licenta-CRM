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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Windows.Controls;

namespace CRMAgentieImobiliara
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    enum ActionState
    {
        New,
        Edit,
        Delete,
        Nothing
    }

    public partial class MainWindow : Window
    {
        //ActionState action = ActionState.Nothing;
       
        public MainWindow()
        {
            InitializeComponent();
            

            string ConnectionString = "SERVER=localhost;DATABASE=crmagentie_db;UID=root;PASSWORD=;";
            MySqlConnection connection = new MySqlConnection(ConnectionString);
            MySqlCommand cmd = new MySqlCommand("select * from proprietati", connection);
            connection.Open();
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            connection.Close();
            proprietatiDataGrid.DataContext = dt;
        }

        private void btnProprietateNoua_Click(object sender, RoutedEventArgs e)
        {
         //   action = ActionState.New;
            Window1 window = new Window1();
            window.Show();
        }

        private void btnProprietateEdit_Click(object sender, RoutedEventArgs e)
        {
            // action = ActionState.Edit;
            DataRowView row_selected = proprietatiDataGrid.SelectedItem as DataRowView;
            if (row_selected != null)
            {
                string idEditat = row_selected["id_proprietate"].ToString();
                WindowEdit window = new WindowEdit(idEditat);
                window.Show();
            }
            else {
                MessageBox.Show("Nu ati selectat nicio proprietate!");
            }
        }

        private void btnProprietateStergere_Click(object sender, RoutedEventArgs e)
        {
            //action = ActionState.Delete;
            DataRowView row_selected = proprietatiDataGrid.SelectedItem as DataRowView;
            if (row_selected != null)
            {
                string ConnectionString = "SERVER=localhost;DATABASE=crmagentie_db;UID=root;PASSWORD=;";
                MySqlConnection connection = new MySqlConnection(ConnectionString);
                string idSters = row_selected["id_proprietate"].ToString();
                string query = "DELETE from proprietati where id_proprietate='" + idSters + "'";
                MySqlCommand cmd = new MySqlCommand(query,connection);
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
                MessageBox.Show("Proprietate stearsa!");
                MySqlCommand refresh = new MySqlCommand("select * from proprietati", connection);
                connection.Open();
                DataTable dt = new DataTable();
                dt.Load(refresh.ExecuteReader());
                connection.Close();
                proprietatiDataGrid.DataContext = dt;

            }
            else
            {
                MessageBox.Show("Nu ati selectat nicio proprietate!");
            }

        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            string ConnectionString = "SERVER=localhost;DATABASE=crmagentie_db;UID=root;PASSWORD=;";
            MySqlConnection connection = new MySqlConnection(ConnectionString);
            MySqlCommand cmd = new MySqlCommand("select * from proprietati", connection);
            connection.Open();
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            connection.Close();
            proprietatiDataGrid.DataContext = dt;
        }

        private void btnViewContacte_Click(object sender, RoutedEventArgs e)
        {
            WindowContacte window = new WindowContacte();
            window.Show();

        }

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataRowView row_selected = proprietatiDataGrid.SelectedItem as DataRowView;
            if (row_selected != null)
            {
                string idDetalii = row_selected["id_proprietate"].ToString();
                DetailsWindow windowDetalii = new DetailsWindow(idDetalii);
                windowDetalii.Show();
            }
            
        }
    }
}

