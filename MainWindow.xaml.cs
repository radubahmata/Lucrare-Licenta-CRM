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

        private void activitatiDataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataRowView row_selected = activitatiDataGrid.SelectedItem as DataRowView;
            if (row_selected != null)
            {
                string idActivitate = row_selected["id"].ToString();
                DetaliiActivitate windowDetaliiActivitate = new DetaliiActivitate(idActivitate);
                windowDetaliiActivitate.Show();
               // DetailsWindow windowDetalii = new DetailsWindow(idDetalii);
               // windowDetalii.Show();
            }
        }

        private void btnAddActivitate_Click(object sender, RoutedEventArgs e)
        {
            WindowAddActivitate window = new WindowAddActivitate();
            window.Show();
        }

        private void btnRefreshActivitati_Click(object sender, RoutedEventArgs e)
        {
            string ConnectionString = "SERVER=localhost;DATABASE=crmagentie_db;UID=root;PASSWORD=;";
            MySqlConnection connection = new MySqlConnection(ConnectionString);
            MySqlCommand cmd = new MySqlCommand("select * from activitati", connection);
            connection.Open();
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            connection.Close();
            activitatiDataGrid.DataContext = dt;
        }

        /* private void activitatiDataGridRow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
         {

             DataRowView row_selected = activitatiDataGrid.SelectedItem as DataRowView;
             if (row_selected != null)
             {
                 string idActivitate = row_selected["id"].ToString();
                 btnEditActivitate.IsEnabled = true;
                 btnDelActivitate.IsEnabled = true;

                 string connectionstring = "SERVER=localhost;DATABASE=crmagentie_db;UID=root;PASSWORD=;";
                 MySqlConnection con = new MySqlConnection(connectionstring);
                 string query = "select * from activitati where id='" + idActivitate + "'";
                 MySqlCommand cmd = new MySqlCommand(query, con);
                 MySqlDataReader dr;

                 try
                 {
                     con.Open();
                     dr = cmd.ExecuteReader();
                     while (dr.Read())
                     {
                         cmbActivitate.Text = dr.GetString("tip");
                         string numeContact;
                         string prenumeContact;
                         string nrTel;
                         MySqlConnection conContact = new MySqlConnection(connectionstring);
                         string idContact = dr.GetString("id_contact").ToString();
                         string queryContact = "select * from contacte where id_contact='" + idContact + "'";
                         MySqlCommand cmdContact = new MySqlCommand(queryContact, conContact);
                         MySqlDataReader drContact;
                         conContact.Open();
                         drContact = cmdContact.ExecuteReader();
                         while (drContact.Read())
                         {
                             numeContact = drContact.GetString("nume");
                             prenumeContact = drContact.GetString("prenume");
                             nrTel = drContact.GetString("nr_tel");
                             cmbContact.Text = numeContact + " " + prenumeContact + "(ID=" + idContact + ")\n " + nrTel;
                         }
                         conContact.Close();

                         string idContact2 = dr.GetString("id_contact2").ToString();
                         string queryContact2 = "select * from contacte where id_contact='" + idContact2 + "'";
                         MySqlCommand cmdContact2 = new MySqlCommand(queryContact2, conContact);
                         MySqlDataReader drContact2;
                         conContact.Open();
                         drContact2 = cmdContact2.ExecuteReader();
                         while (drContact2.Read())
                         {
                             numeContact = drContact2.GetString("nume");
                             prenumeContact = drContact2.GetString("prenume");
                             nrTel = drContact2.GetString("nr_tel");
                             cmbContact2.Text = numeContact + " " + prenumeContact + "(ID=" + idContact2 + ")\n " + nrTel;
                         }
                         conContact.Close();
                     }
                 }
                 catch { }
             }
         }*/


    }
}

