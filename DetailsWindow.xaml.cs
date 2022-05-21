using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
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
using MySql.Data.MySqlClient;

namespace CRMAgentieImobiliara
{
    /// <summary>
    /// Interaction logic for DetailsWindow.xaml
    /// </summary>
    public partial class DetailsWindow : Window
    {
        string idDetalii;
        public DetailsWindow(string idDet)
        {
            PrintDialog printDlg = new PrintDialog();

            // printDlg.PrintVisual(this, "Window Printing.");
            printDlg.ShowDialog();
            idDetalii = idDet;
            InitializeComponent();
            //MessageBox.Show(idDet);
            string connectionstring = "SERVER=localhost;DATABASE=crmagentie_db;UID=root;PASSWORD=;";
            MySqlConnection con = new MySqlConnection(connectionstring);
            string query = "select * from proprietati where id_proprietate='" + idDetalii + "'";
            MySqlCommand cmd = new MySqlCommand(query, con);
            MySqlDataReader dr;
            try {
                con.Open();
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
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
                        txtProprietar.Text = numeContact + " " + prenumeContact + "(ID="+idContact+")\n " + nrTel;
                    }
                    conContact.Close();

                    string tipOferta = dr.GetString("tip_oferta").ToUpper();
                    string tipProprietate = dr.GetString("tip_proprietate").ToUpper();
                    string nrCamere = dr.GetInt32("nr_camere").ToString();
                    string zona = dr.GetString("zona").ToString().ToUpper();
                    string localitate = dr.GetString("localitate").ToString().ToUpper();
                    string id = dr.GetString("id_proprietate").ToString();
                    string adresa = dr.GetString("adresa").ToString();
                    string pret = dr.GetDouble("pret").ToString();
                    string comision = dr.GetDouble("comision").ToString();
                    string suprafata = dr.GetDouble("suprafata_utila").ToString();
                    string etaj = dr.GetString("etaj").ToString();
                    string etajeImobil = dr.GetString("nr_etaje_imobil").ToString();
                    string compartimentare = dr.GetString("compartimentare").ToString();
                    string nrBai = dr.GetString("nr_bai").ToString();
                    string descriere = dr.GetString("descriere").ToString();
                    string linkOferta = dr.GetString("link_oferta").ToString();
                    

                    if (nrCamere == "1")
                    {
                        txtTitle.Text = tipOferta + " " + tipProprietate + " " + nrCamere + " CAMERA " + zona + " - " + localitate;
                    }
                    else {
                        txtTitle.Text = tipOferta + " " + tipProprietate + " " + nrCamere + " CAMERE " + zona + " - " + localitate;
                    }
                    txtID.Text = id;
                    txtAdresa.Text = adresa;
                    txtPret.Text = pret + " EUR";
                    txtComision.Text = comision + " EUR";
                    txtSuprafata.Text = suprafata + " mp";
                    txtEtaj.Text = etaj + "/" + etajeImobil;
                    txtCompartimentare.Text = compartimentare;
                    txtNrBai.Text = nrBai;
                    txtDescriere.Text = string.Format(descriere);
                    txtLinkOferta.Text = linkOferta;

                    byte[] imgblob = (byte[])dr["imagini"];

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
            catch { }
        }
    }
}
