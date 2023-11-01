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
using static System.Net.Mime.MediaTypeNames;

namespace InżynierkaBiblioteka
{
    /// <summary>
    /// Interaction logic for LogowanieRFID.xaml
    /// </summary>
    public partial class LogowanieRFID : Page
    {
        private static string Kod = "";
        public LogowanieRFID()
        {
            InitializeComponent();
            //PreviewKeyDown += CzytanieZCzytnika;
        }

        private void btnPowrot_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.GlownaRamka.GoBack();
        }

        //RIP IN PEACE  27.10.2023
        //private static void CzytanieZCzytnika(object sender, KeyEventArgs e)
        //{
        //    if (e.Key >= Key.D0 && e.Key <= Key.D9)
        //    {
        //        Kod += (e.Key - Key.D0).ToString();
        //    }
        //    else if (e.Key == Key.Enter)
        //    {
        //        //Tutaj sprawdzic konto
        //        //
        //        Kod = String.Empty;
        //    }
        //}
    }
}
