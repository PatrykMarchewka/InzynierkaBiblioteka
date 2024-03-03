using InżynierkaBiblioteka.BazaDanych;
using Microsoft.EntityFrameworkCore;
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
    /// Interaction logic for PowiazAutorzyKsiazki.xaml
    /// </summary>
    public partial class PowiazAutorzyKsiazki : Page
    {
        public static Autorzy? WybranyAutor;
        public static Ksiazki? WybranaKsiazka;
        public PowiazAutorzyKsiazki()
        {
            InitializeComponent();
            comboKsiazki.ItemsSource = GlowneOkno.BazaDanych.Ksiazki.OrderBy(k => k.TytulKsiazki).ToHashSet();
            comboKsiazki.SelectedItem = WybranaKsiazka;
            comboAutorzy.ItemsSource = GlowneOkno.BazaDanych.Autorzy.OrderBy(a => a.NazwiskoAutora).ToHashSet();
        }

        private void btnPowiaz_Click(object sender, RoutedEventArgs e)
        {
            if (GlowneOkno.BazaDanych.HashKsiazkiAutorzy.Any(h => h.Ksiazka == WybranaKsiazka && h.Autor == WybranyAutor))
            {
                MessageBoxResult result = MessageBox.Show("Czy chcesz usunac dotychczasowe powiazanie?", "Znaleziono powiazanie", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    HashKsiazkiAutorzy hash = GlowneOkno.BazaDanych.HashKsiazkiAutorzy.First(h => h.Ksiazka == WybranaKsiazka && h.Autor == WybranyAutor);
                    GlowneOkno.BazaDanych.HashKsiazkiAutorzy.Remove(hash);
                    Logi nowyLog = new Logi() { DataWystapienia = DateTime.UtcNow, Uzytkownicy = GlowneOkno.ZalogowanyAdministrator, Waznosc = 1, TrescWiadomosci = $"Administrator usunal polaczenie {hash.Ksiazka.TytulKsiazki} i {hash.Autor.ImieAutora} {hash.Autor.NazwiskoAutora}" };
                    GlowneOkno.BazaDanych.Logi.Add(nowyLog);
                }
                
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Czy chcesz dodac nowe powiazanie?", "Nieznaleziono powiazania", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    HashKsiazkiAutorzy hash = new HashKsiazkiAutorzy() { Autor = WybranyAutor, Ksiazka = WybranaKsiazka, idAutora = WybranyAutor.idAutora, idKsiazki = WybranaKsiazka.idKsiazki };
                    GlowneOkno.BazaDanych.HashKsiazkiAutorzy.Add(hash);
                    Logi nowyLog = new Logi() { DataWystapienia = DateTime.UtcNow, Uzytkownicy = GlowneOkno.ZalogowanyAdministrator, Waznosc = 1, TrescWiadomosci = $"Administrator dodal polaczenie {hash.Ksiazka.TytulKsiazki} i {hash.Autor.ImieAutora} {hash.Autor.NazwiskoAutora}" };
                    GlowneOkno.BazaDanych.Logi.Add(nowyLog);
                }
                
            }
            GlowneOkno.BazaDanych.SaveChanges();
        }

        private void txtBoxKsiazki_TextChanged(object sender, TextChangedEventArgs e)
        {
            comboKsiazki.ItemsSource = GlowneOkno.BazaDanych.Ksiazki.Where(k => EF.Functions.Like(k.TytulKsiazki, $"%{txtBoxKsiazki.Text}%") || EF.Functions.Like(k.ISBN, $"%{txtBoxKsiazki.Text}%")).OrderBy(k => k.TytulKsiazki).ToHashSet();

        }

        private void txtBoxAutorzy_TextChanged(object sender, TextChangedEventArgs e)
        {
            comboAutorzy.ItemsSource = GlowneOkno.BazaDanych.Autorzy.Where(a => EF.Functions.Like(a.ImieAutora, $"%{txtBoxAutorzy.Text}%") || EF.Functions.Like(a.NazwiskoAutora, $"%{txtBoxAutorzy.Text}%")).OrderBy(a => a.NazwiskoAutora).ToHashSet();

        }

        private void btnPowrot_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.GlownaRamka.GoBack();
        }

        private void comboAutorzy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            WybranyAutor = (Autorzy)comboAutorzy.SelectedItem;
        }

        private void comboKsiazki_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            WybranaKsiazka = (Ksiazki)comboKsiazki.SelectedItem;
        }
    }
}
