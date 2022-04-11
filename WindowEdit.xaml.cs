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
using System.Data;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Globalization;


namespace CRMAgentieImobiliara
{
    /// <summary>
    /// Interaction logic for WindowEdit.xaml
    /// </summary>
    public partial class WindowEdit : Window
    {

        public WindowEdit()
        {
            InitializeComponent();
            fillComboId();
            fillComboIdContact();
        }
        string connectionString = "SERVER=localhost;DATABASE=crmagentie_db;UID=root;PASSWORD=;";

        void fillComboId()
        {
            MySqlConnection con = new MySqlConnection(connectionString);
            try {
                con.Open();
                string query = "select * from proprietati";
                MySqlCommand createCommand = new MySqlCommand(query, con);
                MySqlDataReader dr = createCommand.ExecuteReader();

                while (dr.Read()) {
                    int Id = dr.GetInt32("id_proprietate");
                    cmbIdEdit.Items.Add(Id);
                }
                con.Close();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnUpdateProprietate_Click(object sender, RoutedEventArgs e)
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                try
                {
                    using (var cmd = new MySqlCommand("UPDATE `proprietati` SET `id_contact`=@idContact, `tip_oferta`=@tipOferta, `tip_proprietate`=@tipProprietate, `judet`=@judet, `localitate`=@localitate, `zona`=@zona, `adresa`=@adresa, `amplasament`=@amplasament, `nr_camere`=@nrCamere, `nr_bai`=@nrBai, `etaj`=@etaj, `nr_etaje_imobil`=@etajeImobil, `suprafata_utila`=@sUtila, `compartimentare`=@compartimentare, `descriere`=@descriere, `link_oferta`=@linkOferta, `pret`=@pret, `comision`=@comision where id_proprietate='"+cmbIdEdit.SelectedValue+"'",con))
                    {
                        cmd.Connection = con;
                        string s = cmbContact.Text.ToString();
                        int index = s.IndexOf(' ');
                        string idContact = s.Substring(0, index);
                        cmd.Parameters.AddWithValue("@idContact", Convert.ToInt32(idContact));
                        cmd.Parameters.AddWithValue("@tipOferta", cmbTipOferta.Text.ToString());
                        cmd.Parameters.AddWithValue("@tipProprietate", cmbTipProprietate.Text.ToString());
                        cmd.Parameters.AddWithValue("@judet", cmbJudet.Text.ToString());
                        cmd.Parameters.AddWithValue("@localitate", localitateTextBox.Text.ToString());
                        cmd.Parameters.AddWithValue("@zona", zonaTextBox.Text.ToString());
                        cmd.Parameters.AddWithValue("@adresa", adresaTextBox.Text.ToString());
                        cmd.Parameters.AddWithValue("@amplasament", cmbAmplasament.Text.ToString());
                        cmd.Parameters.AddWithValue("@nrCamere", Convert.ToInt32(nrCamereTextBox.Text.ToString()));
                        cmd.Parameters.AddWithValue("@nrBai", Convert.ToInt32(nrBaiTextBox.Text.ToString()));
                        cmd.Parameters.AddWithValue("@etaj", Convert.ToInt32(etajTextBox.Text.ToString()));
                        cmd.Parameters.AddWithValue("@etajeImobil", Convert.ToInt32(etajeimobilTextBox.Text.ToString()));
                        cmd.Parameters.AddWithValue("@sUtila", float.Parse((suprafataUtilaTextBox.Text.ToString()), CultureInfo.InvariantCulture.NumberFormat));
                        cmd.Parameters.AddWithValue("@compartimentare", cmbCompartimentare.Text.ToString());
                        cmd.Parameters.AddWithValue("@descriere", descriereTextBox.Text.ToString());
                        cmd.Parameters.AddWithValue("@linkOferta", linkOfertaTextBox.Text.ToString());
                        cmd.Parameters.AddWithValue("@pret", float.Parse((pretTextBox.Text.ToString()), CultureInfo.InvariantCulture.NumberFormat));
                        cmd.Parameters.AddWithValue("@comision", float.Parse((comisionTextBox.Text.ToString()), CultureInfo.InvariantCulture.NumberFormat));

                        con.Open();
                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            MessageBox.Show("Record updated");
                        }
                        else
                        {
                            MessageBox.Show("Record failed");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error during insert: " + ex.Message);
                }
            }
        }

            void fillComboIdContact()
            {
                String connectionString = "SERVER=localhost;DATABASE=crmagentie_db;UID=root;PASSWORD=;";
                MySqlConnection con = new MySqlConnection(connectionString);
                try
                {
                    con.Open();
                    string query = "select * from contacte";
                    MySqlCommand createCommand = new MySqlCommand(query, con);
                    MySqlDataReader dr = createCommand.ExecuteReader();

                    while (dr.Read())
                    {
                        String Id = dr.GetString("id_contact");
                        String nume = dr.GetString("nume");
                        String prenume = dr.GetString("prenume");
                        cmbContact.Items.Add(Id + " " + nume + " " + prenume);
                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                MessageBox.Show(ex.Message);
            }
            }

            private void WindowEdit_Loaded(object sender, RoutedEventArgs e)
            {
                /* 
                 MySqlConnection connection = new MySqlConnection(connectionString);
                 connection.Open();

                 MySqlCommand cmd = new MySqlCommand("Select * from proprietati", connection);
                 MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                 DataSet ds = new DataSet();
                 da.Fill(ds);
                 cmd.ExecuteNonQuery();
                 connection.Close();
                 cmbIdEdit.DataContext = ds.Tables[0];
                 cmbIdEdit.DisplayMemberPath = "id_proprietate";
                 */
            }

            private void cmbIdEdit_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {
                MySqlConnection con = new MySqlConnection(connectionString);
                string query = "select * from proprietati where id_proprietate='" + cmbIdEdit.SelectedValue + "';";
                MySqlCommand cmd = new MySqlCommand(query, con);
                MySqlDataReader dr;
                try
                {
                    con.Open();
                    dr = cmd.ExecuteReader();



                    while (dr.Read())
                    {
                        string idProprietate = dr.GetInt32("id_proprietate").ToString();
                        MessageBox.Show("Urmeaza sa editati proprietatea cu ID intern"+idProprietate);
                        string tipOferta = dr.GetString("tip_oferta");
                        string tipProprietate = dr.GetString("tip_proprietate");
                        string idContact = dr.GetInt32("id_contact").ToString();
                        string judet = dr.GetString("judet");
                        string localitate = dr.GetString("localitate");
                        string zona = dr.GetString("zona");
                        string adresa = dr.GetString("adresa");
                        string amplasament = dr.GetString("amplasament");
                        string nrCamere = dr.GetInt32("nr_camere").ToString();
                        string nrBai = dr.GetInt32("nr_bai").ToString();
                        string etaj = dr.GetString("etaj");
                        string etajeImobil = dr.GetString("nr_etaje_imobil");
                        string sUtila = dr.GetDouble("suprafata_utila").ToString();
                        string compartimentare = dr.GetString("compartimentare");
                        string descriere = dr.GetString("descriere");
                        string linkOferta = dr.GetString("link_oferta");
                        string pret = dr.GetDouble("pret").ToString();
                        string comision = dr.GetDouble("comision").ToString();
                        string nrParcari = dr.GetInt32("nr_parcari").ToString();

                        cmbIdEdit.Text = idProprietate;
                        cmbTipOferta.Text = tipOferta;
                        cmbTipProprietate.Text = tipProprietate;
                        cmbContact.SelectedValue = idContact;
                        cmbJudet.Text = judet;
                        localitateTextBox.Text = localitate;
                        zonaTextBox.Text = zona;
                        adresaTextBox.Text = adresa;
                        cmbAmplasament.Text = amplasament;
                        nrCamereTextBox.Text = nrCamere;
                        nrBaiTextBox.Text = nrBai;
                        etajTextBox.Text = etaj;
                        etajeimobilTextBox.Text = etajeImobil;
                        suprafataUtilaTextBox.Text = sUtila;
                        cmbCompartimentare.Text = compartimentare;
                        descriereTextBox.Text = descriere;
                        linkOfertaTextBox.Text = linkOferta;
                        pretTextBox.Text = pret;
                        comisionTextBox.Text = comision;
                        locuriParcareTextBox.Text = nrParcari;
                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        
    } 
}
