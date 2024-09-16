using InżynierkaBiblioteka.BazaDanych;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Printing.IndexedProperties;
using System.Security;
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




        //Najdluzszy nr telefonu ma 19 znakow, jest to +999123451234567890 dlatego maks 20 znakow na nr tel




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

        //TODO: Nowa sol i hash co jakis interwal czasowy, dodac pole ostatniej zmiany do tabeli Uzytkownicy
        public static string LosowaSol()
        {
            Random rnd = new Random();
            int length = rnd.Next(255);
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            string wynik = new string(Enumerable.Repeat(chars, length).Select(s => s[rnd.Next(s.Length)]).ToArray());
            return wynik;
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
            else if (txtBoxNrTelefonu.Text.Length > 20 || txtBoxHaslo.Text.Length > 255 || txtBoxLogin.Text.Length > 255 || txtBoxImie.Text.Length > 255 || txtBoxNazwisko.Text.Length > 255 || txtBoxEmail.Text.Length > 255)
            {
                MessageBox.Show("Blad! Jedno lub wiecej pol jest za dlugie, sprawdz wszystkie pola");
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
                    string temp = LosowaSol();
                    Uzytkownicy nowy = new Uzytkownicy() { LoginUzytkownika = txtBoxLogin.Text, hashHaslo = StworzHash(txtBoxHaslo.Text, temp), Imie = txtBoxImie.Text, Nazwisko = txtBoxNazwisko.Text, DataStworzeniaKonta = DateTime.UtcNow, Plec = tempPlec, Rola = tempRola, StatusKonta = tempStatus, DataOstatniegoLogowania = DateTime.UtcNow, salt = temp };
                    if (!String.IsNullOrEmpty(txtBoxEmail.Text))
                    {
                        nowy.email = txtBoxEmail.Text;
                    }
                    if (!String.IsNullOrEmpty(txtBoxNrTelefonu.Text))
                    {
                        nowy.nrTelefonu = txtBoxNrTelefonu.Text;
                    }

                    GlowneOkno.BazaDanych.Uzytkownicy.Add(nowy);



                    GlowneOkno.BazaDanych.SaveChanges();
                    MessageBox.Show("Dodano Uzytkownika");
                    GlowneOkno.ZalogowanyUzytkownik = nowy;
                    nowy = null;
                    Logi nowyLog = new Logi() { DataWystapienia = DateTime.UtcNow, TrescWiadomosci = "Stworzono nowego uzytkownika", Uzytkownicy = GlowneOkno.ZalogowanyUzytkownik, Waznosc = 1 };
                    GlowneOkno.ZalogowanyUzytkownik.WszystkieLogi.Add(nowyLog);
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
