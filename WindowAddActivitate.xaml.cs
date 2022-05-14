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
    /// Interaction logic for WindowAddActivitate.xaml
    /// </summary>
    public partial class WindowAddActivitate : Window
    {
        public WindowAddActivitate()
        {
            InitializeComponent();
            fillComboContact();
            fillComboIdPRop();
        }

        void fillComboContact()
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
                    String nrTel = dr.GetString("nr_tel");
                    cmbContact.Items.Add(Id + " " + nume + " " + prenume + " " + nrTel);
                    cmbContact2.Items.Add(Id + " " + nume + " " + prenume + " " + nrTel);
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void fillComboIdPRop()
        {
            String connectionString = "SERVER=localhost;DATABASE=crmagentie_db;UID=root;PASSWORD=;";
            MySqlConnection con = new MySqlConnection(connectionString);
            try
            {
                con.Open();
                string query = "select id_proprietate from proprietati where 1";
                MySqlCommand createCommand = new MySqlCommand(query, con);
                MySqlDataReader dr = createCommand.ExecuteReader();

                while (dr.Read())
                {
                    String Id = dr.GetInt32("id_proprietate").ToString();
                    cmbIdProp.Items.Add(Id);
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnAddActivitate_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "SERVER = localhost; DATABASE = crmagentie_db; UID = root; PASSWORD =; ";
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                try
                {
                    using (var cmd = new MySqlCommand("INSERT INTO `activitati` ( `tip`, `id_contact`, `id_contact2`, `id_proprietate`, `data`, `detalii`) VALUES (@Tip, @IdContact, @IdContact2, @IdProprietate, @Data, @Detalii)"))
                    {
                        cmd.Connection = con;
                        string idContact, idContact2;
                        string s = cmbContact.Text.ToString();
                        int index = s.IndexOf(' ');
                        if (index > 0)
                        {
                            idContact = s.Substring(0, index);
                        }
                        else
                        {
                            idContact = null;
                        }


                        s = cmbContact2.Text.ToString();
                        index = s.IndexOf(' ');
                        if (index > 0)
                        {
                            idContact2 = s.Substring(0, index);
                        }
                        else
                        {
                            idContact2 = null;
                        }

                        if (cmbIdProp.Text.Length > 0)
                        {
                            cmd.Parameters.AddWithValue("@IdProprietate", cmbIdProp.Text.ToString());
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@IdProprietate", null);
                        }

                        if (idContact != null)
                        {
                            cmd.Parameters.AddWithValue("@IdContact", Convert.ToInt32(idContact));
                        }
                        else {
                            cmd.Parameters.AddWithValue("@IdContact", DBNull.Value);
                        }

                        if (idContact2 != null)
                        {
                            cmd.Parameters.AddWithValue("@IdContact2", Convert.ToInt32(idContact2));
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@IdContact2", DBNull.Value);
                        }

                        cmd.Parameters.AddWithValue("@Tip", cmbActivitate.Text.ToString());

                        cmd.Parameters.AddWithValue("@Data", dtpData.Text);
                        cmd.Parameters.AddWithValue("@Detalii", txtDetalii.Text);

                        con.Open();
                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            MessageBox.Show("Activitate adaugata!");

                        }
                        else
                        {
                            MessageBox.Show("Adaugarea activitatii a esuat!");
                        }

                        this.Close();
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
