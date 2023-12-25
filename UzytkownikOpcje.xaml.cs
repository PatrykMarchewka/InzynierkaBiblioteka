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
    /// Interaction logic for UzytkownikOpcje.xaml
    /// </summary>
    public partial class UzytkownikOpcje : Page
    {
        public UzytkownikOpcje()
        {
            InitializeComponent();
        }

        private void btnPowrot_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.GlownaRamka.GoBack();
        }

        private void btnZmienHaslo_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Nawigacja("ZmienHaslo.xaml");
        }

        private void btnZmienRFIDPin_Click(object sender, RoutedEventArgs e)
        {
            if (GlowneOkno.ZalogowanyUzytkownik.RFID == null)
            {
                MessageBox.Show("Blad! Nie przypisano czytnika RFID");
            }
            else
            {
                //TODO: Przejscie na strone zmiany pinu
            }
        }

        private void btnZmienDaneOsobowe_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Nawigacja("ZmienDaneOsobowe.xaml");
        }
    }
}
