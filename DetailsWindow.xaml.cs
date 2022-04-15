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
    /// Interaction logic for DetailsWindow.xaml
    /// </summary>
    public partial class DetailsWindow : Window
    {
        string idDetalii;
        public DetailsWindow(string idDet)
        {
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
               
                    string tipOferta = dr.GetString("tip_oferta").ToUpper();
                    string tipProprietate = dr.GetString("tip_proprietate").ToUpper();
                    string nrCamere = dr.GetInt32("nr_camere").ToString();
                    string zona = dr.GetString("zona").ToString().ToUpper();
                    if (nrCamere == "1")
                    {
                        txtTitle.Text = tipOferta + " " + tipProprietate + " " + nrCamere + " CAMERA " + zona;
                    }
                    else {
                        txtTitle.Text = tipOferta + " " + tipProprietate + " " + nrCamere + " CAMERE " + zona;
                    }
                }
            } 
            catch { }
        }
    }
}
