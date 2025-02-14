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

namespace InżynierkaBiblioteka.Administrator
{
    /// <summary>
    /// Interaction logic for DodajZaleglosci.xaml
    /// </summary>
    public partial class DodajZaleglosci : Page
    {
        private static Ksiazki? WybranaKsiazka;
        public static Zaleglosci? Zaleglosc;
        Regex regexliczby = new Regex(@"^\d+(,\d{0,2})?$");
        public DodajZaleglosci()
        {
            InitializeComponent();
            //comboKsiazki.ItemsSource = GlowneOkno.BazaDanych.Ksiazki.OrderBy(k => k.TytulKsiazki).ToHashSet();
            var nullItem = new ComboBoxItem() { Content = "(BRAK)", Tag = null };
            comboKsiazki.Items.Insert(0, nullItem);
            foreach (var item in GlowneOkno.BazaDanych.Ksiazki.OrderBy(k => k.TytulKsiazki).ToHashSet())
            {
                comboKsiazki.Items.Add(item);
            }
            comboKsiazki.SelectedIndex = 0;

            if (Zaleglosc != null)
            {
                if (Zaleglosc.Ksiazka != null)
                {
                    WybranaKsiazka = Zaleglosc.Ksiazka;
                    comboKsiazki.SelectedItem = WybranaKsiazka;
                }
                else
                {
                    comboKsiazki.SelectedIndex = 0;
                }
                txtBoxZaplata.Text = Zaleglosc.Zaleglosc.ToString();
                chkBoxZaplacono.IsChecked = Zaleglosc.Zaplacono;
                txtBoxKomentarz.Text = Zaleglosc.Komentarz;
            }
        }

        private void btnPowrot_Click(object sender, RoutedEventArgs e)
        {
            WybranaKsiazka = null;
            Zaleglosc = null;
            MainWindow.GlownaRamka.GoBack();
        }

        private void comboKsiazki_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                WybranaKsiazka = (Ksiazki)comboKsiazki.SelectedItem;
            }
            catch (Exception)
            {
                WybranaKsiazka = null;
            }

        }

        private void txtBoxZaplata_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (!regexliczby.IsMatch(txtBoxZaplata.Text))
            {
                if (txtBoxZaplata.Text.Length > 0)
                {
                    txtBoxZaplata.Text = txtBoxZaplata.Text.Substring(0, txtBoxZaplata.Text.Length - 1);
                    txtBoxZaplata.CaretIndex = txtBoxZaplata.Text.Length;

                }

            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (txtBoxZaplata.Text.Length < 1 || txtBoxZaplata.Text == "." || !Decimal.TryParse(txtBoxZaplata.Text, out Decimal result))
            {
                MessageBox.Show("Blad! Podaj poprawna kwote do zaplaty");
            }
            else if (txtBoxKomentarz.Text.Length < 1 || txtBoxKomentarz.Text.Length > 255)
            {
                MessageBox.Show("Blad! Komentarz za krotki lub za dlugi");
            }
            else
            {
                if (Zaleglosc != null)
                {
                    string tresc = $"Administrator {GlowneOkno.ZalogowanyAdministrator.idUzytkownika} zmienil zaleglosc, poprzednie dane - Ksiazka:{Zaleglosc.Ksiazka.ISBN} Koszt:{Zaleglosc.Zaleglosc} Zaplacony?:{Zaleglosc.Zaplacono} Komentarz:{Zaleglosc.Komentarz}";
                    tresc = tresc.Length > 255 ? tresc.Substring(0, 255) : tresc;
                    Logi nowyLog = new Logi() { DataWystapienia = DateTime.UtcNow, Uzytkownicy = GlowneOkno.ZalogowanyUzytkownik, Waznosc = 1, TrescWiadomosci = tresc };
                    GlowneOkno.BazaDanych.Logi.Add(nowyLog);
                    Zaleglosc.Ksiazka = WybranaKsiazka;
                    Zaleglosc.Zaleglosc = Decimal.Parse(txtBoxZaplata.Text);
                    Zaleglosc.Zaplacono = (bool)chkBoxZaplacono.IsChecked;
                    Zaleglosc.Komentarz = txtBoxKomentarz.Text;
                }
                else
                {
                    Zaleglosc = new Zaleglosci() { Ksiazka = WybranaKsiazka, Uzytkownicy = GlowneOkno.ZalogowanyUzytkownik, Zaleglosc = Decimal.Parse(txtBoxZaplata.Text), Zaplacono = (bool)chkBoxZaplacono.IsChecked, Komentarz = txtBoxKomentarz.Text };
                    GlowneOkno.ZalogowanyUzytkownik.WszystkieZaleglosci.Add(Zaleglosc);
                    string tresc = $"Administrator {GlowneOkno.ZalogowanyAdministrator.idUzytkownika} dodal nowa zaleglosc z komentarzem {txtBoxKomentarz.Text}";
                    tresc = tresc.Length > 255 ? tresc.Substring(0, 255) : tresc;
                    Logi nowyLog = new Logi() { DataWystapienia = DateTime.UtcNow, Uzytkownicy = GlowneOkno.ZalogowanyUzytkownik, Waznosc = 1, TrescWiadomosci = tresc };
                    GlowneOkno.BazaDanych.Logi.Add(nowyLog);
                }
                GlowneOkno.BazaDanych.SaveChanges();
                MainWindow.GlownaRamka.GoBack();
            }
        }
    }
}
