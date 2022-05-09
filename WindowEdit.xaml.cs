using System;
using System.Collections.Generic;
using System.IO;
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
using Microsoft.Win32;
using System.Drawing.Imaging;


namespace CRMAgentieImobiliara
{
    /// <summary>
    /// Interaction logic for WindowEdit.xaml
    /// </summary>
    public partial class WindowEdit : Window
    {
        string idEd;
        string stringName, imageName;
        public WindowEdit(string idEditat)
        {

            InitializeComponent();
            fillComboId();
            fillComboIdContact();
            idEd = idEditat;

            string connectionString = "SERVER=localhost;DATABASE=crmagentie_db;UID=root;PASSWORD=;";
            MySqlConnection con = new MySqlConnection(connectionString);
            MySqlConnection conContact = new MySqlConnection(connectionString);
            string query = "select * from proprietati where id_proprietate='" + idEd + "';";
            MySqlCommand cmd = new MySqlCommand(query, con);
            MySqlDataReader dr;
            try
            {
                con.Open();
                conContact.Open();
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    string idProp = idEd;
                    //string idProprietate = dr.GetInt32("id_proprietate").ToString();
                    //MessageBox.Show("Urmeaza sa editati proprietatea cu ID intern"+idProp);
                    /*string tipOferta = dr.GetString("tip_oferta");
                    string tipProprietate = dr.GetString("tip_proprietate");
                    */
                   
                    /* string judet = dr.GetString("judet");
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
                     // string nrParcari = dr.GetInt32("nr_parcari").ToString();
                     */
                    
                    cmbIdEdit.Text = idProp;
                    cmbTipOferta.Text = dr.GetString("tip_oferta");
                    cmbTipProprietate.Text = dr.GetString("tip_proprietate");
                    //cmbContact.Text = dr.GetString("id_contact");
                    cmbJudet.Text = dr.GetString("judet");
                    localitateTextBox.Text = dr.GetString("localitate");
                    zonaTextBox.Text = dr.GetString("zona");
                    adresaTextBox.Text = dr.GetString("adresa");
                    cmbAmplasament.Text = dr.GetString("amplasament");
                    nrCamereTextBox.Text = dr.GetInt32("nr_camere").ToString();
                    nrBaiTextBox.Text = dr.GetInt32("nr_bai").ToString();
                    etajTextBox.Text = dr.GetString("etaj");
                    etajeimobilTextBox.Text = dr.GetString("nr_etaje_imobil");
                    suprafataUtilaTextBox.Text = dr.GetDouble("suprafata_utila").ToString();
                    cmbCompartimentare.Text = dr.GetString("compartimentare");
                    descriereTextBox.Text = dr.GetString("descriere");
                    linkOfertaTextBox.Text = dr.GetString("link_oferta");
                    pretTextBox.Text = dr.GetDouble("pret").ToString();
                    comisionTextBox.Text = dr.GetDouble("comision").ToString();
                    //locuriParcareTextBox.Text = nrParcari;

                    byte[] imgblob = (byte[])dr["imagini"];
                    if (imgblob != null)
                    {
                        MemoryStream stream = new MemoryStream();
                        stream.Write(imgblob, 0, imgblob.Length);
                        stream.Position = 0;
                        System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                        BitmapImage bi = new BitmapImage();
                        bi.BeginInit();

                        MemoryStream ms = new MemoryStream();
                        img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        ms.Seek(0, SeekOrigin.Begin);
                        bi.StreamSource = ms;
                        bi.EndInit();
                        imgThumb.Source = bi;
                    }
                    string idContact = dr.GetString("id_contact");
                    string queryContact = "select * from contacte where id_contact='" + idContact + "'";
                    MySqlCommand cmdJoinContact = new MySqlCommand(queryContact, conContact);
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
                dr.Close();
                con.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
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
                dr.Close();
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
                    using (var cmd = new MySqlCommand("UPDATE `proprietati` SET `id_contact`=@idContact, `tip_oferta`=@tipOferta, `tip_proprietate`=@tipProprietate, `judet`=@judet, `localitate`=@localitate, `zona`=@zona, `adresa`=@adresa, `amplasament`=@amplasament, `nr_camere`=@nrCamere, `nr_bai`=@nrBai, `etaj`=@etaj, `nr_etaje_imobil`=@etajeImobil, `suprafata_utila`=@sUtila, `compartimentare`=@compartimentare, `descriere`=@descriere, `link_oferta`=@linkOferta, `pret`=@pret, `comision`=@comision, `imagini`=@Img where id_proprietate='"+cmbIdEdit.SelectedValue+"'",con))
                    {
                        cmd.Connection = con;
                        string s = cmbContact.Text.ToString();
                        int index = s.IndexOf(' ');
                        string idContact = s.Substring(0, index);
                        byte[] imageByteArray;
                        try
                        {
                            if (imageName != null)
                            {
                                FileStream fs = new FileStream(imageName, FileMode.Open, FileAccess.Read);
                                imageByteArray = new byte[fs.Length];
                                fs.Read(imageByteArray, 0, Convert.ToInt32(fs.Length));
                                fs.Close();
                                cmd.Parameters.Add(new MySqlParameter("Img", imageByteArray));
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }

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
                            this.Close();
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
                    MySqlCommand cmd = new MySqlCommand(query, con);
                    MySqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        String Id = dr.GetString("id_contact");
                        String nume = dr.GetString("nume");
                        String prenume = dr.GetString("prenume");
                        cmbContact.Items.Add(Id + " " + nume + " " + prenume);
                    }
                    con.Close();
                dr.Close();
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

        private void btChange_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FileDialog dialog = new OpenFileDialog();
                dialog.InitialDirectory = Environment.SpecialFolder.MyPictures.ToString();
                dialog.Filter = "Imagine (*.jpg;*.png;*.jpeg)|*.jpg;*.png;*.jpeg";
                dialog.ShowDialog();
                {
                    stringName = dialog.SafeFileName;
                    imageName = dialog.FileName;
                    ImageSourceConverter isc = new ImageSourceConverter();
                    ///image1.SetVa
                }
                dialog = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

       
    } 
}
