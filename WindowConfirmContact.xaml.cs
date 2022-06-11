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
        string cerere,userId;
        int userIdInt;
        public WindowConfirmContact(string request, string idUser, int IdUserInt)
        {

            InitializeComponent();
            userIdInt = IdUserInt;
            cerere = request;
            userId = idUser;
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
               
                Window1 window = new Window1(userId, userIdInt);
                window.Show();
                this.Close();
            }
            else if (cerere == "activitate") 
            { 
                WindowAddActivitate window = new WindowAddActivitate(userId, userIdInt);
                window.Show();
                this.Close();
            }
        }

        private void btnNu_Click(object sender, RoutedEventArgs e)
        {
           
            WindowContacte window = new WindowContacte(userIdInt);
            window.Show();
            this.Close();
        }
    }
}
