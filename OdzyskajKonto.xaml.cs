using InżynierkaBiblioteka.BazaDanych;
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
    /// Interaction logic for OdzyskajKonto.xaml
    /// </summary>
    public partial class OdzyskajKonto : Page
    {
        public OdzyskajKonto()
        {
            InitializeComponent();
            this.Loaded += OdzyskajKonto_Loaded;
            txtBoxEmail.Text = String.Empty;
            txtBoxLogin.Text = String.Empty;
        }

        private void OdzyskajKonto_Loaded(object sender, RoutedEventArgs e)
        {
            txtBoxEmail.Text = String.Empty;
            txtBoxLogin.Text = String.Empty;
        }

        private void btnPowrot_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.GlownaRamka.GoBack();
        }

        private void btnOdzyskajKonto_Click(object sender, RoutedEventArgs e)
        {
            if (txtBoxEmail.Text.Contains('@'))
            {
                Uzytkownik? proba = null;
                try
                {
                    proba = GlowneOkno.BazaDanych.Uzytkownik.First(u => u.LoginUzytkownika == txtBoxLogin.Text);
                }
                catch (Exception)
                {
                    MessageBox.Show("Nie znaleziono takiego uzytkownika, sprawdz swoj login");
                }

                if (proba != null && proba.email == txtBoxEmail.Text)
                {
                    //TODO: Dokonczyc Wyslanie emaila, ulepszyc, moze przekazac stringi gdzies
                    WysylanieMaili.LogowanieDoMaila();
                    WysylanieMaili.WysylanieWiadomosciEmail("patryk4610@gmail.com", "Prosba o odzyskanie konta", "Przykladowa wiadomosc"); //TODO: Zrobic konto outlook, na prywatnym dziala
                    //Dac txtBoxEmail.text zamiast mojego
                }
            }
            else
            {
                MessageBox.Show("Blad! Email musi zawierac znak @");
            }


           
        }

        private void txtBoxEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtBoxEmail.Text.Contains('@'))
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
