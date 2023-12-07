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


                Uzytkownik? proba = null;
                try
                {
                    proba = GlowneOkno.BazaDanych.Uzytkownik.First(u => u.LoginUzytkownika == Login);
                }
                catch (Exception)
                {
                    MessageBox.Show("Nie znaleziono takiego uzytkownika, sprawdz swoj login");
                }
                

                if(proba != null)
                {
                    string hash = proba.hashHaslo;
                    if (WeryfikacjaHasla(Haslo,hash))
                    {
                        GlowneOkno.ZalogowanyUzytkownik = proba;
                        proba = null;
                        if (GlowneOkno.ZalogowanyUzytkownik.Rola.idRoli == 1)
                        {
                            MainWindow.Nawigacja("PoZalogowaniuUzytkownik.xaml");
                        }
                        else if (GlowneOkno.ZalogowanyUzytkownik.Rola.idRoli == 2)
                        {
                            MainWindow.Nawigacja("PoZalogowaniuAdmin.xaml");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Zle haslo, sprobuj ponownie");
                    }
                }
        }

        private void btnPowrot_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.GlownaRamka.GoBack();
        }
    }
}
