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
    /// Interaction logic for DetaliiActivitate.xaml
    /// </summary>
    public partial class DetaliiActivitate : Window
    {
        string idDetalii;
        public DetaliiActivitate(string idActivitate)
        {
            InitializeComponent();
            idDetalii = idActivitate;

            string connectionstring = "SERVER=localhost;DATABASE=crmagentie_db;UID=root;PASSWORD=;";
            MySqlConnection con = new MySqlConnection(connectionstring);
            string query = "select * from activitati where id='" + idDetalii + "'";
            MySqlCommand cmd = new MySqlCommand(query, con);
            MySqlDataReader dr;

            try
            {
                con.Open();
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    txtTitle.Text = dr.GetString("tip").ToString().ToUpper();
                    txtData.Text = dr.GetDateTime("data").ToString("dddd, dd-MM-yyyy HH:mm");
                    txtProprietate.Text = "ID="+dr.GetString("id_proprietate").ToString();
                    txtDetalii.Text = dr.GetString("detalii").ToString();
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
                        txtContact.Text = numeContact + " " + prenumeContact + "(ID=" + idContact + ")\n " + nrTel;
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
                        txtContact2.Text = numeContact + " " + prenumeContact + "(ID=" + idContact2 + ")\n " + nrTel;
                    }
                    conContact.Close();

                   
                }
            }
            catch { }
        }
    }
}

