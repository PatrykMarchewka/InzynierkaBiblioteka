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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InżynierkaBiblioteka
{
    /// <summary>
    /// Interaction logic for GlowneOkno.xaml
    /// </summary>
    public partial class GlowneOkno : Page
    {
        public GlowneOkno()
        {
            InitializeComponent();
        }

        private void btnZalogujHaslem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnZalogujRFID_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Nawigacja("LogowanieRFID.xaml");
        }

        private void btnStworzNoweKonto_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
