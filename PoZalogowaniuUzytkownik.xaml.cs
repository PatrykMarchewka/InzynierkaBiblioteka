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
    /// Interaction logic for PoZalogowaniu.xaml
    /// </summary>
    public partial class PoZalogowaniu : Page
    {

        public PoZalogowaniu()
        {
            InitializeComponent();


            if (GlowneOkno.ZalogowanyAdministrator != null && GlowneOkno.ZalogowanyUzytkownik.StatusKonta.idStatusu == 2)
            {
                btnZbanuj.Visibility = Visibility.Hidden;
            }
            else if (GlowneOkno.ZalogowanyAdministrator != null && GlowneOkno.ZalogowanyUzytkownik.StatusKonta.idStatusu == 1)
            {
                btnOdbanuj.Visibility = Visibility.Hidden;
            }
            else
            {
                btnZbanuj.Visibility = Visibility.Hidden;
                btnOdbanuj.Visibility = Visibility.Hidden;
            }

            //TODO: Dodanie przyciskow dla administratora
            if (GlowneOkno.ZalogowanyUzytkownik.StatusKonta.idStatusu == 2 && GlowneOkno.ZalogowanyAdministrator == null)
            {
                MessageBox.Show("Blad! Konto obecnie zbanowane, skontaktuj sie z administratorem");
                btnPowrot_Click(this, new RoutedEventArgs());
            }

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

        private void btnZobaczZaleglosci_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Nawigacja("ZobaczZaleglosci.xaml");
        }

        private void btnZbanuj_Click(object sender, RoutedEventArgs e)
        {
            GlowneOkno.ZalogowanyUzytkownik.StatusKonta = GlowneOkno.BazaDanych.Statusy.First(s => s.idStatusu == 2);
            Logi nowyLog = new Logi() { DataWystapienia = DateTime.UtcNow, TrescWiadomosci = $"Administrator o ID= {GlowneOkno.ZalogowanyAdministrator.idUzytkownika} zbanowal uzytkownika", Uzytkownicy = GlowneOkno.ZalogowanyUzytkownik, Waznosc = 1 };
            GlowneOkno.ZalogowanyUzytkownik.WszystkieLogi.Add(nowyLog);
            GlowneOkno.BazaDanych.SaveChanges();
            btnPowrot_Click(this, new RoutedEventArgs());
        }

        private void btnOdbanuj_Click(object sender, RoutedEventArgs e)
        {
            GlowneOkno.ZalogowanyUzytkownik.StatusKonta = GlowneOkno.BazaDanych.Statusy.First(s => s.idStatusu == 1);
            Logi nowyLog = new Logi() { DataWystapienia = DateTime.UtcNow, TrescWiadomosci = $"Administrator o ID= {GlowneOkno.ZalogowanyAdministrator.idUzytkownika} odbanowal uzytkownika", Uzytkownicy = GlowneOkno.ZalogowanyUzytkownik, Waznosc = 1 };
            GlowneOkno.ZalogowanyUzytkownik.WszystkieLogi.Add(nowyLog);
            GlowneOkno.BazaDanych.SaveChanges();
            btnPowrot_Click(this, new RoutedEventArgs());
        }
    }
}
