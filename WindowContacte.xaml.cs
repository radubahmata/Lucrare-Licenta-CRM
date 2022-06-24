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
        int userIdInt;
        MySqlConnection con, conVerificare;
        public WindowContacte(int idUserInt, MySqlConnection connection)
        {
            con = connection;
            conVerificare = connection;
            userIdInt = idUserInt;
            InitializeComponent();
            MySqlCommand cmd = new MySqlCommand("select * from contacte", connection);
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            connection.Close();
            contacteDataGrid.DataContext = dt;
        }

        private void btnAddContact_Click(object sender, RoutedEventArgs e)
        {
            using (con)
            {
                var cmdVerificare = new MySqlCommand("SELECT * FROM contacte WHERE nr_tel='"+txtNrTel.Text+"'");
                cmdVerificare.Connection = con;
                if (con.State == ConnectionState.Closed)
                    con.Open();
                if (Convert.ToInt32( cmdVerificare.ExecuteScalar()) > 0)
                {
                    MessageBox.Show("Exista un contact cu acest numar!");
                }
                else
                {
                    
                    try
                    {
                        using (var cmd = new MySqlCommand("INSERT INTO `contacte` ( `nume`, `prenume`, `nr_tel`, `nr_tel2`, `mail`,`userId`) VALUES (@Nume, @Prenume, @NrTel, @NrTel2, @Mail, " + userIdInt + ")"))
                        {
                            cmd.Connection = con;
                            string nrTel = txtNrTel.Text.ToString();
                            string nrTel2 = txtNrTel2.Text.ToString();
                            if ((nrTel.Length == 10 && nrTel[0] == '0' && nrTel2.Length > 0) || (nrTel.Length == 0 && nrTel2.Length > 0) || (nrTel.Length == 10 && nrTel[0] == '0' && nrTel2.Length == 0))
                            {

                                cmd.Parameters.AddWithValue("@Nume", txtNume.Text.ToString());
                                cmd.Parameters.AddWithValue("@Prenume", txtPrenume.Text.ToString());
                                cmd.Parameters.AddWithValue("@NrTel", txtNrTel.Text.ToString());
                                cmd.Parameters.AddWithValue("@NrTel2", txtNrTel2.Text.ToString());
                                cmd.Parameters.AddWithValue("@Mail", txtMail.Text.ToString());

                                if (con.State == ConnectionState.Closed)
                                    con.Open();
                                if (cmd.ExecuteNonQuery() > 0)
                                {
                                    MessageBox.Show("Contact adaugat");
                                    MySqlCommand command = new MySqlCommand("select * from contacte", con);
                                    if (con.State == ConnectionState.Closed)
                                        con.Open();
                                    DataTable dt = new DataTable();
                                    dt.Load(command.ExecuteReader());
                                    con.Close();
                                    contacteDataGrid.DataContext = dt;
                                }
                                else
                                {
                                    MessageBox.Show("Adaugare esuata");
                                }
                            }
                            else MessageBox.Show("Introduceti cel putin un numar de telefon! Formatul numarului de telefon principal trebuie sa fie cel de Romania. In cazul unui numar de strainatate introduceti in caseta Numar Telefon Alternativ!");
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Eroare la inserare: " + ex.Message);
                    }
                }
            }
        }

        private void btnEditContact_Click(object sender, RoutedEventArgs e)
        {
            DataRowView row_selected = contacteDataGrid.SelectedItem as DataRowView;
            string idEditat = row_selected["id_contact"].ToString();
            if (row_selected != null)
            {
               
                using (con)
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

                            if (con.State == ConnectionState.Closed)
                                con.Open();
                            if (cmd.ExecuteNonQuery() > 0)
                            {
                                MessageBox.Show("Contact editat cu succes!");
                                MySqlCommand command = new MySqlCommand("select * from contacte", con);
                                if (con.State == ConnectionState.Closed)
                                    con.Open();
                                DataTable dt = new DataTable();
                                dt.Load(command.ExecuteReader());
                                con.Close();
                                contacteDataGrid.DataContext = dt;
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
            DataRowView row_selected = contacteDataGrid.SelectedItem as DataRowView;
            string idEditat = row_selected["id_contact"].ToString();
            string query = "select * from contacte where id_contact='" + idEditat + "';";
            MySqlCommand cmd = new MySqlCommand(query, con);
            MySqlDataReader dr;

            try
            {
                if(con.State==ConnectionState.Closed)
                con.Open();
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    txtNume.Text = dr.GetString("nume").ToString();
                    txtPrenume.Text = dr.GetString("prenume");
                    txtNrTel.Text = dr.GetString("nr_tel");
                    txtNrTel2.Text = dr.GetString("nr_tel2");
                    txtMail.Text = dr.GetString("mail"); 
                }
                dr.Close();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}