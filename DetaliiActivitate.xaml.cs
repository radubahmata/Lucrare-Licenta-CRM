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
using MySql.Data.MySqlClient;

namespace CRMAgentieImobiliara
{
    /// <summary>
    /// Interaction logic for DetaliiActivitate.xaml
    /// </summary>
    public partial class DetaliiActivitate : Window
    {
        string idDetalii;
        string connectionString = "Server=localhost;userid=root;password=;Database=crmagentie_db";
       
        public DetaliiActivitate(string idActivitate, MySqlConnection connection)
        {
            MySqlConnection conContact = new MySqlConnection(connectionString);
            InitializeComponent();
            idDetalii = idActivitate;
            string query = "select * from activitati where id='" + idDetalii + "'";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataReader dr;

            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    txtTitle.Text = dr.GetString("tip").ToString().ToUpper();
                    txtData.Text = dr.GetDateTime("data").ToString("dddd, dd-MM-yyyy HH:mm");
                    txtDetalii.Text = dr.GetString("detalii").ToString();
                    string numeContact;
                    string prenumeContact;
                    string nrTel;
                    //conContact = connection;
                    if (!dr.IsDBNull(dr.GetOrdinal("id_contact")))
                    {
                        string idContact = dr.GetInt32("id_contact").ToString();
                        if (idContact != "0")
                        {
                            string queryContact = "select * from contacte where id_contact='" + idContact + "'";
                            MySqlCommand cmdContact = new MySqlCommand(queryContact, conContact);
                            MySqlDataReader drContact;
                            if (conContact.State == ConnectionState.Closed)
                                conContact.Open();
                            drContact = cmdContact.ExecuteReader();
                            while (drContact.Read())
                            {
                                numeContact = drContact.GetString("nume");
                                prenumeContact = drContact.GetString("prenume");
                                nrTel = drContact.GetString("nr_tel");
                                txtContact.Text = numeContact + " " + prenumeContact + "(ID=" + idContact + ")\n " + nrTel;
                            }
                            drContact.Close();
                            conContact.Close();
                        }
                    }
                    
                   
                    if (!dr.IsDBNull(dr.GetOrdinal("id_contact2")))
                    {
                        string idContact2 = dr.GetInt32("id_contact2").ToString();
                        if (idContact2 != "0")
                        {
                            string queryContact2 = "select * from contacte where id_contact='" + idContact2 + "'";
                            MySqlCommand cmdContact2 = new MySqlCommand(queryContact2, conContact);
                            MySqlDataReader drContact2;
                            if (conContact.State == ConnectionState.Closed)
                                conContact.Open();
                            drContact2 = cmdContact2.ExecuteReader();
                            while (drContact2.Read())
                            {
                                numeContact = drContact2.GetString("nume");
                                prenumeContact = drContact2.GetString("prenume");
                                nrTel = drContact2.GetString("nr_tel");
                                txtContact2.Text = numeContact + " " + prenumeContact + "(ID=" + idContact2 + ")\n " + nrTel;
                            }
                            drContact2.Close();
                            conContact.Close();
                        }
                    }
                    if (!dr.IsDBNull(dr.GetOrdinal("id_proprietate"))) 
                    {
                        txtProprietate.Text = "ID=" + dr.GetString(("id_proprietate")).ToString();
                    }
            
                }
                dr.Close();
                connection.Close();
            }
            catch (Exception ex) {
               MessageBox.Show(ex.Source.ToString());
            }
        }
    }
}

