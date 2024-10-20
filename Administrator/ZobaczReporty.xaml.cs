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
                Lista = Lista.Where(r => EF.Functions.Like(r.Reportujacy.Imie, $"%{txtBoxUzytkownik.Text}%") || EF.Functions.Like(r.Reportujacy.Nazwisko, $"%{txtBoxUzytkownik.Text}%") || EF.Functions.Like(r.TrescRaportu, $"%{txtBoxUzytkownik.Text}%") || EF.Functions.Like(r.Recenzje.Uzytkownicy.Imie, $"%{txtBoxUzytkownik.Text}%") || EF.Functions.Like(r.Recenzje.Uzytkownicy.Nazwisko, $"%{txtBoxUzytkownik.Text}%")).OrderBy(r => r.IDReportu).ToHashSet();
            }

            foreach (var item in Lista)
            {
                var KopiaItemu = item;
                Button button = new Button();
                button.Content = $"{KopiaItemu.IDReportu} - {KopiaItemu.TrescRaportu}";
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
            //TODO: Przejscie na strone raportu
            //PokazRaport.raport = r
            //MainWindow.Nawigacja("PokazRaport.xaml");
        }

        private void btnPowrot_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.GlownaRamka.GoBack();
        }
    }
}
