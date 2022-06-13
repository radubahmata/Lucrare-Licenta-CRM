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
    /// Interaction logic for WindowOperatiuni.xaml
    /// </summary>
    public partial class WindowOperatiuni : Window
    {
        string userId;

        public WindowOperatiuni(string idUser, MySqlConnection connection)
        {
            userId = idUser;
            string queryDemand = "SELECT * FROM `cash` WHERE `userID`=" + userId + " ORDER BY data";
            InitializeComponent();
            MySqlCommand cmd = new MySqlCommand(queryDemand, connection);
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            connection.Close();
            operatiuniDataGrid.DataContext = dt;
        }
    }
}
