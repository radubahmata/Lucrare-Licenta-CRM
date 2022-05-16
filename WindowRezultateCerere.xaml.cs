using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace CRMAgentieImobiliara
{
    /// <summary>
    /// Interaction logic for WindowRezultateCerere.xaml
    /// </summary>
    public partial class WindowRezultateCerere : Window
    {
        string queryDemand;
        string connectionString = "SERVER=localhost;DATABASE=crmagentie_db;UID=root;PASSWORD=;";
        public WindowRezultateCerere(string queryRequest)
        {
            InitializeComponent();
            queryDemand = queryRequest;
            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand cmd = new MySqlCommand(queryDemand, connection);
            connection.Open();
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            connection.Close();
            proprietatiDataGrid.DataContext = dt;
        }

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataRowView row_selected = proprietatiDataGrid.SelectedItem as DataRowView;
            if (row_selected != null)
            {
                string idProprietate = row_selected["id_proprietate"].ToString();
                DetaliiActivitate windowDetaliiActivitate = new DetaliiActivitate(idProprietate);
                windowDetaliiActivitate.Show();
            }
        }
    }
}
