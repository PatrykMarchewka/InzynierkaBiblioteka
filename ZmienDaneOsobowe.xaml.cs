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
    /// Interaction logic for ZmienDaneOsobowe.xaml
    /// </summary>
    public partial class ZmienDaneOsobowe : Page
    {
        public ZmienDaneOsobowe()
        {
            InitializeComponent();
            txtBoxImie.Text = GlowneOkno.ZalogowanyUzytkownik.Imie;
            txtBoxNazwisko.Text = GlowneOkno.ZalogowanyUzytkownik.Nazwisko;
            txtBoxEmail.Text = GlowneOkno.ZalogowanyUzytkownik.email;
            txtBoxTelefon.Text = GlowneOkno.ZalogowanyUzytkownik.nrTelefonu;

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
            if (String.IsNullOrWhiteSpace(txtBoxImie.Text) || String.IsNullOrWhiteSpace(txtBoxNazwisko.Text))
            {
                MessageBox.Show("Blad! Wypelnij wszystkie pola");
            }
            else
            {
                GlowneOkno.ZalogowanyUzytkownik.Imie = txtBoxImie.Text;
                GlowneOkno.ZalogowanyUzytkownik.Nazwisko = txtBoxNazwisko.Text;
                GlowneOkno.ZalogowanyUzytkownik.email = txtBoxEmail.Text;
                GlowneOkno.ZalogowanyUzytkownik.nrTelefonu = txtBoxTelefon.Text;
                GlowneOkno.BazaDanych.SaveChanges();
                MessageBox.Show("Sukces! Pomyslnie zmieniono dane");
            }
        }

        private void btnPowrot_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.GlownaRamka.GoBack();
        }
    }
}
