﻿using System;
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

        private void btnDelContact_Click(object sender, RoutedEventArgs e)
        {
            DataRowView row_selected = contacteDataGrid.SelectedItem as DataRowView;
            if (row_selected != null)
            {
                string ConnectionString = "SERVER=localhost;DATABASE=crmagentie_db;UID=root;PASSWORD=;";
                MySqlConnection connection = new MySqlConnection(ConnectionString);
                string idSters = row_selected["id_contact"].ToString();
                string query = "DELETE from contacte where id_contact='" + idSters + "'";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
                MessageBox.Show("Contact sters!");
                MySqlCommand refresh = new MySqlCommand("select * from contacte", connection);
                connection.Open();
                DataTable dt = new DataTable();
                dt.Load(refresh.ExecuteReader());
                connection.Close();
                contacteDataGrid.DataContext = dt;

            }
            else
            {
                MessageBox.Show("Nu ati selectat nicio proprietate!");
            }

        }
    }
}
