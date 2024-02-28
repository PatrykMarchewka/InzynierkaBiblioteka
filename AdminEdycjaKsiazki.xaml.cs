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
    /// Interaction logic for AdminEdycjaKsiazki.xaml
    /// </summary>
    public partial class AdminEdycjaKsiazki : Page
    {
        public static Ksiazki? EdytowanaKsiazka;
        Regex regex = new Regex("^[0-9\\-]*(x)?$");
        Regex regexliczby = new Regex("^[0-9]");
        List<Powiadomienia> powia = new List<Powiadomienia>();

        public AdminEdycjaKsiazki()
        {
            InitializeComponent();
            comboGatunki.ItemsSource = GlowneOkno.BazaDanych.GatunkiKsiazek.OrderBy(g => g.Nazwa).ToHashSet();
            comboJezyk.ItemsSource = GlowneOkno.BazaDanych.Jezyki.OrderBy(j => j.Nazwa).ToHashSet();
            if (EdytowanaKsiazka != null)
            {
                txtBoxISBN.Text = EdytowanaKsiazka.ISBN;
                txtBoxTytul.Text = EdytowanaKsiazka.TytulKsiazki;
                comboGatunki.SelectedItem = EdytowanaKsiazka.GatunekKsiazki;
                txtBoxRok.Text = EdytowanaKsiazka.RokPublikacjiKsiazki.ToString();
                comboJezyk.SelectedItem = EdytowanaKsiazka.JezykKsiazki;
                txtBoxStrony.Text = EdytowanaKsiazka.IloscStron.ToString();
                txtboxKopie.Text = EdytowanaKsiazka.DostepnoscKsiazki.ToString();
                chkBoxWypozyczenie.IsChecked = EdytowanaKsiazka.DoWypozyczenia;

            }
        }

        private void btnPowrot_Click(object sender, RoutedEventArgs e)
        {
            EdytowanaKsiazka = null;
            MainWindow.GlownaRamka.GoBack();
        }

        private void btnZapiszZmiany_Click(object sender, RoutedEventArgs e)
        {
            if (EdytowanaKsiazka == null)
            {
                try
                {
                    EdytowanaKsiazka = new Ksiazki() { ISBN = txtBoxISBN.Text, TytulKsiazki = txtBoxTytul.Text, GatunekKsiazki = (BazaDanych.GatunkiKsiazek)comboGatunki.SelectedItem, RokPublikacjiKsiazki = int.Parse(txtBoxRok.Text), JezykKsiazki = (BazaDanych.Jezyki)comboJezyk.SelectedItem, IloscStron = int.Parse(txtBoxStrony.Text), DostepnoscKsiazki = int.Parse(txtboxKopie.Text), DoWypozyczenia = (bool)chkBoxWypozyczenie.IsChecked };
                    GlowneOkno.BazaDanych.Ksiazki.Add(EdytowanaKsiazka);
                    GlowneOkno.BazaDanych.SaveChanges();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Blad! {ex.Message}");
                }
                
            }
            else
            {
                if (EdytowanaKsiazka.DostepnoscKsiazki == 0)
                {
                    int liczba = int.Parse(txtboxKopie.Text);
                    foreach (var item in GlowneOkno.BazaDanych.Powiadomienia)
                    {
                        if (item.Ksiazka.ISBN == EdytowanaKsiazka.ISBN && item.KiedyWyslanoMail == null && (bool)chkBoxWypozyczenie.IsChecked && liczba > 0)
                        {
                            //Dodawanie do listy poniewaz EF Core nie lubi jak sie otwiera wiele polaczen naraz!
                            powia.Add(item);
                            item.Ksiazka.LiczbaOczekujacych--;
                            liczba--;
                        }
                        else if (liczba <= 0)
                        {
                            break;
                        }
                    }
                }




                foreach (var item in powia)
                {
                    WysylanieMaili.WysylanieWiadomosciEmail(item.Uzytkownicy.email, "Powiadomienie o dostępności książki", $"Otrzymaliśmy dostawę z twoją książką\nKsiążka: {EdytowanaKsiazka.TytulKsiazki} jest teraz dostępna w naszej bibliotece");
                    item.KiedyWyslanoMail = DateTime.UtcNow;
                }
                powia.Clear();

                EdytowanaKsiazka.ISBN = txtBoxISBN.Text;
                EdytowanaKsiazka.TytulKsiazki = txtBoxTytul.Text;
                EdytowanaKsiazka.GatunekKsiazki = (BazaDanych.GatunkiKsiazek)comboGatunki.SelectedItem;
                EdytowanaKsiazka.RokPublikacjiKsiazki = int.Parse(txtBoxRok.Text);
                EdytowanaKsiazka.JezykKsiazki = (BazaDanych.Jezyki)comboJezyk.SelectedItem;
                EdytowanaKsiazka.IloscStron = int.Parse(txtBoxStrony.Text);
                EdytowanaKsiazka.DostepnoscKsiazki = int.Parse(txtboxKopie.Text);
                EdytowanaKsiazka.DoWypozyczenia = (bool)chkBoxWypozyczenie.IsChecked;

                GlowneOkno.BazaDanych.SaveChanges();


            }


        }

        private void txtBoxISBNRegex(object sender, TextCompositionEventArgs e)
        {
            if (sender is TextBox txtBox)
            {
                string tekst = txtBox.Text + e.Text;
                e.Handled = !regex.IsMatch(tekst);
            }
            
        }

        private void txtBoxRegex(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !regexliczby.IsMatch(e.Text);
        }

    }
}
