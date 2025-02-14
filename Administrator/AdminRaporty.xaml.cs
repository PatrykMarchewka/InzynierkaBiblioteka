using InżynierkaBiblioteka.BazaDanych;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace InżynierkaBiblioteka.Administrator
{
    /// <summary>
    /// Interaction logic for AdminRaporty.xaml
    /// </summary>
    public partial class AdminRaporty : Page
    {
        public AdminRaporty()
        {
            InitializeComponent();
        }

        private void btnPowrot_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.GlownaRamka.GoBack();
        }

        private void btnRaportUzytkownicy_Click(object sender, RoutedEventArgs e)
        {
            HashSet<Uzytkownicy> hashset = GlowneOkno.BazaDanych.Uzytkownicy.ToHashSet();

            try
            {
                using (StreamWriter writer = new StreamWriter($"{Directory.GetCurrentDirectory()}\\Uzytkownicy - {DateTime.UtcNow.ToString("dd-MM-yyyy HH-mm-ss")}.csv",false,Encoding.UTF8))
                {
                    writer.WriteLine("ID,Kod RFID,Login,Haslo,Sol,Email,Imie,Nazwisko,Nr Telefonu,Liczba aktualnie wypozyczonych ksiazek,Plec,Rola,Status,Data stworzenia konta, Data ostatniego logowania");
                    foreach (var item in hashset)
                    {
                        writer.WriteLine($"{item.idUzytkownika},{item.RFID},{item.LoginUzytkownika},{item.hashHaslo},{item.salt},{item.email},{item.Imie},{item.Nazwisko},{item.nrTelefonu},{item.LiczbaWypozyczonychKsiazek},{item.Plec.Nazwa},{item.Rola.Nazwa},{item.StatusKonta.Nazwa},{item.DataStworzeniaKonta},{item.DataOstatniegoLogowania}");
                    }
                }
                Logi log = new Logi() { DataWystapienia = DateTime.UtcNow, TrescWiadomosci = $"Administrator eksportowal dane uzytkownikow", Uzytkownicy = GlowneOkno.ZalogowanyAdministrator, Waznosc = 1 };
                GlowneOkno.BazaDanych.Logi.Add(log);
                GlowneOkno.BazaDanych.SaveChanges();
            }
            catch (Exception ex)
            {
                Logi log = new Logi() { DataWystapienia = DateTime.UtcNow, TrescWiadomosci = $"Administrator próbował wyeksportowac dane uzytkownikow", Uzytkownicy = GlowneOkno.ZalogowanyAdministrator, Waznosc = 10 };
                GlowneOkno.BazaDanych.Logi.Add(log);
                GlowneOkno.BazaDanych.SaveChanges();
            }
        }

        private void btnRaportKsiazki_Click(object sender, RoutedEventArgs e)
        {
            HashSet<Ksiazki> hashset = GlowneOkno.BazaDanych.Ksiazki.ToHashSet();


            try
            {
                using (StreamWriter writer = new StreamWriter($"{Directory.GetCurrentDirectory()}\\Ksiazki - {DateTime.UtcNow.ToString("dd-MM-yyyy HH-mm-ss")}.csv", false, Encoding.UTF8))
                {
                    writer.WriteLine("ID,ISBN,Tytul,Gatunek,Rok publikacji,Jezyk,Ilosc stron,Dostepnosc,Liczba Oczekujacych,Do wypozyczenia,Ilosc wypozyczen w tym miesiacu");
                    foreach (var item in hashset)
                    {
                        writer.WriteLine($"{item.idKsiazki},{item.ISBN},{item.TytulKsiazki.Replace(",",".")},{item.GatunekKsiazki.Nazwa},{item.RokPublikacjiKsiazki},{item.JezykKsiazki.Nazwa},{item.IloscStron},{item.DostepnoscKsiazki},{item.LiczbaOczekujacych},{item.DoWypozyczenia.ToString()},{item.IloscWypozyczen30Dni}");
                    }
                }
                Logi log = new Logi() { DataWystapienia = DateTime.UtcNow, TrescWiadomosci = $"Administrator eksportowal dane ksiazek", Uzytkownicy = GlowneOkno.ZalogowanyAdministrator, Waznosc = 1 };
                GlowneOkno.BazaDanych.Logi.Add(log);
                GlowneOkno.BazaDanych.SaveChanges();

            }
            catch (Exception ex)
            {
                Logi log = new Logi() { DataWystapienia = DateTime.UtcNow, TrescWiadomosci = $"Administrator próbował wyeksportowac dane ksiazek", Uzytkownicy = GlowneOkno.ZalogowanyAdministrator, Waznosc = 10 };
                GlowneOkno.BazaDanych.Logi.Add(log);
                GlowneOkno.BazaDanych.SaveChanges();
            }
            
        }

        private void btnRaportWypozyczenia_Click(object sender, RoutedEventArgs e)
        {
            HashSet<Wypozyczenia> hashset = GlowneOkno.BazaDanych.Wypozyczenia.ToHashSet();
            try
            {
                using (StreamWriter writer = new StreamWriter($"{Directory.GetCurrentDirectory()}\\Wypozyczenia - {DateTime.UtcNow.ToString("dd-MM-yyyy HH-mm-ss")}.csv",false,Encoding.UTF8))
                {
                    writer.WriteLine("ID,Uzytkownik,Tytul Ksiazki,Data wypozyczenia,Data oddania,Data do oddania");
                    foreach (var item in hashset)
                    {
                        writer.WriteLine($"{item.idWypozyczenia},{item.Uzytkownicy.LoginUzytkownika},{item.Ksiazka.TytulKsiazki.Replace(",", ".")},{item.DataWypozyczenia},{item.DataAktualnegoOddania},{item.DataDoOddania}");
                    }
                }
                Logi log = new Logi() { DataWystapienia = DateTime.UtcNow, TrescWiadomosci = $"Administrator eksportowal dane wypozyczen", Uzytkownicy = GlowneOkno.ZalogowanyAdministrator, Waznosc = 1 };
                GlowneOkno.BazaDanych.Logi.Add(log);
                GlowneOkno.BazaDanych.SaveChanges();
            }
            catch (Exception ex)
            {
                Logi log = new Logi() { DataWystapienia = DateTime.UtcNow, TrescWiadomosci = $"Administrator próbował wyeksportowac dane wypozyczen", Uzytkownicy = GlowneOkno.ZalogowanyAdministrator, Waznosc = 10 };
                GlowneOkno.BazaDanych.Logi.Add(log);
                GlowneOkno.BazaDanych.SaveChanges();
            }
        }

        private void btnRaportZaleglosci_Click(object sender, RoutedEventArgs e)
        {
            HashSet<Zaleglosci> hashset = GlowneOkno.BazaDanych.Zaleglosci.ToHashSet();
            try
            {
                using (StreamWriter writer = new StreamWriter($"{Directory.GetCurrentDirectory()}\\Zaleglosci - {DateTime.UtcNow.ToString("dd-MM-yyyy HH-mm-ss")}.csv",false,Encoding.UTF8))
                {
                    writer.WriteLine("ID,Uzytkownik,Ksiazka,Zaleglosc,Komentarz,Zaplacono");
                    foreach (var item in hashset)
                    {
                        writer.WriteLine($"{item.idZaleglosci},{item.Uzytkownicy.LoginUzytkownika},{(item.Ksiazka != null ?item.Ksiazka.TytulKsiazki.Replace(",", ".") : "(BRAK)")},{item.Zaleglosc.ToString().Replace(",", ".")},{item.Komentarz.Replace(",", ".")},{item.Zaplacono.ToString()}");
                    }
                }
                Logi log = new Logi() { DataWystapienia = DateTime.UtcNow, TrescWiadomosci = $"Administrator eksportowal dane zaleglosci", Uzytkownicy = GlowneOkno.ZalogowanyAdministrator, Waznosc = 1 };
                GlowneOkno.BazaDanych.Logi.Add(log);
                GlowneOkno.BazaDanych.SaveChanges();
            }
            catch (Exception ex)
            {
                Logi log = new Logi() { DataWystapienia = DateTime.UtcNow, TrescWiadomosci = $"Administrator próbował wyeksportowac dane zaleglosci", Uzytkownicy = GlowneOkno.ZalogowanyAdministrator, Waznosc = 10 };
                GlowneOkno.BazaDanych.Logi.Add(log);
                GlowneOkno.BazaDanych.SaveChanges();
            }
        }
    }
}
