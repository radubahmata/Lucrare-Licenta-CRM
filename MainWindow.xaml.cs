using System;
using System.Data;
using System.Collections.Generic;
using System.Globalization;
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

            MySqlCommand cmdActivitati = new MySqlCommand("SELECT id, tip, id_contact, id_proprietate, data, detalii, stadiu from activitati WHERE data>='"+DateTime.Now.ToString("yyyy-MM-dd")+"' ORDER BY data", connection);
            connection.Open();
            DataTable dtActivitati = new DataTable();
            dtActivitati.Load(cmdActivitati.ExecuteReader());
            connection.Close();
            activitatiDataGrid.DataContext = dtActivitati;

           

            //MessageBox.Show(DateTime.Now.ToString("dddd, dd-MM-yyyy HH:mm"));
        }

       

        

        private void btnProprietateNoua_Click(object sender, RoutedEventArgs e)
        {
            //   action = ActionState.New;
            string request = "proprietate";
            WindowConfirmContact window = new WindowConfirmContact(request);
            window.Show();
           // Window1 window = new Window1();
            //window.Show();
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

        private void activitatiDataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataRowView row_selected = activitatiDataGrid.SelectedItem as DataRowView;
            if (row_selected != null)
            {
                string idActivitate = row_selected["id"].ToString();
                DetaliiActivitate windowDetaliiActivitate = new DetaliiActivitate(idActivitate);
                windowDetaliiActivitate.Show();
            }
        }

        private void btnAddActivitate_Click(object sender, RoutedEventArgs e)
        {
            string request = "activitate";
            WindowConfirmContact window = new WindowConfirmContact(request);
            window.Show();
        }

        private void btnRefreshActivitati_Click(object sender, RoutedEventArgs e)
        {
            string ConnectionString = "SERVER=localhost;DATABASE=crmagentie_db;UID=root;PASSWORD=;";
            MySqlConnection connection = new MySqlConnection(ConnectionString);
            MySqlCommand cmd = new MySqlCommand("SELECT id, tip, id_contact, id_proprietate, data, detalii, stadiu from activitati WHERE data >= '"+DateTime.Now.ToString("yyyy-MM-dd")+"' ORDER BY data", connection);
            connection.Open();
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            connection.Close();
            activitatiDataGrid.DataContext = dt;
        }

        private void btnEditActivitate_Click(object sender, RoutedEventArgs e)
        {
            
            DataRowView row_selected = activitatiDataGrid.SelectedItem as DataRowView;
            if (row_selected != null)
            {
                string idEditat = row_selected["id"].ToString();
                WindowEditActivitate windowEditActivitate = new WindowEditActivitate(idEditat);
                windowEditActivitate.Show();
            }
            else
            {
                MessageBox.Show("Nu ati selectat nicio activitate!");
            }
        }

        private void btnDelActivitate_Click(object sender, RoutedEventArgs e)
        {
            DataRowView row_selected = activitatiDataGrid.SelectedItem as DataRowView;
            if (row_selected != null)
            {
                string ConnectionString = "SERVER=localhost;DATABASE=crmagentie_db;UID=root;PASSWORD=;";
                MySqlConnection connection = new MySqlConnection(ConnectionString);
                string idSters = row_selected["id"].ToString();
                string query = "DELETE from activitati where id='" + idSters + "'";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
                MessageBox.Show("Activitate stearsa!");
                MySqlCommand refresh = new MySqlCommand("SELECT id, tip, id_contact, id_proprietate, data, detalii, stadiu from activitati WHERE data>='" + DateTime.Now.ToString("yyyy-MM-dd") + "' ORDER BY data", connection);
                connection.Open();
                DataTable dt = new DataTable();
                dt.Load(refresh.ExecuteReader());
                connection.Close();
                activitatiDataGrid.DataContext = dt;

            }
            else
            {
                MessageBox.Show("Nu ati selectat nicio activitate!");
            }
        }

        private void btnToday_Click(object sender, RoutedEventArgs e)
        {
            txtIntro.Text = "Activitatile de astazi:";
            var astazi = DateTime.Today;
            var maine = astazi.AddDays(1);
            var ieri = astazi.AddDays(-1);
            string ConnectionString = "SERVER=localhost;DATABASE=crmagentie_db;UID=root;PASSWORD=;";
            MySqlConnection connection = new MySqlConnection(ConnectionString);
            MySqlCommand cmdActivitatiOneDay = new MySqlCommand("SELECT id, tip, id_contact, id_proprietate, data, detalii, stadiu from activitati WHERE data>='" + astazi.ToString("yyyy-MM-dd") + "' AND data<'"+maine.ToString("yyyy-MM-dd")+ "' AND stadiu='viitoare' ORDER BY data", connection);
            connection.Open();
            DataTable dtActivitatiSpecific = new DataTable();
            dtActivitatiSpecific.Load(cmdActivitatiOneDay.ExecuteReader());
            connection.Close();
            activitatiDataGrid.DataContext = dtActivitatiSpecific;
        }

        private void btnOneWeek_Click(object sender, RoutedEventArgs e)
        {
            txtIntro.Text = "Activitatile din urmatoarele 7 zile:";
            var astazi = DateTime.Today;
            var oneWeek = astazi.AddDays(8);
            string ConnectionString = "SERVER=localhost;DATABASE=crmagentie_db;UID=root;PASSWORD=;";
            MySqlConnection connection = new MySqlConnection(ConnectionString);
            MySqlCommand cmdActivitatiOneDay = new MySqlCommand("SELECT id, tip, id_contact, id_proprietate, data, detalii, stadiu from activitati WHERE data>='" + astazi.ToString("yyyy-MM-dd") + "' AND data<'" + oneWeek.ToString("yyyy-MM-dd") + "' AND stadiu='viitoare' ORDER BY data", connection);
            connection.Open();
            DataTable dtActivitatiSpecific = new DataTable();
            dtActivitatiSpecific.Load(cmdActivitatiOneDay.ExecuteReader());
            connection.Close();
            activitatiDataGrid.DataContext = dtActivitatiSpecific;

        }

        private void btnOneMonth_Click(object sender, RoutedEventArgs e)
        {
            txtIntro.Text = "Activitatile din urmatoarele 30 de zile:";
            var astazi = DateTime.Today;
            var oneMonth = astazi.AddDays(31);
            string ConnectionString = "SERVER=localhost;DATABASE=crmagentie_db;UID=root;PASSWORD=;";
            MySqlConnection connection = new MySqlConnection(ConnectionString);
            MySqlCommand cmdActivitatiOneDay = new MySqlCommand("SELECT id, tip, id_contact, id_proprietate, data, detalii, stadiu from activitati WHERE data>='" + astazi.ToString("yyyy-MM-dd") + "' AND data<'" + oneMonth.ToString("yyyy-MM-dd") + "' AND stadiu='viitoare' ORDER BY data", connection);
            connection.Open();
            DataTable dtActivitatiSpecific = new DataTable();
            dtActivitatiSpecific.Load(cmdActivitatiOneDay.ExecuteReader());
            connection.Close();
            activitatiDataGrid.DataContext = dtActivitatiSpecific;
        }

        private void btnViitoareActivitati_Click(object sender, RoutedEventArgs e)
        {
            txtIntro.Text = "Toate activitatile viitoare:";
            string ConnectionString = "SERVER=localhost;DATABASE=crmagentie_db;UID=root;PASSWORD=;";
            MySqlConnection connection = new MySqlConnection(ConnectionString);
            MySqlCommand cmd = new MySqlCommand("SELECT id, tip, id_contact, id_proprietate, data, detalii, stadiu from activitati WHERE data >= '" + DateTime.Now.ToString("yyyy-MM-dd") + "' ORDER BY data", connection);
            connection.Open();
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            connection.Close();
            activitatiDataGrid.DataContext = dt;
        }

        private void btnToateActivitati_Click(object sender, RoutedEventArgs e)
        {
            txtIntro.Text = "Toate activitatile:";
            string ConnectionString = "SERVER=localhost;DATABASE=crmagentie_db;UID=root;PASSWORD=;";
            MySqlConnection connection = new MySqlConnection(ConnectionString);
            MySqlCommand cmd = new MySqlCommand("SELECT * from activitati ORDER BY data", connection);
            connection.Open();
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            connection.Close();
            activitatiDataGrid.DataContext = dt;
        }

        void fillComboLocalitate()
        {
            String connectionString = "SERVER=localhost;DATABASE=crmagentie_db;UID=root;PASSWORD=;";
            MySqlConnection con = new MySqlConnection(connectionString);
            
           
                con.Open();
                string query = "select localitate, zona from proprietati";
                MySqlCommand createCommand = new MySqlCommand(query, con);
                MySqlDataReader dr = createCommand.ExecuteReader();
               
                int i = -1;
                int nr = 0;
                string[] localitati = new string[400];
                while (dr.Read())
                {
                    string localitate = dr.GetString("localitate");
                    if (i == -1)
                    {
                        i++;
                        nr= i;
                        localitati[i] = localitate;
                    }
                    else
                    {
                        int identic = 0;
                        for (int j = 0; j < nr; j++)
                        {
                            if (localitati[j] == localitate) identic = 1;
                        }
                        if (identic == 0)
                        {
                            i++;
                            nr = i;
                            localitati[i] = localitate;
                       
                        }
                    }    
                }
            for (i = 1; i <= nr; i++) cmbLocalitate.Items.Add(localitati[i]);
                con.Close();
           
        }
        private void Cereri_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            fillComboLocalitate();
        }

        private void ManagerActivitati_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            cmbLocalitate.Items.Clear();
        }

        private void lblPropContacte_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            cmbLocalitate.Items.Clear();
        }

        private void lblStatistici_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            cmbLocalitate.Items.Clear();
        }

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            string queryRequest = "SELECT * FROM proprietati";
            int n = 0;
            string tranzactie,localitate,zona, compartimentare;
            int pretMin, pretMax, sMin, sMax;
            int etajMin, etajMax, nrCamMin,nrCamMax;
            tranzactie = cmbTranzactie.Text.ToString();
            if (tranzactie != null && tranzactie!="")
            {
                queryRequest = queryRequest + " WHERE tip_oferta='" + tranzactie + "'";
                n++;
            }
            localitate = cmbLocalitate.Text.ToString();
            if (localitate != null && localitate != "")
            {
                if (n == 0)
                {
                    queryRequest = queryRequest + " WHERE localitate='" + localitate + "'";
                    n++;
                }
                else {
                    queryRequest += " AND localitate='" + localitate + "'";
                    n++;
                } 
            }
            zona = cmbZona.Text.ToString();
            if (zona != null && zona!="")
            {
                if (n == 0)
                {
                    queryRequest = queryRequest + " WHERE zona='" + zona + "'";
                    n++;
                }
                else
                {
                    queryRequest += " AND zona='" + zona + "'";
                    n++;
                }
            }
            if (int.TryParse(txtPretMin.Text, out pretMin)) 
            {
                if (n == 0)
                {
                    queryRequest += " WHERE pret>='" + pretMin + "'";
                }
                else {
                    queryRequest += " AND pret>='" + pretMin + "'";
                }
            }
            if (int.TryParse(txtPretMax.Text, out pretMax))
            {
                if (n == 0)
                {
                    queryRequest += " WHERE pret<='" + pretMax + "'";
                }
                else {
                    queryRequest += " AND pret<='" + pretMax + "'";
                }
            }
            if (int.TryParse(txtSupMin.Text, out sMin)) 
            {
                if (n == 0)
                {
                    queryRequest += " WHERE suprafata_utila>='" + sMin + "'";
                }
                else
                {
                    queryRequest += " AND suprafata_utila>='" + sMin + "'";
                }
            }
            if (int.TryParse(txtSupMax.Text, out sMax))
            {
                if (n == 0)
                {
                    queryRequest += " WHERE suprafata_utila<='" + sMax + "'";
                }
                else
                {
                    queryRequest += " AND suprafata_utila<='" + sMax + "'";
                }
            }
            if (int.TryParse(txtEtajMin.Text, out etajMin))
            {
                if (n == 0)
                {
                    queryRequest += " WHERE etaj>='" + etajMin + "'";
                }
                else
                {
                    queryRequest += " AND etaj>='" + etajMin + "'";
                }
            }
            if (int.TryParse(txtEtajMax.Text, out etajMax))
            {
                if (n == 0)
                {
                    queryRequest += " WHERE etaj<='" + etajMax + "'";
                }
                else
                {
                    queryRequest += " AND etaj<='" + etajMax + "'";
                }
            }
            compartimentare = cmbCompartimentare.Text.ToString();
            if (compartimentare != null && compartimentare!="")
            {
                if (n == 0)
                {
                    queryRequest = queryRequest + " WHERE compartimentare='" + compartimentare + "'";
                    n++;
                }
                else
                {
                    queryRequest += " AND compartimentare='" + compartimentare + "'";
                    n++;
                }
            }
            if (int.TryParse(txtNrCamMin.Text, out nrCamMin))
            {
                if (n == 0)
                {
                    queryRequest += " WHERE nr_camere>='" + nrCamMin + "'";
                }
                else
                {
                    queryRequest += " AND nr_camere>='" + nrCamMin + "'";
                }
            }
            if (int.TryParse(txtNrCamMax.Text, out nrCamMax))
            {
                if (n == 0)
                {
                    queryRequest += " WHERE nr_camere<='" + nrCamMax + "'";
                }
                else
                {
                    queryRequest += " AND nr_camere<='" + nrCamMax + "'";
                }
            }
            MessageBox.Show(queryRequest);

            WindowRezultateCerere window = new WindowRezultateCerere(queryRequest);
            window.Show();

        }

       
    }
}

