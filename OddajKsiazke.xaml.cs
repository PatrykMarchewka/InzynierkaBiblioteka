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
    /// Interaction logic for OddajKsiazke.xaml
    /// </summary>
    public partial class OddajKsiazke : Page
    {
        public OddajKsiazke()
        {
            InitializeComponent();
            foreach (var item in GlowneOkno.ZalogowanyUzytkownik.Wypozyczenia)
            {
                grid.RowDefinitions.Add(new RowDefinition() { Height= GridLength.Auto});
                Button button = new Button() { Content= item.Ksiazka.TytulKsiazki };
                button.Click += (s, e) => btnButton_Click(s, e, item);
                Grid.SetRow(button, grid.RowDefinitions.Count - 1);
                grid.Children.Add(button);
            }
        }

        private void btnPowrot_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.GlownaRamka.GoBack();
        }

        private void btnButton_Click(object sender, RoutedEventArgs e, Wypozyczenia w)
        {
            
            //Przetestowac to bo watpie by dzialalo
            
                w.DataAktualnegoOddania = DateTime.UtcNow;
                GlowneOkno.ZalogowanyUzytkownik.LiczbaWypozyczonychKsiazek--;
                MessageBox.Show($"Oddano ksiazke! {w.Ksiazka.TytulKsiazki}");
                w.Ksiazka.DostepnoscKsiazki++;
                GlowneOkno.BazaDanych.SaveChanges();


        }
    }
}
