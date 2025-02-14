using InżynierkaBiblioteka.BazaDanych;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for ZmienDaneOsobowe.xaml
    /// </summary>
    public partial class ZmienDaneOsobowe : Page
    {


        private static readonly Regex regex = new Regex(@"^(?:\+|\+\d{1,18}|\d{1,20})$", RegexOptions.Compiled);

        public ZmienDaneOsobowe()
        {
            InitializeComponent();
            txtBoxImie.Text = GlowneOkno.ZalogowanyUzytkownik.Imie;
            txtBoxNazwisko.Text = GlowneOkno.ZalogowanyUzytkownik.Nazwisko;
            txtBoxEmail.Text = GlowneOkno.ZalogowanyUzytkownik.email;
            txtBoxTelefon.Text = GlowneOkno.ZalogowanyUzytkownik.nrTelefonu;
            lblZnakA.Visibility = Visibility.Hidden;

            if (GlowneOkno.ZalogowanyAdministrator != null)
            {
                lblKomentarze.Content += GlowneOkno.ZalogowanyUzytkownik.Komentarze;
            }
            else
            {
                lblKomentarze.Visibility = Visibility.Hidden;
            }


        }

        private void btnZapiszZmiany_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txtBoxImie.Text) || String.IsNullOrWhiteSpace(txtBoxNazwisko.Text) || lblZnakA.Visibility == Visibility.Visible)
            {
                MessageBox.Show("Blad! Wypelnij wszystkie pola");
            }
            else if (txtBoxImie.Text.Length > 255 || txtBoxNazwisko.Text.Length > 255 || txtBoxEmail.Text.Length > 255 || txtBoxTelefon.Text.Length > 20)
            {
                MessageBox.Show("Blad! Jedno lub wiecej pol jest za dlugie, sprawdz wszystkie pola");
            }
            else
            {
                try
                {
                    WysylanieMaili.LogowanieDoMaila();
                    WysylanieMaili.WysylanieWiadomosciEmail(GlowneOkno.ZalogowanyUzytkownik.email, "Zmiana danych osobowych", $"Otrzymaliśmy prośbę o zmianę twoich danych osobowych\nJeśli to nie ty wysłałeś prośbę to zmień hasło jak najszybciej!");
                }
                catch (Exception ex)
                {
                    Logi nowyLog = new Logi() { DataWystapienia = DateTime.UtcNow, TrescWiadomosci = $"Nie udalo sie wyslac wiadomosci Email {ex.Message} - {ex.InnerException}", Uzytkownicy = GlowneOkno.ZalogowanyUzytkownik, Waznosc = 10 };
                    GlowneOkno.BazaDanych.Logi.Add(nowyLog);
                }
                GlowneOkno.ZalogowanyUzytkownik.Imie = txtBoxImie.Text;
                GlowneOkno.ZalogowanyUzytkownik.Nazwisko = txtBoxNazwisko.Text;
                if (String.IsNullOrEmpty(txtBoxEmail.Text))
                {
                    GlowneOkno.ZalogowanyUzytkownik.email = null;
                }
                else
                {
                    GlowneOkno.ZalogowanyUzytkownik.email = txtBoxEmail.Text;
                }
                if (String.IsNullOrEmpty(txtBoxTelefon.Text))
                {
                    GlowneOkno.ZalogowanyUzytkownik.nrTelefonu = null;
                }
                else
                {
                    GlowneOkno.ZalogowanyUzytkownik.nrTelefonu = txtBoxTelefon.Text;
                }
                Logi l = new Logi() { DataWystapienia = DateTime.UtcNow, TrescWiadomosci = $"Zmieniono dane uzytkownika", Uzytkownicy = GlowneOkno.ZalogowanyUzytkownik, Waznosc = 1 };
                GlowneOkno.BazaDanych.Logi.Add(l);
                GlowneOkno.BazaDanych.SaveChanges();
                MessageBox.Show("Sukces! Pomyslnie zmieniono dane");

            }
        }

        private void btnPowrot_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.GlownaRamka.GoBack();
        }

        private void txtBoxEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtBoxEmail.Text.Contains('@'))
            {
                lblZnakA.Visibility = Visibility.Hidden;
                btnZapiszZmiany.Visibility = Visibility.Visible;
            }
            else if (String.IsNullOrEmpty(txtBoxEmail.Text))
            {
                lblZnakA.Visibility = Visibility.Hidden;
                btnZapiszZmiany.Visibility = Visibility.Visible;
            }
            else
            {
                lblZnakA.Visibility = Visibility.Visible;
                btnZapiszZmiany.Visibility = Visibility.Hidden;
            }
        }

        private void txtBoxTelefon_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            string nowy = textBox.Text.Insert(textBox.SelectionStart, e.Text);

            e.Handled = !regex.IsMatch(nowy);
        }
    }
}
