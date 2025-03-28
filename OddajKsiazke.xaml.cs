﻿using InżynierkaBiblioteka.BazaDanych;
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
    /// Interaction logic for OddajKsiazke.xaml
    /// </summary>
    public partial class OddajKsiazke : Page
    {
        public OddajKsiazke()
        {
            InitializeComponent();
            grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            foreach (var item in GlowneOkno.ZalogowanyUzytkownik.Wypozyczenia.Where(w => w.DataAktualnegoOddania == null))
            {
                grid.RowDefinitions.Add(new RowDefinition() { Height= GridLength.Auto});
                Button button = new Button() { Content= item.Ksiazka.TytulKsiazki };
                button.Click += (s, e) => btnButton_Click(s, e, item);
                Grid.SetRow(button, grid.RowDefinitions.Count-1);
                grid.Children.Add(button);
            }
        }

        private void btnPowrot_Click(object sender, RoutedEventArgs e)
        {
            grid.Children.Clear();
            MainWindow.GlownaRamka.GoBack();
        }

        private void btnButton_Click(object sender, RoutedEventArgs e, Wypozyczenia w)
        {
            w.DataAktualnegoOddania = DateTime.UtcNow;
            if (w.DataAktualnegoOddania > w.DataDoOddania)
            {
                TimeSpan data = w.DataAktualnegoOddania.Value - w.DataDoOddania;
                Zaleglosci nowaZaleglosc = new Zaleglosci() { Ksiazka = w.Ksiazka, Uzytkownicy = GlowneOkno.ZalogowanyUzytkownik, Zaleglosc = 10 * data.Days, Zaplacono = false, Komentarz = "Oddanie ksiazki po czasie" };

                GlowneOkno.ZalogowanyUzytkownik.WszystkieZaleglosci.Add(nowaZaleglosc);

                Logi nowyLog = new Logi() { DataWystapienia = DateTime.UtcNow, TrescWiadomosci = $"Uzytkownik oddal ksiazke po czasie i zostala policzona kara {(10 * data.Days):C}", Uzytkownicy = GlowneOkno.ZalogowanyUzytkownik, Waznosc = 1 };
                GlowneOkno.ZalogowanyUzytkownik.WszystkieLogi.Add(nowyLog);

                MessageBox.Show($"Ksiazka zostala oddana po czasie, naliczono kare: {(10 * data.Days):C}. Nie mozesz wypozyczac wiecej ksiazek dopoki nie uregulujesz tej platnosci");
            }
            GlowneOkno.ZalogowanyUzytkownik.LiczbaWypozyczonychKsiazek--;
            MessageBox.Show($"Oddano ksiazke! {w.Ksiazka.TytulKsiazki}");
            
            w.Ksiazka.DostepnoscKsiazki++;

            if (w.Ksiazka.LiczbaOczekujacych > 0)
            {
                Powiadomienia? p = GlowneOkno.BazaDanych.Powiadomienia.FirstOrDefault(p => p.Ksiazka == w.Ksiazka && p.KiedyWyslanoMail == null);
                if (p != null)
                {
                    WysylanieMaili.LogowanieDoMaila();
                    WysylanieMaili.WysylanieWiadomosciEmail(p.Uzytkownicy.email, "Powiadomienie o dostępności książki", $"Otrzymaliśmy dostawę z twoją książką\nKsiążka: {p.Ksiazka.TytulKsiazki} jest teraz dostępna w naszej bibliotece");
                    p.KiedyWyslanoMail = DateTime.UtcNow;
                    p.Ksiazka.LiczbaOczekujacych--;
                }

            }
            GlowneOkno.BazaDanych.SaveChanges();

            grid.Children.Remove((Button)sender);

        }
    }
}
