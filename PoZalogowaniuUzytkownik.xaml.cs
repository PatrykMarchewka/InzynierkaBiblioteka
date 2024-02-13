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
    /// Interaction logic for PoZalogowaniu.xaml
    /// </summary>
    public partial class PoZalogowaniu : Page
    {

        public PoZalogowaniu()
        {
            InitializeComponent();
            
        }

        private void btnPowrot_Click(object sender, RoutedEventArgs e)
        {
            GlowneOkno.ZalogowanyUzytkownik = null;
            MainWindow.GlownaRamka.GoBack();
        }

        private void btnWyszukajKsiazke_Click(object sender, RoutedEventArgs e)
        {
                MainWindow.Nawigacja("WyszukajKsiazke.xaml");
        }

        private void btnOddajKsiazke_Click(object sender, RoutedEventArgs e)
        {
            if (GlowneOkno.ZalogowanyUzytkownik.LiczbaWypozyczonychKsiazek == 0)
            {
                MessageBox.Show("Blad! Nie masz ksiazek do oddania");
            }
            else
            {
                //Przejscie do strony oddania
                MainWindow.Nawigacja("OddajKsiazke.xaml");
            }
        }

        private void btnOpcje_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Nawigacja("UzytkownikOpcje.xaml");
        }
    }
}
