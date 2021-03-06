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
    /// Interaction logic for WindowEditActivitate.xaml
    /// </summary>
    public partial class WindowEditActivitate : Window
    {
        string connectionString = "Server=localhost;userid=root;password=;Database=crmagentie_db";
        string idActivitateEdit;
        MySqlConnection con;
        

        public WindowEditActivitate(string idEditat, MySqlConnection connection)
        {
            con = connection;
            InitializeComponent();
            idActivitateEdit = idEditat;
            fillComboIdContact();
            fillComboIdProp(); 
            string query = "select * from activitati JOIN contacte ON activitati.id_contact=contacte.id_contact where id='" + idEditat + "';";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataReader dr;
            MySqlConnection conContact = new MySqlConnection(connectionString);
            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    cmbActivitate.Text = dr.GetString("tip").ToString();
                    txtDetalii.Text = dr.GetString("detalii");
                    cmbStadiu.Text = dr.GetString("stadiu");
                    dtpData.Value = Convert.ToDateTime(dr.GetDateTime("data").ToString());
                    if (!dr.IsDBNull(dr.GetOrdinal("id_proprietate")))
                    {
                        cmbIdProp.Text = dr.GetString("id_proprietate");
                    }
                    if (!dr.IsDBNull(dr.GetOrdinal("id_contact")))
                    {
                        string idContact = dr.GetString("id_contact");
                        string queryContact = "select * from contacte where id_contact='" + idContact + "'";
                        
                        MySqlCommand cmdJoinContact = new MySqlCommand(queryContact, conContact);
                        if (conContact.State == ConnectionState.Closed)
                            conContact.Open();
                        MySqlDataReader drContact = cmdJoinContact.ExecuteReader();
                        while (drContact.Read())
                        {
                            String Id = drContact.GetString("id_contact");
                            String nume = drContact.GetString("nume");
                            String prenume = drContact.GetString("prenume");
                            cmbContact.Text = Id + " " + nume + " " + prenume;

                        }
                        drContact.Close();
                        conContact.Close();
                    }
                   
                    if (!dr.IsDBNull(dr.GetOrdinal("id_contact2")))
                    {
                        string idContact2 = dr.GetString("id_contact2");
                        string queryContact = "select * from contacte where id_contact='" + idContact2 + "'";
                        MySqlCommand cmdJoinContact2 = new MySqlCommand(queryContact, conContact);
                        if (conContact.State == ConnectionState.Closed)
                            conContact.Open();
                        MySqlDataReader drContact2 = cmdJoinContact2.ExecuteReader();
                        while (drContact2.Read())
                        {
                            String Id2 = drContact2.GetString("id_contact");
                            String nume2 = drContact2.GetString("nume");
                            String prenume2 = drContact2.GetString("prenume");
                            cmbContact2.Text = Id2 + " " + nume2 + " " + prenume2;

                        }
                        drContact2.Close();
                    }
                    conContact.Close();
                }
                dr.Close();
                connection.Close();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnEditActivitate_Click(object sender, RoutedEventArgs e)
        {
            using (con)
            {
                try
                {
                    using (var cmd = new MySqlCommand("UPDATE `activitati` SET `tip`=@tip, `id_contact`=@idContact, `id_contact2`=@idContact2, `id_proprietate`=@idProprietate, `data`=@data, `detalii`=@detalii, `stadiu`=@stadiu where id='" + idActivitateEdit + "'", con))
                    {
                        cmd.Connection = con;
                        string s = cmbContact.Text.ToString();
                        string idContact, idContact2;
                        int index = s.IndexOf(' ');
                        cmd.Parameters.AddWithValue("@data", dtpData.Text);
                        cmd.Parameters.AddWithValue("@detalii", txtDetalii.Text);
                        cmd.Parameters.AddWithValue("@tip", cmbActivitate.Text);
                        cmd.Parameters.AddWithValue("@stadiu", cmbStadiu.Text);

                        if (index > 0)
                        {
                            idContact = s.Substring(0, index);
                            cmd.Parameters.AddWithValue("@idContact", Convert.ToInt32(idContact));
                        }
                        else
                        {
                            idContact = null;
                            cmd.Parameters.AddWithValue("@idContact", DBNull.Value);

                        }

                        s = cmbContact2.Text.ToString();
                        index = s.IndexOf(' ');
                        if (index > 0)
                        {
                            idContact2 = s.Substring(0, index);
                            cmd.Parameters.AddWithValue("@idContact2", Convert.ToInt32(idContact2));
                        }
                        else
                        {
                            idContact2 = null;
                            cmd.Parameters.AddWithValue("@idContact2", DBNull.Value);
                        }


                        if (cmbIdProp.Text.Length > 0)
                        {
                            cmd.Parameters.AddWithValue("@idProprietate", cmbIdProp.Text.ToString());
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@idProprietate", null);
                        }
                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            MessageBox.Show("Activitate editata cu succes!");
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Editarea activitatii a esuat!");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        void fillComboIdContact()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                string query = "select * from contacte";
                MySqlCommand cmd = new MySqlCommand(query, con);
                MySqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    String Id = dr.GetString("id_contact");
                    String nume = dr.GetString("nume");
                    String prenume = dr.GetString("prenume");
                    cmbContact.Items.Add(Id + " " + nume + " " + prenume);
                    cmbContact2.Items.Add(Id + " " + nume + " " + prenume);
                }
                dr.Close();
                con.Close();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void fillComboIdProp()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                string query = "select id_proprietate from proprietati where 1";
                MySqlCommand createCommand = new MySqlCommand(query, con);
                MySqlDataReader dr = createCommand.ExecuteReader();

                while (dr.Read())
                {
                    String Id = dr.GetInt32("id_proprietate").ToString();
                    cmbIdProp.Items.Add(Id);
                }
                dr.Close();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
