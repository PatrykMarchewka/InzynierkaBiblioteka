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
        }

        private void btnPowrot_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.GlownaRamka.GoBack();
        }

        private void btnZapiszZmiany_Click(object sender, RoutedEventArgs e)
        {
            StringComparer stringComparer = StringComparer.OrdinalIgnoreCase;

            if (stringComparer.Compare(StworzNoweKonto.StworzHash(txtBoxStareHaslo.Text),GlowneOkno.ZalogowanyUzytkownik.hashHaslo) == 0)
            {
                if (stringComparer.Compare(txtBoxNoweHaslo1.Text,txtBoxNoweHaslo2.Text) == 0)
                {
                    GlowneOkno.ZalogowanyUzytkownik.hashHaslo = StworzNoweKonto.StworzHash(txtBoxNoweHaslo1.Text);
                    GlowneOkno.BazaDanych.SaveChanges();
                }
                else
                {
                    MessageBox.Show("Blad! Sprawdz swoje dane");
                }
            }
            else
            {
                MessageBox.Show("Blad! Sprawdz swoje dane");
            }
        }
    }
}
