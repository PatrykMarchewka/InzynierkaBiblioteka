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

namespace InżynierkaBiblioteka.Administrator
{
    /// <summary>
    /// Interaction logic for ZobaczReporty.xaml
    /// </summary>
    public partial class ZobaczReporty : Page
    {
        HashSet<Reporty> Lista;
        public ZobaczReporty()
        {
            InitializeComponent();
            Wyszukaj();
        }

        private void Wyszukaj()
        {
            Stack.Children.Clear();
            Stack.BeginInit();
            if (chkBoxRozwiazane.IsChecked == true)
            {
                Lista = GlowneOkno.BazaDanych.Reporty.OrderBy(r => r.IDReportu).ToHashSet();
            }
            else
            {
                Lista = GlowneOkno.BazaDanych.Reporty.Where(r => r.StatusRaportu == false).OrderBy(r => r.IDReportu).ToHashSet();
            }

            if (!String.IsNullOrWhiteSpace(txtBoxUzytkownik.Text))
            {
                var queryText = txtBoxUzytkownik.Text.ToLower();

                Lista = Lista.Where(r =>
                    r.Reportujacy.Imie.ToLower().Contains(queryText) ||
                    r.Reportujacy.Nazwisko.ToLower().Contains(queryText) ||
                    r.Reportujacy.LoginUzytkownika.ToLower().Contains(queryText) ||
                    r.TrescRaportu.ToLower().Contains(queryText) ||
                    r.Recenzje.Uzytkownicy.Imie.ToLower().Contains(queryText) ||
                    r.Recenzje.Uzytkownicy.Nazwisko.ToLower().Contains(queryText) ||
                    r.Recenzje.Uzytkownicy.LoginUzytkownika.ToLower().Contains(queryText)
                ).OrderBy(r => r.IDReportu).ToHashSet();
            }

            foreach (var item in Lista)
            {
                var KopiaItemu = item;
                Button button = new Button();
                button.Content = $"{KopiaItemu.IDReportu} - {KopiaItemu.Reportujacy.LoginUzytkownika} {KopiaItemu.TrescRaportu} | {KopiaItemu.Recenzje.Uzytkownicy.LoginUzytkownika}";
                button.Margin = new Thickness(5);
                if (!KopiaItemu.StatusRaportu)
                {
                    button.Background = new SolidColorBrush(Colors.Red);
                }
                else
                {
                    button.Background = new SolidColorBrush(Colors.Black);
                }
                button.Foreground = new SolidColorBrush(Colors.White);

                button.Click += (s, e) => Button_Click(s, e, KopiaItemu);
                Stack.Children.Add(button);
            }
            Stack.EndInit();


        }

        private void Button_Click(object sender, RoutedEventArgs e, Reporty r)
        {
            PokazRaport.report = r;
            MainWindow.Nawigacja("Administrator/PokazRaport.xaml");
        }

        private void btnPowrot_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.GlownaRamka.GoBack();
        }

        private void chkBoxRozwiazane_Checked(object sender, RoutedEventArgs e)
        {
            Wyszukaj();
        }

        private void txtBoxUzytkownik_TextChanged(object sender, TextChangedEventArgs e)
        {
            Wyszukaj();
        }
    }
}
