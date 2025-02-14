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
        public static Recenzje? recenzja;
        public static Uzytkownicy? reportujacy;
        public ZobaczRecenzje()
        {
            InitializeComponent();
            this.Loaded += ZobaczRecenzje_Loaded;


            if (recenzja.Ukryta)
            {
                btnSchowajPokazRecenzje.Content = " Odblokuj recenzje ";
            }
            else
            {
                btnSchowajPokazRecenzje.Content = " Zablokuj recenzje ";
            }

            if (GlowneOkno.ZalogowanyAdministrator != null && reportujacy != null)
            {
                btnZbanujReportowanego.Visibility = Visibility.Visible;
                btnSchowajPokazRecenzje.Visibility = Visibility.Visible;
                btnZbanujReportujacego.Visibility = Visibility.Visible;
                btnZglosRecenzje.Visibility = Visibility.Visible;
            }
            else if (recenzja.Uzytkownicy == GlowneOkno.ZalogowanyUzytkownik)
            {
                btnZbanujReportowanego.Visibility = Visibility.Hidden;
                btnSchowajPokazRecenzje.Visibility = Visibility.Hidden;
                btnZbanujReportujacego.Visibility = Visibility.Hidden;
                btnZglosRecenzje.Visibility = Visibility.Hidden;
            }
            else if(GlowneOkno.ZalogowanyUzytkownik != null)
            {
                btnZbanujReportowanego.Visibility = Visibility.Hidden;
                btnSchowajPokazRecenzje.Visibility = Visibility.Hidden;
                btnZbanujReportujacego.Visibility = Visibility.Hidden;
                btnZglosRecenzje.Visibility = Visibility.Visible;    
            }
            
            else
            {
                btnZbanujReportowanego.Visibility = Visibility.Hidden;
                btnSchowajPokazRecenzje.Visibility = Visibility.Hidden;
                btnZbanujReportujacego.Visibility = Visibility.Hidden;
                btnZglosRecenzje.Visibility = Visibility.Visible;
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
            recenzja = null;
            reportujacy = null;
            MainWindow.GlownaRamka.GoBack();
        }

        private void btnZglosRecenzje_Click(object sender, RoutedEventArgs e)
        {

            MainWindow.Nawigacja("RaportujRecenzje.xaml");
        }

        private void btnZbanujReportujacego_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Czy chcesz zbanowac tego uzytkownika?", "Banowanie", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                reportujacy.StatusKonta = GlowneOkno.BazaDanych.Statusy.First(s => s.idStatusu == 2);
                Logi nowylog = new Logi() { DataWystapienia = DateTime.UtcNow, Uzytkownicy = reportujacy, TrescWiadomosci = $"Administrator {GlowneOkno.ZalogowanyAdministrator.idUzytkownika} zbanowal uzytkownika", Waznosc = 1 };
                GlowneOkno.BazaDanych.Logi.Add(nowylog);
                GlowneOkno.BazaDanych.SaveChanges();
            }
        }

        private void btnZbanujReportowanego_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Czy chcesz zbanowac tego uzytkownika?", "Banowanie", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                recenzja.Uzytkownicy.StatusKonta = GlowneOkno.BazaDanych.Statusy.First(s => s.idStatusu == 2);
                Logi nowylog = new Logi() { DataWystapienia = DateTime.UtcNow, Uzytkownicy = recenzja.Uzytkownicy, TrescWiadomosci = $"Administrator {GlowneOkno.ZalogowanyAdministrator.idUzytkownika} zbanowal uzytkownika", Waznosc = 1 };
                GlowneOkno.BazaDanych.Logi.Add(nowylog);
                GlowneOkno.BazaDanych.SaveChanges();
            }
        }

        private void btnSchowajPokazRecenzje_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Czy chcesz zmienic widocznosc recenzji?", "Zmiana widocznosci", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                recenzja.Ukryta = !recenzja.Ukryta;
                Logi nowylog = new Logi() { DataWystapienia = DateTime.UtcNow, Uzytkownicy = recenzja.Uzytkownicy, TrescWiadomosci = $"Administrator {GlowneOkno.ZalogowanyAdministrator.idUzytkownika} zmienil widocznosc na {recenzja.Ukryta}", Waznosc = 1 };
                GlowneOkno.BazaDanych.Logi.Add(nowylog);
                GlowneOkno.BazaDanych.SaveChanges();
            }
        }
    }
}
