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
using MySql.Data.MySqlClient;

namespace CRMAgentieImobiliara
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        private void btnAddProprietateNoua_Click(object sender, RoutedEventArgs e)
        {
            String connectionString = "Server=localhost;userid=root;password=;Database=crmagentie_db";
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                try
                {
                    using (var cmd = new MySqlCommand("INSERT INTO `proprietati` ( `id_contact`, `tip_oferta`, `tip_proprietate`, `judet`, `localitate`, `zona`, `adresa`, `amplasament`, `nr_camere`, `nr_bai`, `etaj`, `nr_etaje_imobil`, `suprafata_utila`, `compartimentare`, `descriere`, `link_oferta`, `pret`, `comision`) VALUES (@Contact, @Oferta, @TipProprietate, @Judet, @Localitate, @Zona, @Adresa, @Amplasament, @NrCamere, @NrBai, @Etaj, @EtajeImobil, @SUtila, @Compartimentare, @Descriere, @LinkOferta, @Pret, @Comision)"))
                    {
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@Contact", Convert.ToInt32(contactTextBox.Text.ToString()));
                        cmd.Parameters.AddWithValue("@Oferta", cmbTipOferta.Text.ToString());
                        cmd.Parameters.AddWithValue("@TipProprietate", cmbTipProprietate.Text.ToString());
                        cmd.Parameters.AddWithValue("@Judet", cmbJudet.Text.ToString());
                        cmd.Parameters.AddWithValue("@Localitate", localitateTextBox.Text.ToString());
                        cmd.Parameters.AddWithValue("@Zona", zonaTextBox.Text.ToString());
                        cmd.Parameters.AddWithValue("@Adresa", adresaTextBox.Text.ToString());
                        cmd.Parameters.AddWithValue("@Amplasament", cmbAmplasament.Text.ToString());
                        cmd.Parameters.AddWithValue("@NrCamere", Convert.ToInt32(nrCamereTextBox.Text.ToString()));
                        cmd.Parameters.AddWithValue("@NrBai", Convert.ToInt32(nrBaiTextBox.Text.ToString()));
                        cmd.Parameters.AddWithValue("@Etaj", Convert.ToInt32(etajTextBox.Text.ToString()));
                        cmd.Parameters.AddWithValue("@EtajeImobil", Convert.ToInt32(etajeimobilTextBox.Text.ToString()));
                        cmd.Parameters.AddWithValue("@SUtila", float.Parse((suprafataUtilaTextBox.Text.ToString()), CultureInfo.InvariantCulture.NumberFormat));
                        cmd.Parameters.AddWithValue("@Compartimentare", cmbCompartimentare.Text.ToString());
                        cmd.Parameters.AddWithValue("@Descriere", descriereTextBox.Text.ToString());
                        cmd.Parameters.AddWithValue("@LinkOferta", linkOfertaTextBox.Text.ToString());
                        cmd.Parameters.AddWithValue("@Pret", float.Parse((pretTextBox.Text.ToString()),CultureInfo.InvariantCulture.NumberFormat));
                        cmd.Parameters.AddWithValue("@Comision", float.Parse((comisionTextBox.Text.ToString()), CultureInfo.InvariantCulture.NumberFormat));

                        con.Open();
                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            MessageBox.Show("Record inserted");
                        }
                        else {
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
    }
}
