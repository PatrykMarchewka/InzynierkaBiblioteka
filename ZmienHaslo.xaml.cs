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
    /// Interaction logic for ZmienHaslo.xaml
    /// </summary>
    public partial class ZmienHaslo : Page
    {
        public ZmienHaslo()
        {
            InitializeComponent();
            txtBoxStareHaslo.Text = String.Empty;
            txtBoxNoweHaslo1.Text = String.Empty;
            txtBoxNoweHaslo2.Text = String.Empty;
        }

        private void btnPowrot_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.GlownaRamka.GoBack();
        }

        private void btnZapiszZmiany_Click(object sender, RoutedEventArgs e)
        {
            StringComparer stringComparer = StringComparer.OrdinalIgnoreCase;


            if (OdzyskajKonto.proba != null)
            {
                OdzyskajKonto.proba.hashHaslo = StworzNoweKonto.StworzHash(txtBoxNoweHaslo1.Text);
                GlowneOkno.ZalogowanyUzytkownik = OdzyskajKonto.proba;
                ZmienDane();
            }
            
            else if (stringComparer.Compare(StworzNoweKonto.StworzHash(txtBoxStareHaslo.Text),GlowneOkno.ZalogowanyUzytkownik.hashHaslo) == 0)
            {
                if (stringComparer.Compare(txtBoxNoweHaslo1.Text,txtBoxNoweHaslo2.Text) == 0)
                {
                    GlowneOkno.ZalogowanyUzytkownik.hashHaslo = StworzNoweKonto.StworzHash(txtBoxNoweHaslo1.Text);
                    ZmienDane();
                }
                else
                {
                    MessageBox.Show("Blad! Sprawdz swoje dane");
                    txtBoxStareHaslo.Text = String.Empty;
                    txtBoxNoweHaslo1.Text = String.Empty;
                    txtBoxNoweHaslo2.Text = String.Empty;
                }
            }
            else
            {
                MessageBox.Show("Blad! Sprawdz swoje dane");
                txtBoxStareHaslo.Text = String.Empty;
                txtBoxNoweHaslo1.Text = String.Empty;
                txtBoxNoweHaslo2.Text = String.Empty;
            }
        }

        private void ZmienDane()
        {
            GlowneOkno.BazaDanych.SaveChanges();
            MessageBox.Show("Sukces! Pomyslnie zmieniono dane");
            WysylanieMaili.LogowanieDoMaila();
            WysylanieMaili.WysylanieWiadomosciEmail(OdzyskajKonto.proba.email, "Prośba o odzyskanie konta", $"Otrzymaliśmy prośbę o odzyskanie konta\nTwoje nowe hasło to: {txtBoxNoweHaslo1.Text}\nJeśli to nie ty wysłałeś prośbę to zmień hasło jak najszybciej!");
            OdzyskajKonto.proba = null;
            MainWindow.Nawigacja("PoZalogowaniuUzytkownik.xaml");
        }
    }
}
