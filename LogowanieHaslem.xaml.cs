using InżynierkaBiblioteka.BazaDanych;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
    /// Interaction logic for LogowanieHaslem.xaml
    /// </summary>
    public partial class LogowanieHaslem : Page
    {
        public LogowanieHaslem()
        {
            InitializeComponent();
            this.Loaded += LogowanieHaslem_Loaded;
        }

        private void LogowanieHaslem_Loaded(object sender, RoutedEventArgs e)
        {
            txtBoxZalogujHaslemLogin.Text = String.Empty;
            txtBoxZalogujHaslemHaslo.Text = String.Empty;
        }

        private static bool WeryfikacjaHasla(string Haslo, string Hash, string SolString = "BibliotekaInzynieria")
        {
            byte[] SolIHaslo = Encoding.UTF8.GetBytes(SolString + Haslo);

            byte[] HashDoSprawdzenia;

            using (HashAlgorithm hs = SHA256.Create())
            {
                HashDoSprawdzenia = hs.ComputeHash(SolIHaslo);
            }

            StringBuilder HashJakoString = new StringBuilder();
            for (int i = 0; i < HashDoSprawdzenia.Length; i++)
            {
                HashJakoString.Append(HashDoSprawdzenia[i].ToString("x2"));
            }

            StringComparer stringComparer = StringComparer.OrdinalIgnoreCase;
            if (stringComparer.Compare(Hash, HashJakoString.ToString()) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            string Login = txtBoxZalogujHaslemLogin.Text;
            string Haslo = txtBoxZalogujHaslemHaslo.Text;


                Uzytkownicy? proba = null;
                try
                {
                    proba = GlowneOkno.BazaDanych.Uzytkownicy.First(u => u.LoginUzytkownika == Login);
                }
                catch (Exception)
                {
                    MessageBox.Show("Nie znaleziono takiego Uzytkownika, sprawdz swoj login");
                }
                

                if(proba != null)
                {

                    if (proba.StatusKonta.idStatusu == 2)
                    {
                    MessageBox.Show("Blad! Konto zbanowane, skontaktuj sie z administratoem");
                    proba = null;
                    MainWindow.GlownaRamka.GoBack();
                    }
                    else
                    {
                        string hash = proba.hashHaslo;
                        if (WeryfikacjaHasla(Haslo, hash))
                        {
                            GlowneOkno.ZalogowanyUzytkownik = proba;
                            proba = null;
                        GlowneOkno.ZalogowanyUzytkownik.DataOstatniegoLogowania = DateTime.UtcNow;
                        GlowneOkno.BazaDanych.SaveChanges();
                            if (GlowneOkno.ZalogowanyUzytkownik.Rola.idRoli == 1)
                            {
                                MainWindow.Nawigacja("PoZalogowaniuUzytkownik.xaml");
                            }
                            else if (GlowneOkno.ZalogowanyUzytkownik.Rola.idRoli == 2)
                            {
                                GlowneOkno.ZalogowanyAdministrator = GlowneOkno.ZalogowanyUzytkownik;
                                GlowneOkno.ZalogowanyUzytkownik = null;
                            GlowneOkno.ZalogowanyAdministrator.DataOstatniegoLogowania = DateTime.UtcNow;
                            GlowneOkno.BazaDanych.SaveChanges();
                                MainWindow.Nawigacja("PoZalogowaniuAdmin.xaml");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Zle haslo, sprobuj ponownie");
                        }
                    }

                    
                }
        }

        private void btnPowrot_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.GlownaRamka.GoBack();
        }

        private void btnOdzyskajKonto_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Nawigacja("OdzyskajKonto.xaml");
        }


    }
}
