using InżynierkaBiblioteka.BazaDanych;
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
        int ocena = 0;
        public static Recenzje? IstniejacaRecenzja;
        Button[] ocenyArray;

        public NapiszRecenzje()
        {
            InitializeComponent();
            ocenyArray = new[] { btnGwiazda1,btnGwiazda2,btnGwiazda3,btnGwiazda4,btnGwiazda5 };
            if (IstniejacaRecenzja != null)
            {
                txtBoxRecenzja.Text = IstniejacaRecenzja.TekstRecenzji;
                ocena = IstniejacaRecenzja.Ocena;
                btnGwiazda_Click(ocenyArray[ocena-1], new RoutedEventArgs());
                
            }
            
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
                        ocena = 1;
                        break;
                    case "btnGwiazda2":
                        ocena = 2;
                        break;
                    case "btnGwiazda3":
                        ocena = 3;
                        break;
                    case "btnGwiazda4":
                        ocena = 4;
                        break;
                    case "btnGwiazda5":
                        ocena = 5;
                        break;


                    default:
                        break;
                }

                for (int i = 0; i < 5; i++)
                {
                    if (ocena > i)
                    {
                        ocenyArray[i].Foreground = white;
                    }
                    else
                    {
                        ocenyArray[i].Foreground = black;
                    }
                }
            }
        }

        private void btnDodajRecenzje_Click(object sender, RoutedEventArgs e)
        {
            if (btnGwiazda1.Foreground == new SolidColorBrush(Colors.White))
            {
                MessageBox.Show("Blad! Wybierz ocene przed dodaniem recenzji");
            }
            else if (txtBoxRecenzja.Text.Length >= 512)
            {
                MessageBox.Show("Blad! Tresc recenzji zbyt dluga");
            }
            else
            {
                if (IstniejacaRecenzja != null)
                {
                    IstniejacaRecenzja.DataWystawienia = DateTime.UtcNow;
                    IstniejacaRecenzja.Ocena = ocena;
                    IstniejacaRecenzja.TekstRecenzji = txtBoxRecenzja.Text;
                    IstniejacaRecenzja.Ukryta = false;
                }
                else
                {
                    GlowneOkno.ZalogowanyUzytkownik.Recenzje.Add(new BazaDanych.Recenzje
                    {
                        DataWystawienia = DateTime.UtcNow,
                        Ocena = ocena,
                        TekstRecenzji = txtBoxRecenzja.Text,
                        Ukryta = false,
                        Uzytkownicy = GlowneOkno.ZalogowanyUzytkownik,
                        Ksiazka = PokazKsiazke.PokazKsiazkeKsiazka
                    });
                }


                GlowneOkno.BazaDanych.SaveChanges();
                MainWindow.GlownaRamka.GoBack();
            }
        }

        private void btnPowrot_Click(object sender, RoutedEventArgs e)
        {
            IstniejacaRecenzja = null;
            MainWindow.GlownaRamka.GoBack();
        }
    }
}
