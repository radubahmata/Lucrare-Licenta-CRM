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
using System.Globalization;
using System.IO;
using MySql.Data.MySqlClient;
using System.Drawing.Imaging;
using Microsoft.Win32;

namespace CRMAgentieImobiliara
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        String connectionString = "Server=localhost;userid=root;password=;Database=crmagentie_db";
       // DataSet dataSet;
        string stringName, imageName;
        string userId;
        public Window1(string idUser)
        {
            userId = idUser;
            InitializeComponent();
            fillComboIdContact();
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
                    cmbContact.Items.Add(nume+" "+prenume+" "+"(ID="+Id+")");
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAddProprietateNoua_Click(object sender, RoutedEventArgs e)
        {
            
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                try
                {
                    using (var cmd = new MySqlCommand("INSERT INTO `proprietati` ( `id_contact`, `tip_oferta`, `tip_proprietate`, `judet`, `localitate`, `zona`, `adresa`, `amplasament`, `nr_camere`, `nr_bai`, `etaj`, `nr_etaje_imobil`, `suprafata_utila`, `compartimentare`, `descriere`, `link_oferta`, `pret`, `comision`,`imagini`,`userId`) VALUES (@Contact, @Oferta, @TipProprietate, @Judet, @Localitate, @Zona, @Adresa, @Amplasament, @NrCamere, @NrBai, @Etaj, @EtajeImobil, @SUtila, @Compartimentare, @Descriere, @LinkOferta, @Pret, @Comision, @Img, @userId)"))
                    {
                        cmd.Connection = con;
                        string s = cmbContact.Text.ToString();
                        int index = s.IndexOf('=');
                       
                        string idContact = s.Substring(index+1);
                        int index2 = idContact.IndexOf(')');
                        string idContactFinal = idContact.Substring(0, index2);
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

                        cmd.Parameters.AddWithValue("@userId", userId);
                        cmd.Parameters.AddWithValue("@Contact", Convert.ToInt32(idContactFinal));
                        cmd.Parameters.AddWithValue("@Oferta", cmbTipOferta.Text.ToString());
                        cmd.Parameters.AddWithValue("@TipProprietate", cmbTipProprietate.Text.ToString());
                        cmd.Parameters.AddWithValue("@Judet", cmbJudet.Text.ToString());
                        cmd.Parameters.AddWithValue("@Localitate", localitateTextBox.Text.ToString());
                        cmd.Parameters.AddWithValue("@Zona", zonaTextBox.Text.ToString());
                        cmd.Parameters.AddWithValue("@Adresa", adresaTextBox.Text.ToString());
                        cmd.Parameters.AddWithValue("@Amplasament", cmbAmplasament.Text.ToString());
                        cmd.Parameters.AddWithValue("@NrCamere", Convert.ToInt32(nrCamereTextBox.Text.ToString()));
                        cmd.Parameters.AddWithValue("@NrBai", Convert.ToInt32(nrBaiTextBox.Text.ToString()));
                        cmd.Parameters.AddWithValue("@Etaj", etajTextBox.Text.ToString());
                        if (etajTextBox.Text.ToString() == "P")
                        {
                            cmd.Parameters.AddWithValue("@EtajeImobil", "0"); 
                        }
                        else {
                            cmd.Parameters.AddWithValue("@EtajeImobil", Convert.ToInt32(etajeimobilTextBox.Text.ToString()));
                        }
                        cmd.Parameters.AddWithValue("@SUtila", float.Parse((suprafataUtilaTextBox.Text.ToString()), CultureInfo.InvariantCulture.NumberFormat));
                        cmd.Parameters.AddWithValue("@Compartimentare", cmbCompartimentare.Text.ToString());
                        cmd.Parameters.AddWithValue("@Descriere", descriereTextBox.Text.ToString());
                        cmd.Parameters.AddWithValue("@LinkOferta", linkOfertaTextBox.Text.ToString());
                        cmd.Parameters.AddWithValue("@Pret", float.Parse((pretTextBox.Text.ToString()),CultureInfo.InvariantCulture.NumberFormat));
                        cmd.Parameters.AddWithValue("@Comision", float.Parse((comisionTextBox.Text.ToString()), CultureInfo.InvariantCulture.NumberFormat));
                        
                        con.Open();
                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            MessageBox.Show("Proprietate adaugata!");
                            
               
                            this.Close();
                           
                        }
                        else {
                            MessageBox.Show("Adaugarea proprietatii a esuat!");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error during insert: " + ex.Message);
                }
            }
            
        }

        

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            try {
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
            catch (Exception ex) {
                MessageBox.Show(ex.Message.ToString());
            }
        }

      /*  private void insertImgData()
        {
            try {
                if (imageName != null)
                {
                    FileStream fs = new FileStream(imageName, FileMode.Open, FileAccess.Read);
                    byte[] imageByteArray = new byte[fs.Length];
                    fs.Read(imageByteArray, 0, Convert.ToInt32(fs.Length));
                    fs.Close();
                    using (MySqlConnection imgcon = new MySqlConnection(connectionString))
                    {
                        imgcon.Open();
                        string instructiune = "insert into proprietati (imagini) values @img";
                        using (MySqlCommand imgcmd = new MySqlCommand(instructiune, imgcon))
                        { 
                            imgcmd.Parameters.
                        }
                    }
                }
            }
            catch (Exception e) {
                MessageBox.Show(e.Message);
            }
        }*/
    }
}
