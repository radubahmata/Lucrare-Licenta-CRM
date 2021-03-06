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
using System.Windows.Threading;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace CRMAgentieImobiliara
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        MySqlConnection con;
        string userId;
        int userIdInt;
        public MainWindow(string idUser, int IdUserInt, MySqlConnection connection)
        {
            con = connection;
            InitializeComponent();
            userId = idUser;
            userIdInt = IdUserInt;
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            string queryProprietati = "select * from proprietati where userId=" + userId + " AND stadiu ='activa'";
            MySqlCommand cmd = new MySqlCommand(queryProprietati, connection);
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            connection.Close();
            proprietatiDataGrid.DataContext = dt;
            MySqlCommand cmdActivitati = new MySqlCommand("SELECT id, tip, id_contact, id_proprietate, data, detalii, stadiu from activitati WHERE userId="+userId+" AND data>='"+DateTime.Now.ToString("yyyy-MM-dd")+"' ORDER BY data", connection);
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            DataTable dtActivitati = new DataTable();
            dtActivitati.Load(cmdActivitati.ExecuteReader());
            connection.Close();
            activitatiDataGrid.DataContext = dtActivitati;
        }

        private void btnProprietateNoua_Click(object sender, RoutedEventArgs e)
        {
            string request = "proprietate";
            WindowConfirmContact window = new WindowConfirmContact(request, userId, userIdInt, con);
            window.Show();
        }

        private void btnProprietateEdit_Click(object sender, RoutedEventArgs e)
        {
            DataRowView row_selected = proprietatiDataGrid.SelectedItem as DataRowView;
            if (row_selected != null)
            {
                string idEditat = row_selected["id_proprietate"].ToString();
                WindowEdit window = new WindowEdit(idEditat, con);
                window.Show();
            }
            else {
                MessageBox.Show("Nu ati selectat nicio proprietate!");
            }
        }

        private void btnProprietateStergere_Click(object sender, RoutedEventArgs e)
        {
            DataRowView row_selected = proprietatiDataGrid.SelectedItem as DataRowView;
            if (row_selected != null)
            {
                string idSters = row_selected["id_proprietate"].ToString();
                string query = "DELETE from proprietati where id_proprietate='" + idSters + "'";
                MySqlCommand cmd = new MySqlCommand(query,con);
                if (con.State == ConnectionState.Closed)
                    con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Proprietate stearsa!");
                MySqlCommand refresh = new MySqlCommand("select * from proprietati WHERE userId=" + userId + "", con);
                if (con.State == ConnectionState.Closed)
                    con.Open();
                DataTable dt = new DataTable();
                dt.Load(refresh.ExecuteReader());
                con.Close();
                proprietatiDataGrid.DataContext = dt;
            }
            else
            {
                MessageBox.Show("Nu ati selectat nicio proprietate!");
            }
        }

        private void CmbStadiuProp_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string queryProprietatiCombo;
            string stadiu = (sender as ComboBox).SelectedItem.ToString();
            if (stadiu == "System.Windows.Controls.ComboBoxItem: Active") queryProprietatiCombo = "select * from proprietati where userId=" + userId + " AND stadiu ='activa'";
            else if (stadiu == "System.Windows.Controls.ComboBoxItem: Pierdute") queryProprietatiCombo = "select * from proprietati where userId=" + userId + " AND stadiu ='pierduta'";
            else queryProprietatiCombo = "select * from proprietati where userId=" + userId + " AND stadiu ='retrasa'";

            MySqlCommand cmd = new MySqlCommand(queryProprietatiCombo, con);
            if (con.State == ConnectionState.Closed)
                con.Open();
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            con.Close();
            proprietatiDataGrid.DataContext = dt;
        }

        private void btnViewContacte_Click(object sender, RoutedEventArgs e)
        {
            WindowContacte window = new WindowContacte(userIdInt, con);
            window.Show();          
        }

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataRowView row_selected = proprietatiDataGrid.SelectedItem as DataRowView;
            if (row_selected != null)
            {
                string idDetalii = row_selected["id_proprietate"].ToString();
                DetailsWindow windowDetalii = new DetailsWindow(idDetalii, con);
                windowDetalii.Show();
            }            
        }

        private void activitatiDataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataRowView row_selected = activitatiDataGrid.SelectedItem as DataRowView;
            if (row_selected != null)
            {
                string idActivitate = row_selected["id"].ToString();
                DetaliiActivitate windowDetaliiActivitate = new DetaliiActivitate(idActivitate, con);
                windowDetaliiActivitate.Show();
            }
        }

        private void btnAddActivitate_Click(object sender, RoutedEventArgs e)
        {
            string request = "activitate";
            WindowConfirmContact window = new WindowConfirmContact(request, userId, userIdInt, con);
            window.Show();
        }

        private void btnEditActivitate_Click(object sender, RoutedEventArgs e)
        {
            
            DataRowView row_selected = activitatiDataGrid.SelectedItem as DataRowView;
            if (row_selected != null)
            {
                string idEditat = row_selected["id"].ToString();
                WindowEditActivitate windowEditActivitate = new WindowEditActivitate(idEditat, con);
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
                string idSters = row_selected["id"].ToString();
                string query = "DELETE from activitati where id='" + idSters + "'";
                MySqlCommand cmd = new MySqlCommand(query, con);
                if (con.State == ConnectionState.Closed)
                    con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Activitate stearsa!");
                MySqlCommand refresh = new MySqlCommand("SELECT id, tip, id_contact, id_proprietate, data, detalii, stadiu from activitati WHERE userId=" + userId + " AND data>='" + DateTime.Now.ToString("yyyy-MM-dd") + "' ORDER BY data", con);
                if (con.State == ConnectionState.Closed)
                    con.Open();
                DataTable dt = new DataTable();
                dt.Load(refresh.ExecuteReader());
                con.Close();
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
            MySqlCommand cmdActivitatiOneDay = new MySqlCommand("SELECT id, tip, id_contact, id_proprietate, data, detalii, stadiu from activitati WHERE userId=" + userId + " AND data>='" + astazi.ToString("yyyy-MM-dd") + "' AND data<'"+maine.ToString("yyyy-MM-dd")+ "' AND stadiu='viitoare' ORDER BY data", con);
            if (con.State == ConnectionState.Closed)
                con.Open();
            DataTable dtActivitatiSpecific = new DataTable();
            dtActivitatiSpecific.Load(cmdActivitatiOneDay.ExecuteReader());
            con.Close();
            activitatiDataGrid.DataContext = dtActivitatiSpecific;
        }

        private void btnOneWeek_Click(object sender, RoutedEventArgs e)
        {
            txtIntro.Text = "Activitatile din urmatoarele 7 zile:";
            var astazi = DateTime.Today;
            var oneWeek = astazi.AddDays(8);
            MySqlCommand cmdActivitatiOneDay = new MySqlCommand("SELECT id, tip, id_contact, id_proprietate, data, detalii, stadiu from activitati WHERE userId=" + userId + " AND data>='" + astazi.ToString("yyyy-MM-dd") + "' AND data<'" + oneWeek.ToString("yyyy-MM-dd") + "' AND stadiu='viitoare' ORDER BY data", con);
            if (con.State == ConnectionState.Closed)
                con.Open();
            DataTable dtActivitatiSpecific = new DataTable();
            dtActivitatiSpecific.Load(cmdActivitatiOneDay.ExecuteReader());
            con.Close();
            activitatiDataGrid.DataContext = dtActivitatiSpecific;

        }

        private void btnOneMonth_Click(object sender, RoutedEventArgs e)
        {
            txtIntro.Text = "Activitatile din urmatoarele 30 de zile:";
            var astazi = DateTime.Today;
            var oneMonth = astazi.AddDays(31);
            MySqlCommand cmdActivitatiOneDay = new MySqlCommand("SELECT id, tip, id_contact, id_proprietate, data, detalii, stadiu from activitati WHERE userId=" + userId + " AND data>='" + astazi.ToString("yyyy-MM-dd") + "' AND data<'" + oneMonth.ToString("yyyy-MM-dd") + "' AND stadiu='viitoare' ORDER BY data", con);
            if (con.State == ConnectionState.Closed)
                con.Open();
            DataTable dtActivitatiSpecific = new DataTable();
            dtActivitatiSpecific.Load(cmdActivitatiOneDay.ExecuteReader());
            con.Close();
            activitatiDataGrid.DataContext = dtActivitatiSpecific;
        }

        private void btnViitoareActivitati_Click(object sender, RoutedEventArgs e)
        {
            txtIntro.Text = "Toate activitatile viitoare:";
            MySqlCommand cmd = new MySqlCommand("SELECT id, tip, id_contact, id_proprietate, data, detalii, stadiu from activitati WHERE userId=" + userId + " AND stadiu='viitoare' AND data >= '" + DateTime.Now.ToString("yyyy-MM-dd") + "' ORDER BY data", con);
            if (con.State == ConnectionState.Closed)
                con.Open();
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            con.Close();
            activitatiDataGrid.DataContext = dt;
        }

        private void btnToateActivitati_Click(object sender, RoutedEventArgs e)
        {
            txtIntro.Text = "Toate activitatile:";
            MySqlCommand cmd = new MySqlCommand("SELECT * from activitati WHERE userId=" + userId + " ORDER BY data", con);
            if (con.State == ConnectionState.Closed)
                con.Open();
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            con.Close();
            activitatiDataGrid.DataContext = dt;
        }

        private void btnRaportActivitati_Click(object sender, RoutedEventArgs e)
        {
            string dataStart = "", dataEnd = "";
            if (dpStartRaport.SelectedDate != null)
            {
                dataStart = dpStartRaport.SelectedDate.Value.ToString("yyyy-MM-dd");
            }
            else dataStart = "1753-01-01";
            if (dpFinalRaport.SelectedDate != null)
            {
                dataEnd = dpFinalRaport.SelectedDate.Value.ToString("yyyy-MM-dd");
            }
            else dataEnd = "9999-12-31";
            string queryActTotale = "SELECT COUNT(*) FROM activitati where userId=" + userId + " and data>='" + dataStart + "' and data<='" + dataEnd + "'";
            string queryActFinalizate = "SELECT COUNT(*) FROM activitati where userId=" + userId + " and stadiu='finalizata' and data>='" + dataStart + "' and data<='" + dataEnd + "'";
            string queryActAnulate = "SELECT COUNT(*) FROM activitati where userId=" + userId + " and stadiu='anulata' and data>='" + dataStart + "' and data<='" + dataEnd + "'";
            string queryActAmanata = "SELECT COUNT(*) FROM activitati where userId=" + userId + " and stadiu='amanata' and data>='" + dataStart + "' and data<='" + dataEnd + "'";

            MySqlCommand cmd = new MySqlCommand(queryActFinalizate, con);
            if (con.State == ConnectionState.Closed)
                con.Open();
            var ActFinalizate = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();

            cmd.CommandText = queryActAnulate;
            if (con.State == ConnectionState.Closed)
                con.Open();
            var ActAnulate = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();

            cmd.CommandText = queryActAmanata;
            if (con.State == ConnectionState.Closed)
                con.Open();
            var ActAmanate = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();

            cmd.CommandText = queryActTotale;
            if (con.State == ConnectionState.Closed)
                con.Open();
            var ActTotale = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
            double procentFinalizare = Convert.ToDouble(ActFinalizate) / Convert.ToDouble(ActTotale) * 100;

            txtActivitatiFinalizate.Text = ActFinalizate + " activitati finalizate";
            txtActivitatiAnulate.Text = ActAnulate + " activitati anulate";
            txtActivitatiAmanate.Text = ActAmanate + " activitati amanate";
            txtProcentajFinalizare.Text = "Grad finalizare activitati " + procentFinalizare + "%";
        }

        void fillComboLocalitate()
        {
            if (con.State == ConnectionState.Closed)
                con.Open();
            string query = "select localitate, zona from proprietati WHERE userId=" + userId + "";
            MySqlCommand createCommand = new MySqlCommand(query, con);
            MySqlDataReader dr = createCommand.ExecuteReader();
            int iLocalitati = -1;
            int nrLoc = 0;
            string[] localitati = new string[400];
            while (dr.Read())
            {
                string localitate = dr.GetString("localitate");
                if (iLocalitati == -1)
                {
                    iLocalitati++;
                    nrLoc = iLocalitati+1;
                    localitati[iLocalitati] = localitate;  
                }
                else
                {
                    int identic = 0;
                    for (int j = 0; j < nrLoc; j++)
                    {
                        if (localitati[j] == localitate) identic = 1;
                    }
                    if (identic == 0)
                    {
                        iLocalitati++;
                        nrLoc = iLocalitati+1;
                        localitati[iLocalitati] = localitate;
                    }
                }
            }
            dr.Close();
            con.Close();
            for (iLocalitati = 0; iLocalitati < nrLoc; iLocalitati++) cmbLocalitate.Items.Add(localitati[iLocalitati]);
        }

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            string queryRequest = "SELECT * FROM proprietati ";
            int n = 0;
            string tranzactie, localitate, zona, compartimentare;
            int pretMin, pretMax, sMin, sMax;
            int etajMin, etajMax, nrCamMin, nrCamMax;
            tranzactie = cmbTranzactie.Text.ToString();
            if (tranzactie != null && tranzactie != "")
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
                else
                {
                    queryRequest += " AND localitate='" + localitate + "'";
                    n++;
                }
            }
            zona = cmbZona.Text.ToString();
            if (zona != null && zona != "")
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
                    n++;
                }
                else
                {
                    queryRequest += " AND pret>='" + pretMin + "'";
                    n++;
                }
            }
            if (int.TryParse(txtPretMax.Text, out pretMax))
            {
                if (n == 0)
                {
                    queryRequest += " WHERE pret<='" + pretMax + "'";
                    n++;
                }
                else
                {
                    queryRequest += " AND pret<='" + pretMax + "'";
                    n++;
                }
            }
            if (int.TryParse(txtSupMin.Text, out sMin))
            {
                if (n == 0)
                {
                    queryRequest += " WHERE suprafata_utila>='" + sMin + "'";
                    n++;
                }
                else
                {
                    queryRequest += " AND suprafata_utila>='" + sMin + "'";
                    n++;
                }
            }
            if (int.TryParse(txtSupMax.Text, out sMax))
            {
                if (n == 0)
                {
                    queryRequest += " WHERE suprafata_utila<='" + sMax + "'";
                    n++;
                }
                else
                {
                    queryRequest += " AND suprafata_utila<='" + sMax + "'";
                    n++;
                }
            }
            if (int.TryParse(txtEtajMin.Text, out etajMin))
            {
                if (n == 0)
                {
                    queryRequest += " WHERE etaj>='" + etajMin + "'";
                    n++;
                }
                else
                {
                    queryRequest += " AND etaj>='" + etajMin + "'";
                    n++;
                }
            }
            if (int.TryParse(txtEtajMax.Text, out etajMax))
            {
                if (n == 0)
                {
                    queryRequest += " WHERE etaj<='" + etajMax + "'";
                    n++;
                }
                else
                {
                    queryRequest += " AND etaj<='" + etajMax + "'";
                    n++;
                }
            }
            compartimentare = cmbCompartimentare.Text.ToString();
            if (compartimentare != null && compartimentare != "")
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
                    n++;
                }
                else
                {
                    queryRequest += " AND nr_camere>='" + nrCamMin + "'";
                    n++;
                }
            }
            if (int.TryParse(txtNrCamMax.Text, out nrCamMax))
            {
                if (n == 0)
                {
                    queryRequest += " WHERE nr_camere<='" + nrCamMax + "'";
                    n++;
                }
                else
                {
                    queryRequest += " AND nr_camere<='" + nrCamMax + "'";
                    n++;
                }
            }
            if (n == 0)
                queryRequest += " WHERE stadiu='activa'";
            else queryRequest += " AND stadiu='activa'";
            WindowRezultateCerere window = new WindowRezultateCerere(queryRequest, con);
            window.Show();

        }

        void fillComboLocalitateStatistici()
        {
            if (con.State == ConnectionState.Closed)
                con.Open();
            string query = "select localitate, zona from proprietati WHERE userId=" + userId + "";
            MySqlCommand createCommand = new MySqlCommand(query, con);
            MySqlDataReader dr = createCommand.ExecuteReader();
            int iLocalitati = -1;
            int nrLoc = 0;
            string[] localitati = new string[400];
            while (dr.Read())
            {
                string localitate = dr.GetString("localitate");
                if (iLocalitati == -1)
                {
                    iLocalitati++;
                    nrLoc = iLocalitati + 1;
                    localitati[iLocalitati] = localitate;
                }
                else
                {
                    int identic = 0;
                    for (int j = 0; j < nrLoc; j++)
                    {
                        if (localitati[j] == localitate) identic = 1;
                    }
                    if (identic == 0)
                    {
                        iLocalitati++;
                        nrLoc = iLocalitati + 1;
                        localitati[iLocalitati] = localitate;
                    }
                }
            }
            dr.Close();
            for (iLocalitati = 0; iLocalitati < nrLoc; iLocalitati++) cmbLocalitateStatistici.Items.Add(localitati[iLocalitati]);
        }

        private void cmbLocalitate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cmbZona.IsEnabled = true;
            cmbZona.Items.Clear();
            string localitate = (sender as ComboBox).SelectedItem as string;
            if (con.State == ConnectionState.Closed)
                con.Open();
            string query = "select zona from proprietati where userId=" + userId + " AND localitate='" + localitate + "'";
            MySqlCommand createCommand = new MySqlCommand(query, con);
            MySqlDataReader dr = createCommand.ExecuteReader();
            int iZone = -1;
            int nrZon = 0;
            string[] zone = new string[1200];
            while (dr.Read())
            {
                string zona = dr.GetString("zona");
                if (iZone == -1)
                {
                    iZone++;
                    nrZon = iZone+1;
                    zone[iZone] = zona;
                }
                else
                {
                    int identic = 0;
                    for (int j = 0; j < nrZon; j++)
                    {
                        if (zone[j] == zona) identic = 1;
                    }
                    if (identic == 0)
                    {
                        iZone++;
                        nrZon = iZone+1;
                        zone[iZone] = zona;

                    }
                }
            }
            dr.Close();
            for (iZone = 0; iZone < nrZon; iZone++) cmbZona.Items.Add(zone[iZone]);
            con.Close();
        }
        
        private void Cereri_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            fillComboLocalitate();
            cmbLocalitateStatistici.Items.Clear();
        }

        private void ManagerActivitati_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            cmbLocalitate.Items.Clear();
            cmbLocalitateStatistici.Items.Clear();
        }

        private void lblPropContacte_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            cmbLocalitate.Items.Clear();
            cmbLocalitateStatistici.Items.Clear();
            MySqlCommand cmd = new MySqlCommand("select * from proprietati where userId=" + userId + " AND stadiu ='activa'", con);
            if (con.State == ConnectionState.Closed)
                con.Open();
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            con.Close();
            proprietatiDataGrid.DataContext = dt; 
        }

        private void lblStatistici_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            cmbLocalitate.Items.Clear();
            fillComboLocalitateStatistici();
            string queryVanzareActiv = "SELECT suprafata_utila, pret from proprietati where tip_oferta ='vanzare' and stadiu = 'activa' and localitate='Cluj-Napoca'";
            string queryVanzareTranzactionat = "SELECT suprafata_utila, pret_tranzactionare, data_tranzactionare from proprietati where tip_oferta ='vanzare' and stadiu = 'tranzactionata' and localitate='Cluj-Napoca'";
            string queryInchiriereActiv = "SELECT nr_camere, pret from proprietati where tip_oferta ='inchiriere' and stadiu = 'activa' and localitate='Cluj-Napoca'";
            string queryInchiriereTranz = "SELECT nr_camere, pret_tranzactionare from proprietati where tip_oferta ='inchiriere' and stadiu = 'tranzactionata' and localitate='Cluj-Napoca'";
            if (con.State == ConnectionState.Closed)
                con.Open();
            MySqlCommand cmdVanzareActiv = new MySqlCommand(queryVanzareActiv, con);
            MySqlDataReader drVanzareActiv = cmdVanzareActiv.ExecuteReader();

            double suma = 0, nr=0, pret=0, pretMediu=0, suprafataUtila=0, pretMediuCurent=0;
            while (drVanzareActiv.Read())
            {
                pret = drVanzareActiv.GetDouble("pret");
                suprafataUtila = drVanzareActiv.GetDouble("suprafata_utila");
                pretMediuCurent = pret / suprafataUtila;
                suma = suma + pretMediuCurent;
                nr++;
            }
            pretMediu = suma / nr;
            txtVanzariActive.Text = Math.Round(pretMediu, 2).ToString();
            con.Close();
            suma = 0;
            nr = 0;
            if (con.State == ConnectionState.Closed)
                con.Open();
            MySqlCommand cmdVanzareTranzactionat = new MySqlCommand(queryVanzareTranzactionat, con);
            MySqlDataReader drVanzareTranzactionat = cmdVanzareTranzactionat.ExecuteReader();

            while (drVanzareTranzactionat.Read())
            {
                pret = drVanzareTranzactionat.GetDouble("pret_tranzactionare");
                suprafataUtila = drVanzareTranzactionat.GetDouble("suprafata_utila");
                pretMediuCurent = pret / suprafataUtila;
                suma = suma + pretMediuCurent;
                nr++;
            }
            pretMediu = suma / nr;
            txtPropVandute.Text = Math.Round(pretMediu, 2).ToString();
            con.Close();
            if (con.State == ConnectionState.Closed)
                con.Open();
            MySqlCommand cmdInchiriereActiv = new MySqlCommand(queryInchiriereActiv, con);
            MySqlDataReader drInchiriereActiv = cmdInchiriereActiv.ExecuteReader();

            suma = 0;
            nr = 0;
            int nrCamere;
            while (drInchiriereActiv.Read())
            {
                pret = drInchiriereActiv.GetDouble("pret");
                nrCamere = drInchiriereActiv.GetInt32("nr_camere");
                pretMediuCurent = pret / nrCamere;
                suma = suma + pretMediuCurent;
                nr++;
            }
            pretMediu = suma / nr;
            txtChiriiActive.Text = Math.Round(pretMediu, 2).ToString();
            con.Close();
            if (con.State == ConnectionState.Closed)
                con.Open();
            MySqlCommand cmdInchiriereTranz = new MySqlCommand(queryInchiriereTranz, con);
            MySqlDataReader drInchiriereTranz = cmdInchiriereTranz.ExecuteReader();

            suma = 0;
            nr = 0;
            while (drInchiriereTranz.Read())
            {
                pret = drInchiriereTranz.GetDouble("pret_tranzactionare");
                nrCamere = drInchiriereTranz.GetInt32("nr_camere");
                pretMediuCurent = pret / nrCamere;
                suma = suma + pretMediuCurent;
                nr++;
            }
            pretMediu = suma / nr;
            txtPropInchiriate.Text = Math.Round(pretMediu, 2).ToString();
            con.Close();
        }

       
       
        private void cmbLocalitateStatistici_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string localitate = (sender as ComboBox).SelectedItem as string;
            string queryVanzareActiv = "SELECT suprafata_utila, pret from proprietati where tip_oferta ='vanzare' and stadiu = 'activa' and localitate='"+localitate+"'";
            string queryVanzareTranzactionat = "SELECT suprafata_utila, pret_tranzactionare, data_tranzactionare from proprietati where tip_oferta ='vanzare' and stadiu = 'tranzactionata' and localitate='" + localitate + "'";
            string queryInchiriereActiv = "SELECT nr_camere, pret from proprietati where tip_oferta ='inchiriere' and stadiu = 'activa' and localitate='" + localitate + "'";
            string queryInchiriereTranz = "SELECT nr_camere, pret_tranzactionare from proprietati where tip_oferta ='inchiriere' and stadiu = 'tranzactionata' and localitate='" + localitate + "'";
            if (con.State == ConnectionState.Closed)
                con.Open();
            MySqlCommand cmdVanzareActiv = new MySqlCommand(queryVanzareActiv, con);
            MySqlDataReader drVanzareActiv = cmdVanzareActiv.ExecuteReader();

            double suma = 0, nr = 0, pret = 0, pretMediu = 0, suprafataUtila = 0, pretMediuCurent = 0;
            while (drVanzareActiv.Read())
            {
                pret = drVanzareActiv.GetDouble("pret");
                suprafataUtila = drVanzareActiv.GetDouble("suprafata_utila");
                pretMediuCurent = pret / suprafataUtila;
                suma = suma + pretMediuCurent;
                nr++;
            }
            pretMediu = suma / nr;
            txtVanzariActive.Text = Math.Round(pretMediu, 2).ToString();
            drVanzareActiv.Close();
            con.Close();
            suma = 0;
            nr = 0;

            if (con.State == ConnectionState.Closed)
                con.Open();
            MySqlCommand cmdVanzareTranzactionat = new MySqlCommand(queryVanzareTranzactionat, con);
            MySqlDataReader drVanzareTranzactionat = cmdVanzareTranzactionat.ExecuteReader();

            while (drVanzareTranzactionat.Read())
            {
                pret = drVanzareTranzactionat.GetDouble("pret_tranzactionare");
                suprafataUtila = drVanzareTranzactionat.GetDouble("suprafata_utila");
                pretMediuCurent = pret / suprafataUtila;
                suma = suma + pretMediuCurent;
                nr++;
            }
            pretMediu = suma / nr;
            txtPropVandute.Text = Math.Round(pretMediu, 2).ToString();
            drVanzareTranzactionat.Close();
            con.Close();
            if (con.State == ConnectionState.Closed)
                con.Open();
            MySqlCommand cmdInchiriereActiv = new MySqlCommand(queryInchiriereActiv, con);
            MySqlDataReader drInchiriereActiv = cmdInchiriereActiv.ExecuteReader();

            suma = 0;
            nr = 0;
            int nrCamere;
            while (drInchiriereActiv.Read())
            {
                pret = drInchiriereActiv.GetDouble("pret");
                nrCamere = drInchiriereActiv.GetInt32("nr_camere");
                pretMediuCurent = pret / nrCamere;
                suma = suma + pretMediuCurent;
                nr++;
            }
            pretMediu = suma / nr;
            txtChiriiActive.Text = Math.Round(pretMediu, 2).ToString();
            drInchiriereActiv.Close();
            con.Close();
            if (con.State == ConnectionState.Closed)
                con.Open();
            MySqlCommand cmdInchiriereTranz = new MySqlCommand(queryInchiriereTranz, con);
            MySqlDataReader drInchiriereTranz = cmdInchiriereTranz.ExecuteReader();

            suma = 0;
            nr = 0;
            while (drInchiriereTranz.Read())
            {
                pret = drInchiriereTranz.GetDouble("pret_tranzactionare");
                nrCamere = drInchiriereTranz.GetInt32("nr_camere");
                pretMediuCurent = pret / nrCamere;
                suma = suma + pretMediuCurent;
                nr++;
            }
            pretMediu = suma / nr;
            txtPropInchiriate.Text = Math.Round(pretMediu, 2).ToString();
            drInchiriereTranz.Close();
            con.Close();
        }

        private void btnVenit_Click(object sender, RoutedEventArgs e)
        {
            using (con)
            {
                try
                {
                    using (var cmd = new MySqlCommand("INSERT INTO `cash` ( `operatie` , `suma` , `data`, `userId`) VALUES ( 'venit', @suma, @data, "+userIdInt+")"))
                    {
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@suma", Convert.ToDouble(txtVenit.Text));
                        cmd.Parameters.AddWithValue("@data", dpVenit.SelectedDate.Value.ToString("yyyy-MM-dd"));
                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            MessageBox.Show("Venit adaugat!");
                        }
                        else
                        {
                            MessageBox.Show("Adaugarea venitului a esuat!");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void btnCheltuiala_Click(object sender, RoutedEventArgs e)
        {
            using (con)
            {
                try
                {
                    using (var cmd = new MySqlCommand("INSERT INTO `cash` ( `operatie` , `suma` , `data`, `userId`) VALUES ( 'cheltuiala', @suma, @data, "+userIdInt+" )"))
                    {
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@suma", Convert.ToDouble(txtCheltuiala.Text));
                        cmd.Parameters.AddWithValue("@data", dpCheltuiala.SelectedDate.Value.ToString("yyyy-MM-dd"));

                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            MessageBox.Show("Cheltuiala adaugata!");
                        }
                        else
                        {
                            MessageBox.Show("Adaugarea cheltuielii a esuat!");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnCalcTotal_Click(object sender, RoutedEventArgs e)
        {
            if (con.State == ConnectionState.Closed)
                con.Open();
            string query = "select * from cash WHERE userId=" + userId + "";
            MySqlCommand createCommand = new MySqlCommand(query, con);
            MySqlDataReader dr = createCommand.ExecuteReader();

            double venituri = 0, cheltuieli = 0, profit;
            while (dr.Read())
            {
                string operatie = dr.GetString("operatie");
                if (operatie == "venit")
                {
                    double venit = dr.GetDouble("suma");
                    venituri += venit;
                }
                else {
                    double cheltuiala = dr.GetDouble("suma");
                    cheltuieli += cheltuiala;
                }
            }
            dr.Close();
            profit = venituri - cheltuieli;
            txtProfitTotal.Text = profit.ToString();
        }

        private void btnCalcPeriodic_Click(object sender, RoutedEventArgs e)
        {
            if (con.State == ConnectionState.Closed)
                con.Open();
            string query;
            string dataStart="", dataEnd="";
            if (dpStart.SelectedDate != null)
            {
                dataStart = dpStart.SelectedDate.Value.ToString("yyyy-MM-dd");
            }
            else dataStart = DBNull.Value.ToString();
            if (dpEnd.SelectedDate != null)
            {
                dataEnd = dpEnd.SelectedDate.Value.ToString("yyyy-MM-dd");
            }
            else dataEnd = "9999-12-31";
            
            double venituri = 0, cheltuieli = 0, profitPer;
            
            if (dataStart != "")
            {
                if (dataEnd != "")
                {
                    query = "select operatie, suma from cash where userId=" + userId + " AND data>='" + dataStart + "' and data<='" + dataEnd + "'";
                    MySqlCommand cmd = new MySqlCommand(query, con);
                    MySqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        string operatie = dr.GetString("operatie");
                        if (operatie == "venit")
                        {
                            double venit = dr.GetDouble("suma");
                            venituri += venit;
                        }
                        else
                        {
                            double cheltuiala = dr.GetDouble("suma");
                            cheltuieli += cheltuiala;
                        }

                        profitPer = venituri - cheltuieli;
                        txtProfitPeriodic.Text = profitPer.ToString();
                    }dr.Close();
                }
                else
                {
                    query = "select operatie, suma from cash where userId=" + userId + " AND data>='" + dataStart + "'";
                    MySqlCommand cmd = new MySqlCommand(query, con);
                    MySqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        string operatie = dr.GetString("operatie");
                        if (operatie == "venit")
                        {
                            double venit = dr.GetDouble("suma");
                            venituri += venit;
                        }
                        else
                        {
                            double cheltuiala = dr.GetDouble("suma");
                            cheltuieli += cheltuiala;
                        }

                        profitPer = venituri - cheltuieli;
                        txtProfitPeriodic.Text = profitPer.ToString();
                    }dr.Close();
                }
            }
            else {
                query = "select operatie, suma from cash WHERE userId=" + userId + "";
                MySqlCommand cmd = new MySqlCommand(query, con);
                MySqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    string operatie = dr.GetString("operatie");
                    if (operatie == "venit")
                    {
                        double venit = dr.GetDouble("suma");
                        venituri += venit;
                    }
                    else
                    {
                        double cheltuiala = dr.GetDouble("suma");
                        cheltuieli += cheltuiala;
                    }

                    profitPer = venituri - cheltuieli;
                    txtProfitPeriodic.Text = profitPer.ToString();
                }dr.Close();
            }
        }

        private void btnTranzactii_Click(object sender, RoutedEventArgs e)
        {
            WindowTranzactii window = new WindowTranzactii(userId, con);
            window.Show();
        }

        private void btnOperatiuni_Click(object sender, RoutedEventArgs e)
        {
            WindowOperatiuni window = new WindowOperatiuni(userId, con);
            window.Show();
        }
    }
}

