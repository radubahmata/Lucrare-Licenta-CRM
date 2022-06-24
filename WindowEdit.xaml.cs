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
        byte[] imgDB;
        MySqlConnection con;
        public WindowEdit(string idEditat, MySqlConnection connection)
        {
            con = connection;
            InitializeComponent();
            fillComboIdContact();
            idEd = idEditat;
            string query = "select * from proprietati JOIN contacte  ON proprietati.id_contact=contacte.id_contact where proprietati.id_proprietate='" + idEd + "'";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataReader dr;
            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    string idProp = idEd;

                    string idContact = dr.GetString("id_contact");
                    String Id = dr.GetString("id_contact");
                    String nume = dr.GetString("nume");
                    String prenume = dr.GetString("prenume");
                    cmbContact.Text = Id + " " + nume + " " + prenume;

                    cmbIdEdit.Text = idProp;
                    cmbTipOferta.Text = dr.GetString("tip_oferta");
                    cmbTipProprietate.Text = dr.GetString("tip_proprietate");
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

                    byte[] imgblob = (byte[])dr["imagini"];
                    
                    if (imgblob != null)
                    {
                        imgDB = imgblob;
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
                }
                dr.Close();
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            } 
        }

        private void btnUpdateProprietate_Click(object sender, RoutedEventArgs e)
        {
            using (con)
            {
                try
                {
                    using (var cmd = new MySqlCommand("UPDATE `proprietati` SET `id_contact`=@idContact, `tip_oferta`=@tipOferta, `tip_proprietate`=@tipProprietate, `judet`=@judet, `localitate`=@localitate, `zona`=@zona, `adresa`=@adresa, `amplasament`=@amplasament, `nr_camere`=@nrCamere, `nr_bai`=@nrBai, `etaj`=@etaj, `nr_etaje_imobil`=@etajeImobil, `suprafata_utila`=@sUtila, `compartimentare`=@compartimentare, `descriere`=@descriere, `link_oferta`=@linkOferta, `pret`=@pret, `comision`=@comision, `imagini`=@Img where id_proprietate='"+idEd+"'",con))
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
                                cmd.Parameters.AddWithValue("Img", imageByteArray);
                            }
                            else {
                                cmd.Parameters.AddWithValue("Img", imgDB);
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
                        if (etajTextBox.Text.ToString() != "P")
                        {
                            cmd.Parameters.AddWithValue("@etaj", etajTextBox.Text.ToString());
                        }
                        else {
                            cmd.Parameters.AddWithValue("@etaj", "0");
                        }
                        cmd.Parameters.AddWithValue("@etajeImobil", etajeimobilTextBox.Text.ToString());
                        cmd.Parameters.AddWithValue("@sUtila", float.Parse((suprafataUtilaTextBox.Text.ToString()), CultureInfo.InvariantCulture.NumberFormat));
                        cmd.Parameters.AddWithValue("@compartimentare", cmbCompartimentare.Text.ToString());
                        cmd.Parameters.AddWithValue("@descriere", descriereTextBox.Text.ToString());
                        cmd.Parameters.AddWithValue("@linkOferta", linkOfertaTextBox.Text.ToString());
                        cmd.Parameters.AddWithValue("@pret", float.Parse((pretTextBox.Text.ToString()), CultureInfo.InvariantCulture.NumberFormat));
                        cmd.Parameters.AddWithValue("@comision", float.Parse((comisionTextBox.Text.ToString()), CultureInfo.InvariantCulture.NumberFormat));

                        if (con.State == ConnectionState.Closed)
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
                    }
                    con.Close();
                dr.Close();
                }
                catch (Exception ex)
                {
                MessageBox.Show(ex.Message);
                }
            }



       

        private void btnTranzactionat_Click(object sender, RoutedEventArgs e)
        {
            WindowTranzactie window = new WindowTranzactie(idEd, con);
            window.Show();
            this.Close();
        }

        private void btnRetras_Click(object sender, RoutedEventArgs e)
        {
            using (con)
            {
                try
                {
                    using (var cmd = new MySqlCommand("UPDATE `proprietati` SET `stadiu`='retrasa' where id_proprietate='" + idEd + "'", con))
                    {
                        cmd.Connection = con;
                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            MessageBox.Show("Proprietate retrasa!");

                        }
                        else
                        {
                            MessageBox.Show("Nu s-au modificat date!");
                        }

                        this.Close();
                    }
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
        }

        private void btnPierdut_Click(object sender, RoutedEventArgs e)
        {
            using (con)
            {
                try
                {
                    using (var cmd = new MySqlCommand("UPDATE `proprietati` SET `stadiu`='pierduta' where id_proprietate='" + idEd + "'", con))
                    {
                        cmd.Connection = con;
                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            MessageBox.Show("Proprietate pierduta!");

                        }
                        else
                        {
                            MessageBox.Show("Nu s-au modificat date!");
                        }

                        this.Close();
                    }
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
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
