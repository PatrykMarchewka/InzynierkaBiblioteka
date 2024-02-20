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
    /// Interaction logic for PoZalogowaniuAdmin.xaml
    /// </summary>
    public partial class PoZalogowaniuAdmin : Page
    {
        public PoZalogowaniuAdmin()
        {
            InitializeComponent();
            GlowneOkno.ZalogowanyAdministrator = GlowneOkno.ZalogowanyUzytkownik;

        }

        private void btnPowrot_Click(object sender, RoutedEventArgs e)
        {
            GlowneOkno.ZalogowanyUzytkownik = null;
            GlowneOkno.ZalogowanyAdministrator = null;
            MainWindow.GlownaRamka.GoBack();
        }

        private void btnWyszukajUzytkownika_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Przejscie na wyszukiwanie uzytkownikow

        }

        private void btnDodajKsiazke_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Nawigacja("AdminDodajKsiazke.xaml");
        }

        private void btnZobaczReporty_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Przejscie na zobaczenie reportow
        }
    }
}
