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

namespace CRMAgentieImobiliara
{
    /// <summary>
    /// Interaction logic for WindowContacte.xaml
    /// </summary>
    public partial class WindowContacte : Window
    {
        string ConnectionString = "SERVER=localhost;DATABASE=crmagentie_db;UID=root;PASSWORD=;";

        public WindowContacte()
        {
            InitializeComponent();
            string ConnectionString = "SERVER=localhost;DATABASE=crmagentie_db;UID=root;PASSWORD=;";
            MySqlConnection connection = new MySqlConnection(ConnectionString);
            MySqlCommand cmd = new MySqlCommand("select * from contacte", connection);
            connection.Open();
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            connection.Close();
            contacteDataGrid.DataContext = dt;
        }

        private void btnAddContact_Click(object sender, RoutedEventArgs e)
        {
            String connectionString = "Server=localhost;userid=root;password=;Database=crmagentie_db";
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                try
                {
                    using (var cmd = new MySqlCommand("INSERT INTO `contacte` ( `nume`, `prenume`, `nr_tel`, `nr_tel2`, `mail`) VALUES (@Nume, @Prenume, @NrTel, @NrTel2, @Mail)"))
                    {
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@Nume", txtNume.Text.ToString());
                        cmd.Parameters.AddWithValue("@Prenume", txtPrenume.Text.ToString());
                        cmd.Parameters.AddWithValue("@NrTel", txtNrTel.Text.ToString());
                        cmd.Parameters.AddWithValue("@NrTel2", txtNrTel2.Text.ToString());
                        cmd.Parameters.AddWithValue("@Mail", txtMail.Text.ToString());

                        con.Open();
                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            MessageBox.Show("Contact adaugat");
                            string ConnectionString = "SERVER=localhost;DATABASE=crmagentie_db;UID=root;PASSWORD=;";
                            MySqlConnection connection = new MySqlConnection(ConnectionString);
                            MySqlCommand command = new MySqlCommand("select * from contacte", connection);
                            connection.Open();
                            DataTable dt = new DataTable();
                            dt.Load(command.ExecuteReader());
                            connection.Close();
                            contacteDataGrid.DataContext = dt;

                            //this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Adaugare esuata");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Eroare la inserare: " + ex.Message);
                }
            }
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            string ConnectionString = "SERVER=localhost;DATABASE=crmagentie_db;UID=root;PASSWORD=;";
            MySqlConnection connection = new MySqlConnection(ConnectionString);
            MySqlCommand cmd = new MySqlCommand("select * from contacte", connection);
            connection.Open();
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            connection.Close();
            contacteDataGrid.DataContext = dt;
        }

       

        private void btnEditContact_Click(object sender, RoutedEventArgs e)
        {
            DataRowView row_selected = contacteDataGrid.SelectedItem as DataRowView;
            string idEditat = row_selected["id_contact"].ToString();
            if (row_selected != null)
            {
               
                using (MySqlConnection con = new MySqlConnection(ConnectionString))
                {
                    try
                    {
                        using (var cmd = new MySqlCommand("UPDATE `contacte` SET `nume`=@nume, `prenume`=@prenume, `nr_tel`=@nrTel, `nr_tel2`=@nrTel2, `mail`=@mail where id_contact='" + idEditat + "'", con))
                        {
                            cmd.Connection = con;

                            cmd.Parameters.AddWithValue("@nume", txtNume.Text);
                            cmd.Parameters.AddWithValue("@prenume", txtPrenume.Text);
                            cmd.Parameters.AddWithValue("@nrTel", txtNrTel.Text);
                            cmd.Parameters.AddWithValue("@nrTel2", txtNrTel2.Text);
                            cmd.Parameters.AddWithValue("@mail", txtMail.Text);


                            con.Open();
                            if (cmd.ExecuteNonQuery() > 0)
                            {
                                MessageBox.Show("Contact editat cu succes!");
                            }
                            else
                            {
                                MessageBox.Show("Editarea contactului a esuat!");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }


                }

            }
            else
            {
                MessageBox.Show("Nu ati selectat nicio activitate!");
            }
            
        }

        private void contacteDataGrid_Selected(object sender, RoutedEventArgs e)
        {
            MySqlConnection con = new MySqlConnection(ConnectionString);
            MySqlConnection conContact = new MySqlConnection(ConnectionString);
            DataRowView row_selected = contacteDataGrid.SelectedItem as DataRowView;
            string idEditat = row_selected["id_contact"].ToString();
            string query = "select * from contacte where id_contact='" + idEditat + "';";
            MySqlCommand cmd = new MySqlCommand(query, con);
            MySqlDataReader dr;

            try
            {
                con.Open();
                conContact.Open();
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    txtNume.Text = dr.GetString("nume").ToString();
                    txtPrenume.Text = dr.GetString("prenume");
                    txtNrTel.Text = dr.GetString("nr_tel");
                    txtNrTel2.Text = dr.GetString("nr_tel2");
                    txtMail.Text = dr.GetString("mail"); 
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