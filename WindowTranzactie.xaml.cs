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
    /// Interaction logic for WindowTranzactie.xaml
    /// </summary>
    public partial class WindowTranzactie : Window
    {
        string idEd;
        String connectionString = "SERVER=localhost;DATABASE=crmagentie_db;UID=root;PASSWORD=;";
        public WindowTranzactie(string idEditat)
        {
            idEd = idEditat;
            InitializeComponent();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                try
                {
                    using (var cmd = new MySqlCommand("UPDATE `proprietati` SET `stadiu`='tranzactionata', `pret_tranzactionare`=@pretTranz, `data_tranzactionare`=@dataTranz, `comision_incasat`=@comisionIncasat where id_proprietate='" + idEd + "'", con))
                    {
                        cmd.Connection = con;
                        double pretTranz = Convert.ToDouble (txtPretTranzactie.Text);
                        double comisionIncasat = Convert.ToDouble(txtComisionIncasat.Text);
                        string data = dataTranzactie.SelectedDate.Value.ToString("yyyy-MM-dd");
                        cmd.Parameters.AddWithValue("@dataTranz", data);
                        cmd.Parameters.AddWithValue("@comisionIncasat", comisionIncasat);
                        cmd.Parameters.AddWithValue("@pretTranz", pretTranz);
                        con.Open();
                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            MessageBox.Show("Felicitari!");

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
    }
}