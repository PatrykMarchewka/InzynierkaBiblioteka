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
using InżynierkaBiblioteka.BazaDanych;
using ZXing;

namespace InżynierkaBiblioteka.Administrator
{
    /// <summary>
    /// Interaction logic for PokazRaport.xaml
    /// </summary>
    public partial class PokazRaport : Page
    {
        public static Reporty? report;

        public PokazRaport()
        {
            InitializeComponent();
            if (report.Recenzje.Ukryta)
            {
                btnUkryjRecenzje.Content = " Pokaz recenzje ";
            }
            else
            {
                btnUkryjRecenzje.Content = " Ukryj recenzje ";
            }

            if (report.Reportujacy.StatusKonta == GlowneOkno.BazaDanych.Statusy.First(s => s.idStatusu == 1))
            {
                btnZbanujReportujacego.Content = " Zbanuj reportujacego ";
            }
            else if(report.Reportujacy.StatusKonta == GlowneOkno.BazaDanych.Statusy.First(s => s.idStatusu == 2))
            {
                btnZbanujReportujacego.Content = " Odbanuj reportujacego ";
            }


            if (report.Recenzje.Uzytkownicy.StatusKonta == GlowneOkno.BazaDanych.Statusy.First(s => s.idStatusu == 1))
            {
                btnZbanujReportowanego.Content = " Zbanuj reportowanego ";
            }
            else if(report.Recenzje.Uzytkownicy.StatusKonta == GlowneOkno.BazaDanych.Statusy.First(s => s.idStatusu == 2))
            {
                btnZbanujReportowanego.Content = " Odbanuj reportowanego ";
            }
            lblReportowany.Content = report.Recenzje.Uzytkownicy.LoginUzytkownika;
            lblReportujacy.Content = report.Reportujacy.LoginUzytkownika;
            txtBlockRecenzja.Text = report.Recenzje.TekstRecenzji;


        }

        private void btnPowrot_Click(object sender, RoutedEventArgs e)
        {
            report = null;
            MainWindow.GlownaRamka.GoBack();
        }

        private void btnZbanujReportujacego_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result;
            if (report.Reportujacy.StatusKonta == GlowneOkno.BazaDanych.Statusy.First(s => s.idStatusu == 1))
            {
                result = MessageBox.Show("Czy chcesz zbanowac reportujacego uzytkownika?", "Zbanowac reportujacego", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    report.Reportujacy.StatusKonta = GlowneOkno.BazaDanych.Statusy.First(s => s.idStatusu == 2);
                    Logi nowylog = new Logi() { DataWystapienia = DateTime.UtcNow, Uzytkownicy = GlowneOkno.ZalogowanyAdministrator, Waznosc = 1, TrescWiadomosci = $"Administrator zbanowal uzytkownika {report.Reportujacy.idUzytkownika}" };
                    GlowneOkno.BazaDanych.Logi.Add(nowylog);
                    report.StatusRaportu = true;
                }
            }
            else if(report.Reportujacy.StatusKonta == GlowneOkno.BazaDanych.Statusy.First(s => s.idStatusu == 2))
            {
                result = MessageBox.Show("Czy chcesz odbanowac reportujacego uzytkownika?", "Odbanowac reportujacego", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    report.Reportujacy.StatusKonta = GlowneOkno.BazaDanych.Statusy.First(s => s.idStatusu == 1);
                    Logi nowylog = new Logi() { DataWystapienia = DateTime.UtcNow, Uzytkownicy = GlowneOkno.ZalogowanyAdministrator, Waznosc = 1, TrescWiadomosci = $"Administrator odbanowal uzytkownika {report.Reportujacy.idUzytkownika}" };
                    GlowneOkno.BazaDanych.Logi.Add(nowylog);
                    report.StatusRaportu = true;
                }
            }





            GlowneOkno.BazaDanych.SaveChanges();
        }

        private void btnZbanujReportowanego_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result;
            if (report.Recenzje.Uzytkownicy.StatusKonta == GlowneOkno.BazaDanych.Statusy.First(s => s.idStatusu == 1))
            {
                result = MessageBox.Show("Czy chcesz zbanowac reportowanego uzytkownika?", "Zbanowac reportowanego", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    report.Recenzje.Uzytkownicy.StatusKonta = GlowneOkno.BazaDanych.Statusy.First(s => s.idStatusu == 2);
                    Logi nowylog = new Logi() { DataWystapienia = DateTime.UtcNow, Uzytkownicy = GlowneOkno.ZalogowanyAdministrator, Waznosc = 1, TrescWiadomosci = $"Administrator zbanowal uzytkownika {report.Recenzje.Uzytkownicy.idUzytkownika}" };
                    GlowneOkno.BazaDanych.Logi.Add(nowylog);
                    report.StatusRaportu = true;
                }
            }
            else if(report.Recenzje.Uzytkownicy.StatusKonta == GlowneOkno.BazaDanych.Statusy.First(s => s.idStatusu == 2))
            {
                result = MessageBox.Show("Czy chcesz odbanowac reportowanego uzytkownika?", "Odbanowac reportowanego", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    report.Recenzje.Uzytkownicy.StatusKonta = GlowneOkno.BazaDanych.Statusy.First(s => s.idStatusu == 1);

                    Logi nowylog = new Logi() { DataWystapienia = DateTime.UtcNow, Uzytkownicy = GlowneOkno.ZalogowanyAdministrator, Waznosc = 1, TrescWiadomosci = $"Administrator odbanowal uzytkownika {report.Recenzje.Uzytkownicy.idUzytkownika}" };
                    GlowneOkno.BazaDanych.Logi.Add(nowylog);
                    report.StatusRaportu = true;
                }
            }


            GlowneOkno.BazaDanych.SaveChanges();
        }

        private void btnUkryjRecenzje_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result;
            if (report.Recenzje.Ukryta)
            {
                result = MessageBox.Show("Czy chcesz odblokowac recenzje?", "Pokaz recenzje", MessageBoxButton.YesNo);

            }
            else
            {
                result = MessageBox.Show("Czy chcesz zablokowac recenzje?", "Ukryj recenzje", MessageBoxButton.YesNo);
            }
            if (result == MessageBoxResult.Yes)
            {
                if (report.Recenzje.Ukryta)
                {
                    Logi nowylog = new Logi() { DataWystapienia = DateTime.UtcNow, Uzytkownicy = GlowneOkno.ZalogowanyAdministrator, TrescWiadomosci = $"Administrator pokazal recenzje {report.Recenzje.IDRecenzji}", Waznosc = 1 };
                    GlowneOkno.BazaDanych.Logi.Add(nowylog);
                }
                else
                {
                    Logi nowylog = new Logi() { DataWystapienia = DateTime.UtcNow, Uzytkownicy = GlowneOkno.ZalogowanyAdministrator, TrescWiadomosci = $"Administrator ukryl recenzje {report.Recenzje.IDRecenzji}", Waznosc = 1 };
                    GlowneOkno.BazaDanych.Logi.Add(nowylog);
                }
                report.Recenzje.Ukryta = !report.Recenzje.Ukryta;
                report.StatusRaportu = true;
                GlowneOkno.BazaDanych.SaveChanges();
            }

        }

        private void btnNicNieRob_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Czy chcesz oznaczyc zgloszenie jako rozwiazane?", "Oznacz jako rozwiazane", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                report.StatusRaportu = true;
                btnPowrot_Click(this, new RoutedEventArgs());
            }
        }
    }
}
