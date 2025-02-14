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
    /// Interaction logic for UzytkownikOpcje.xaml
    /// </summary>
    public partial class UzytkownikOpcje : Page
    {
        bool usun = false;
        public UzytkownikOpcje()
        {
            InitializeComponent();

            if (GlowneOkno.ZalogowanyAdministrator != null && GlowneOkno.ZalogowanyUzytkownik == null)
            {
                GlowneOkno.ZalogowanyUzytkownik = GlowneOkno.ZalogowanyAdministrator;
            }


            if (GlowneOkno.ZalogowanyUzytkownik.RFID != null)
            {
                btnDodajRFID.Content = " Usun karte RFID ";
                usun = true;
            }
            else
            {
                btnDodajRFID.Content = " Zarejestruj karte RFID ";
                usun = false;
            }

            //2 to administrator
            if (GlowneOkno.ZalogowanyUzytkownik.Rola.idRoli != 2)
            {
                btnZmienOpcjeBazy.Visibility = Visibility.Hidden;
            }
            else
            {
                btnZmienOpcjeBazy.Visibility = Visibility.Visible;
            }
        }

        private void btnPowrot_Click(object sender, RoutedEventArgs e)
        {
            if (GlowneOkno.ZalogowanyUzytkownik == GlowneOkno.ZalogowanyAdministrator)
            {
                GlowneOkno.ZalogowanyUzytkownik = null;
            }
            MainWindow.GlownaRamka.GoBack();
        }

        private void btnZmienHaslo_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Nawigacja("ZmienHaslo.xaml");
        }

        private void btnZmienDaneOsobowe_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Nawigacja("ZmienDaneOsobowe.xaml");
        }

        private void btnDodajRFID_Click(object sender, RoutedEventArgs e)
        {
            if (usun)
            {
                MessageBoxResult result = MessageBox.Show("Czy chcesz usunac przypisany RFID?", "Usuniecie RFID", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {

                    Logi nowyLog = new Logi() { DataWystapienia = DateTime.UtcNow, TrescWiadomosci = "Usunieto czytnik RFID", Uzytkownicy = GlowneOkno.ZalogowanyUzytkownik, Waznosc = 1 };
                    GlowneOkno.ZalogowanyUzytkownik.WszystkieLogi.Add(nowyLog);

                    GlowneOkno.ZalogowanyUzytkownik.RFID = null;
                    GlowneOkno.BazaDanych.SaveChanges();
                    MessageBox.Show("Pomyslnie usunieto przypisany RFID");
                    MainWindow.GlownaRamka.NavigationService.Refresh();
                }
            }
            else
            {
                MainWindow.Nawigacja("LogowanieRFID.xaml");
            }
            
        }

        private void btnZmienOpcjeBazy_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.opcjePolaczeniaOkno.Show();
        }
    }
}
