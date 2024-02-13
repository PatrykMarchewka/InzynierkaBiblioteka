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

        public static Uzytkownik? proba;

        public OdzyskajKonto()
        {
            InitializeComponent();
            this.Loaded += OdzyskajKonto_Loaded;
            txtBoxEmail.Text = String.Empty;
            txtBoxLogin.Text = String.Empty;
        }

        private void OdzyskajKonto_Loaded(object sender, RoutedEventArgs e)
        {
            proba = null;
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
                proba = null;
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
                    MainWindow.Nawigacja("ZmienHaslo.xaml");
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
