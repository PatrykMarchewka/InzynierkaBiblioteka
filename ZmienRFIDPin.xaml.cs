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
    /// Interaction logic for ZmienRFIDPin.xaml
    /// </summary>
    public partial class ZmienRFIDPin : Page
    {
        public ZmienRFIDPin()
        {
            InitializeComponent();
            txtBoxStaryPin.Text = String.Empty;
            txtBoxNowyPin.Text = String.Empty;
            txtBoxNowyPin2.Text = String.Empty;
        }

        private void btnPowrot_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.GlownaRamka.GoBack();
        }

        private void btnZapiszZmiany_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (int.Parse(txtBoxStaryPin.Text) == GlowneOkno.ZalogowanyUzytkownik.RFIDPin || GlowneOkno.ZalogowanyUzytkownik.RFIDPin == null)
                {
                    StringComparer stringComparer = StringComparer.OrdinalIgnoreCase;
                    if (stringComparer.Compare(txtBoxNowyPin, txtBoxNowyPin2) == 0)
                    {
                        GlowneOkno.ZalogowanyUzytkownik.RFIDPin = int.Parse(txtBoxNowyPin.Text);
                        GlowneOkno.BazaDanych.SaveChanges();
                        MessageBox.Show("Sukces! Pomyslnie zmieniono dane");
                    }
                    else
                    {
                        MessageBox.Show("Blad! Sprawdz swoje dane");
                        txtBoxStaryPin.Text = String.Empty;
                        txtBoxNowyPin.Text = String.Empty;
                        txtBoxNowyPin2.Text = String.Empty;
                    }


                }
                else
                {
                    MessageBox.Show("Blad! Sprawdz swoje dane");
                    txtBoxStaryPin.Text = String.Empty;
                    txtBoxNowyPin.Text = String.Empty;
                    txtBoxNowyPin2.Text = String.Empty;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Blad! {ex.Message}");
                txtBoxStaryPin.Text = String.Empty;
                txtBoxNowyPin.Text = String.Empty;
                txtBoxNowyPin2.Text = String.Empty;
            }
        }
    }
}
