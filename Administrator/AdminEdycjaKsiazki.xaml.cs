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


            if (EdytowanaKsiazka == null)
            {
                btnPrzypiszAutorow.Visibility = Visibility.Hidden;
            }



        }

        private void btnPowrot_Click(object sender, RoutedEventArgs e)
        {
            EdytowanaKsiazka = null;
            MainWindow.GlownaRamka.GoBack();
        }

        private void btnZapiszZmiany_Click(object sender, RoutedEventArgs e)
        {
            if (txtBoxISBN.Text.Length > 13 || txtBoxTytul.Text.Length > 255 || !int.TryParse(txtBoxRok.Text, out int result) || !int.TryParse(txtBoxStrony.Text, out int result2) || !int.TryParse(txtboxKopie.Text, out int result3))
            {
                MessageBox.Show("Blad! Jedno lub wiecej pol jest za dlugie, sprawdz wszystkie pola");
            }
            else
            {
                if (EdytowanaKsiazka == null)
                {
                    try
                    {
                        EdytowanaKsiazka = new Ksiazki() { ISBN = txtBoxISBN.Text, TytulKsiazki = txtBoxTytul.Text, GatunekKsiazki = (BazaDanych.GatunkiKsiazek)comboGatunki.SelectedItem, RokPublikacjiKsiazki = int.Parse(txtBoxRok.Text), JezykKsiazki = (BazaDanych.Jezyki)comboJezyk.SelectedItem, IloscStron = int.Parse(txtBoxStrony.Text), DostepnoscKsiazki = int.Parse(txtboxKopie.Text), DoWypozyczenia = (bool)chkBoxWypozyczenie.IsChecked };
                        GlowneOkno.BazaDanych.Ksiazki.Add(EdytowanaKsiazka);
                        string tresc = $"Administrator {GlowneOkno.ZalogowanyAdministrator.idUzytkownika} dodal nowa ksiazke {EdytowanaKsiazka.TytulKsiazki}";
                        tresc = tresc.Length > 255 ? tresc.Substring(0, 255) : tresc;
                        Logi nowyLog = new Logi() { DataWystapienia = DateTime.UtcNow, Uzytkownicy = GlowneOkno.ZalogowanyAdministrator, Waznosc = 1, TrescWiadomosci = tresc };
                        GlowneOkno.BazaDanych.Logi.Add(nowyLog);

                    }
                    catch (Exception ex)
                    {
                        Logi nowyLog = new Logi() { DataWystapienia = DateTime.UtcNow, Uzytkownicy = GlowneOkno.ZalogowanyAdministrator, Waznosc = 10, TrescWiadomosci = $"Blad dodawania ksiazki {ex.Message} - {ex.InnerException}" };
                        MessageBox.Show($"Blad! {ex.Message}");
                        GlowneOkno.BazaDanych.Logi.Add(nowyLog);
                    }
                    GlowneOkno.BazaDanych.SaveChanges();

                }
                else
                {
                    if (EdytowanaKsiazka.DostepnoscKsiazki == 0)
                    {
                        string ltresc = $"Ksiazka {EdytowanaKsiazka.TytulKsiazki} jest teraz ponownie dostepna";
                        ltresc = ltresc.Length > 255 ? ltresc.Substring(0, 255) : ltresc;


                        Logi l = new Logi() { DataWystapienia = DateTime.UtcNow, TrescWiadomosci = ltresc, Uzytkownicy = GlowneOkno.ZalogowanyAdministrator, Waznosc = 1 };
                        GlowneOkno.BazaDanych.Logi.Add(l);
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

                    string tresc = $"Administrator {GlowneOkno.ZalogowanyAdministrator.idUzytkownika} zmienil dane ksiazki {EdytowanaKsiazka.ISBN} - {EdytowanaKsiazka.TytulKsiazki}";
                    tresc = tresc.Length > 255 ? tresc.Substring(0, 255) : tresc;
                    Logi nowyLog = new Logi() { DataWystapienia = DateTime.UtcNow, TrescWiadomosci = tresc, Uzytkownicy = GlowneOkno.ZalogowanyAdministrator, Waznosc = 1 };
                    GlowneOkno.BazaDanych.Logi.Add(nowyLog);

                    GlowneOkno.BazaDanych.SaveChanges();


                }
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

        private void btnPrzypiszAutorow_Click(object sender, RoutedEventArgs e)
        {
            PowiazAutorzyKsiazki.WybranaKsiazka = EdytowanaKsiazka;
            MainWindow.Nawigacja("Administrator/PowiazAutorzyKsiazki.xaml");
        }
    }
}
