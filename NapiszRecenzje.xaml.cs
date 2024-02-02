using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for NapiszRecenzje.xaml
    /// </summary>
    public partial class NapiszRecenzje : Page
    {
        int ocena;
        public NapiszRecenzje()
        {
            InitializeComponent();
        }

        private void btnGwiazda_Click(object sender, RoutedEventArgs e)
        {
            SolidColorBrush white = new SolidColorBrush(Colors.White);
            SolidColorBrush black = new SolidColorBrush(Colors.Black);
            if (sender is Button btn)
            {
                switch (btn.Name)
                {
                    case "btnGwiazda1":
                        btnGwiazda1.Foreground = white;
                        btnGwiazda2.Foreground = black;
                        btnGwiazda3.Foreground = black;
                        btnGwiazda4.Foreground = black;
                        btnGwiazda5.Foreground = black;
                        ocena = 1;
                        break;
                    case "btnGwiazda2":
                        btnGwiazda1.Foreground = white;
                        btnGwiazda2.Foreground = white;
                        btnGwiazda3.Foreground = black;
                        btnGwiazda4.Foreground = black;
                        btnGwiazda5.Foreground = black;
                        ocena = 2;
                        break;
                    case "btnGwiazda3":
                        btnGwiazda1.Foreground = white;
                        btnGwiazda2.Foreground = white;
                        btnGwiazda3.Foreground = white;
                        btnGwiazda4.Foreground = black;
                        btnGwiazda5.Foreground = black;
                        ocena = 3;
                        break;
                    case "btnGwiazda4":
                        btnGwiazda1.Foreground = white;
                        btnGwiazda2.Foreground = white;
                        btnGwiazda3.Foreground = white;
                        btnGwiazda4.Foreground = white;
                        btnGwiazda5.Foreground = black;
                        ocena = 4;
                        break;
                    case "btnGwiazda5":
                        btnGwiazda1.Foreground = white;
                        btnGwiazda2.Foreground = white;
                        btnGwiazda3.Foreground = white;
                        btnGwiazda4.Foreground = white;
                        btnGwiazda5.Foreground = white;
                        ocena = 5;
                        break;


                    default:
                        break;
                }
            }
        }

        private void btnDodajRecenzje_Click(object sender, RoutedEventArgs e)
        {
            if (btnGwiazda1.Foreground == new SolidColorBrush(Colors.White))
            {
                MessageBox.Show("Blad! Wybierz ocene przed dodaniem recenzji");
            }
            else
            {
                GlowneOkno.ZalogowanyUzytkownik.Recenzje.Add(new BazaDanych.Recenzje
                {
                    DataWystawienia = DateTime.UtcNow,
                    Ocena = ocena,
                    TekstRecenzji = txtBoxRecenzja.Text,
                    Ukryta = false,
                    Uzytkownik = GlowneOkno.ZalogowanyUzytkownik,
                    Ksiazka = PokazKsiazke.PokazKsiazkeKsiazka
                });
                GlowneOkno.BazaDanych.SaveChanges();
                MainWindow.GlownaRamka.GoBack();
            }
        }

        private void btnPowrot_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.GlownaRamka.GoBack();
        }
    }
}
