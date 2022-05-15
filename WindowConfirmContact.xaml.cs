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

namespace CRMAgentieImobiliara
{
    /// <summary>
    /// Interaction logic for WindowConfirmContact.xaml
    /// </summary>
    public partial class WindowConfirmContact : Window
    {
        string cerere;
        public WindowConfirmContact(string request)
        {
            InitializeComponent();
            cerere = request;
            if (cerere == "proprietate")
            {
                txtConfirmContact.Text = "Asigurati-va ca ati introdus contactele aferente proprietatii adaugate!";
            }
            else if (cerere == "activitate")
            {
                txtConfirmContact.Text = "Asigurati-va ca ati introdus contactele aferente activitatii adaugate!";
            }
        }

        private void btnDa_Click(object sender, RoutedEventArgs e)
        {
            if (cerere == "proprietate")
            {
               
                Window1 window = new Window1();
                window.Show();
                this.Close();
            }
            else if (cerere == "activitate") 
            { 
                WindowAddActivitate window = new WindowAddActivitate();
                window.Show();
                this.Close();
            }
        }

        private void btnNu_Click(object sender, RoutedEventArgs e)
        {
           
            WindowContacte window = new WindowContacte();
            window.Show();
            this.Close();
        }
    }
}
