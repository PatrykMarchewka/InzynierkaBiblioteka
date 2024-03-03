using InżynierkaBiblioteka.BazaDanych;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Printing.IndexedProperties;
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
    /// Interaction logic for StworzNoweKonto.xaml
    /// </summary>
    public partial class StworzNoweKonto : Page
    {
        private static int WybranaPlecID = 1;
        private static List<string> ListaPlci = new List<string>();
        public StworzNoweKonto()
        {
            InitializeComponent();
            try
            {

                    foreach (var item in GlowneOkno.BazaDanych.Plec.OrderBy(x => x.idPlci))
                    {
                        ListaPlci.Add(item.Nazwa);
                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Blad! {ex.Message}");
                MainWindow.Nawigacja("GlowneOkno.xaml");
            }
            
            comboBoxPlec.ItemsSource = ListaPlci;
            comboBoxPlec.SelectedIndex = 0;
        }

        public static string StworzHash(string Haslo, string SolString = "BibliotekaInzynieria")
        {
            byte[] SolIHaslo = Encoding.UTF8.GetBytes(SolString + Haslo);
            byte[] Hash;
            using(HashAlgorithm hs = SHA256.Create())
            {
                Hash = hs.ComputeHash(SolIHaslo);
            }
            StringBuilder HashJakoString = new StringBuilder();
            for (int i = 0; i < Hash.Length; i++)
            {
                HashJakoString.Append(Hash[i].ToString("x2"));
            }

            return HashJakoString.ToString();
        }

        private void comboBoxPlec_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*
            if (comboBoxPlec.SelectedItem.ToString() == "Inna")
            {
                txtBoxPlec.Visibility = Visibility.Visible;
            }
            else
            {
                txtBoxPlec.Visibility = Visibility.Hidden;
                WybranaPlecID = comboBoxPlec.SelectedIndex + 1;
            }
            */
            
        }

        private void btnStworzUzytkownika_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(txtBoxHaslo.Text) || String.IsNullOrEmpty(txtBoxImie.Text) || String.IsNullOrEmpty(txtBoxLogin.Text) || String.IsNullOrEmpty(txtBoxNazwisko.Text) || (comboBoxPlec.SelectedItem.ToString() == "Inna" && String.IsNullOrEmpty(txtBoxPlec.Text)) || lblZnakA.Visibility == Visibility.Visible)
            {
                MessageBox.Show("Blad! Wypelnij wszystkie wymagane pola");
            }
            else if (GlowneOkno.BazaDanych.Uzytkownicy.Any(u => u.LoginUzytkownika == txtBoxLogin.Text))
            {
                MessageBox.Show("Blad! Niedostępna nazwa użytkownika (Login)");
            }
            else
            {
                //if (comboBoxPlec.SelectedItem.ToString() == "Inna")
                //{
                //    try
                //    {
                        
                //            string tempPlec = txtBoxPlec.Text;
                //            tempPlec = tempPlec.ToLower();
                //            tempPlec = char.ToUpper(tempPlec[0]) + tempPlec.Substring(1);
                //            GlowneOkno.BazaDanych.Plec.Add(new Plec() { Nazwa = tempPlec });
                //        GlowneOkno.BazaDanych.SaveChanges();
                //            WybranaPlecID = GlowneOkno.BazaDanych.Plec.First(p => p.Nazwa == tempPlec).idPlci;
                //    }
                //    catch (Exception ex)
                //    {
                //        MessageBox.Show($"Blad! {ex.Message}");
                //        MainWindow.Nawigacja("GlowneOkno.xaml");
                //    }
                    
                //}

                try
                {
                        Plec tempPlec = GlowneOkno.BazaDanych.Plec.First(p => p.idPlci == WybranaPlecID);
                        Role tempRola = GlowneOkno.BazaDanych.Role.First(r => r.idRoli == 1);
                        Statusy tempStatus = GlowneOkno.BazaDanych.Statusy.First(s => s.idStatusu == 1);

                        Uzytkownicy nowy = new Uzytkownicy() { LoginUzytkownika = txtBoxLogin.Text, hashHaslo = StworzHash(txtBoxHaslo.Text),Imie = txtBoxImie.Text, Nazwisko = txtBoxNazwisko.Text, DataStworzeniaKonta = DateTime.UtcNow, Plec = tempPlec, Rola =tempRola, StatusKonta = tempStatus, DataOstatniegoLogowania = DateTime.UtcNow };
                        if (!String.IsNullOrEmpty(txtBoxEmail.Text))
                        {
                            nowy.email = txtBoxEmail.Text;
                        }
                        if (!String.IsNullOrEmpty(txtBoxNrTelefonu.Text))
                        {
                        //TODO: Walidacja nr telefonu, tutaj i przy zmianie danych
                            nowy.nrTelefonu = txtBoxNrTelefonu.Text;
                        }

                    GlowneOkno.BazaDanych.Uzytkownicy.Add(nowy);
                    GlowneOkno.BazaDanych.SaveChanges();
                        MessageBox.Show("Dodano Uzytkownika");
                        GlowneOkno.ZalogowanyUzytkownik = nowy;
                        nowy = null;
                    MainWindow.GlownaRamka.GoBack();
                }
                catch (Exception ex)
                {
                    Logi nowyLog = new Logi() { DataWystapienia = DateTime.UtcNow, TrescWiadomosci = $"Nie mozna stworzyc uzytkownika {ex.Message} - {ex.InnerException}", Uzytkownicy = null, Waznosc = 10 };
                    MessageBox.Show($"Blad! Nie mozna stworzyc uzytkownika");
                    MainWindow.Nawigacja("GlowneOkno.xaml");
                }
            }
        }

        private void btnPowrot_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.GlownaRamka.GoBack();
        }

        private void txtBoxEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (String.IsNullOrEmpty(txtBoxEmail.Text) || txtBoxEmail.Text.Contains('@'))
            {
                lblZnakA.Visibility = Visibility.Hidden;
            }
            else
            {
                lblZnakA.Visibility = Visibility.Visible;
            }
        }
    }
}
