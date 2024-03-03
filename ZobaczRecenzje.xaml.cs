using InżynierkaBiblioteka.BazaDanych;
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
    /// Interaction logic for ZobaczRecenzje.xaml
    /// </summary>
    public partial class ZobaczRecenzje : Page
    {
        public static Recenzje recenzja;
        public ZobaczRecenzje()
        {
            InitializeComponent();
            this.Loaded += ZobaczRecenzje_Loaded;
            if (recenzja.Uzytkownicy == GlowneOkno.ZalogowanyUzytkownik)
            {
                btnZglosRecenzje.Visibility = Visibility.Hidden;
            }
            
        }

        private void ZobaczRecenzje_Loaded(object sender, RoutedEventArgs e)
        {
            lblUzytkownicy.Content = String.Empty;
            lblOcena.Content = String.Empty;
            txtBlockRecenzja.Text = String.Empty;
            lblUzytkownicy.Content = recenzja.Uzytkownicy.LoginUzytkownika;
            for (int i = 0; i < recenzja.Ocena; i++)
            {
                lblOcena.Content += "⭐";
            }

            txtBlockRecenzja.Text = recenzja.TekstRecenzji;



        }

        private void btnPowrot_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.GlownaRamka.GoBack();
        }

        private void btnZglosRecenzje_Click(object sender, RoutedEventArgs e)
        {

            MainWindow.Nawigacja("RaportujRecenzje.xaml");
        }
    }
}
