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
    /// Interaction logic for PokazKsiazke.xaml
    /// </summary>
    public partial class PokazKsiazke : Page
    {
        public static BazaDanych.Ksiazki PokazKsiazkeKsiazka;
        public PokazKsiazke()
        {
            InitializeComponent();
            textBlockISBN.Text = PokazKsiazkeKsiazka.ISBN;
            textBlockTytul.Text = PokazKsiazkeKsiazka.TytulKsiazki;
            textBlockGatunek.Text = PokazKsiazkeKsiazka.GatunekKsiazki.Nazwa;
            textBlockRokPublikacji.Text = PokazKsiazkeKsiazka.RokPublikacjiKsiazki.ToString();
            textBlockJezyk.Text = PokazKsiazkeKsiazka.JezykKsiazki.Nazwa;
            textBlockIloscStron.Text = PokazKsiazkeKsiazka.IloscStron.ToString();
            if (PokazKsiazkeKsiazka.LiczbaOczekujacych > 0)
            {
                textBlockDostepnosc.Text = "Liczba oczekujacych: " + PokazKsiazkeKsiazka.LiczbaOczekujacych.ToString();
            }
            else
            {
                textBlockDostepnosc.Text = "Dostepnych jest " + PokazKsiazkeKsiazka.DostepnoscKsiazki.ToString() + " egzemplarzy";
            }
            if (PokazKsiazkeKsiazka.DoWypozyczenia)
            {
                textBlockCzyDoWypozyczenia.Text = "Mozna wypozyczyc";
            }
            else
            {
                textBlockCzyDoWypozyczenia.Text = "Nie mozna wypozyczyc";
                btnWypozyczKsiazke.Visibility = Visibility.Hidden;
            }
            textBlockIloscWypozyczen30Dni.Text = PokazKsiazkeKsiazka.IloscWypozyczen30Dni.ToString();

            if (GlowneOkno.ZalogowanyUzytkownik.WszystkieWypozyczoneKsiazkiHash().Contains(PokazKsiazkeKsiazka))
            {
                btnNapiszRecenzje.Visibility = Visibility.Visible;
                if (GlowneOkno.ZalogowanyUzytkownik.Recenzje.Any(r => r.Ksiazka == PokazKsiazkeKsiazka))
                {
                    btnNapiszRecenzje.Content = " Edytuj Recenzje ";
                    NapiszRecenzje.IstniejacaRecenzja = GlowneOkno.ZalogowanyUzytkownik.Recenzje.First(r => r.Ksiazka == PokazKsiazkeKsiazka);
                }
                else
                {
                    NapiszRecenzje.IstniejacaRecenzja = null;
                }
            }

            if (!PokazKsiazkeKsiazka.DoWypozyczenia || PokazKsiazkeKsiazka.DostepnoscKsiazki == 0)
            {
                btnWypozyczKsiazke.Visibility = Visibility.Hidden;
                btnStworzPowiadomienie.Visibility = Visibility.Visible;
            }



            //Recenzje
            foreach (var item in GlowneOkno.BazaDanych.Recenzje.Where(r => r.Ksiazka == PokazKsiazkeKsiazka))
            {
                var kopiaItemu = item;
                Button button = new Button();
                //145 znakow maks
                string content = $"{kopiaItemu.DataWystawienia.ToShortDateString()} : {kopiaItemu.Ocena}/5 {kopiaItemu.TekstRecenzji}";
                if (content.Length > 145)
                {
                    button.Content = content.Substring(0, 145) + "...";
                }
                else
                {
                    button.Content = content;
                }
                button.Background = new SolidColorBrush(Colors.Transparent);
                button.Margin = new Thickness(5);
                button.Click += (s,e) => Button_Click(s,e,kopiaItemu);
                Stack.Children.Add(button);
            }




        }

        private void Button_Click(object sender, RoutedEventArgs e, Recenzje r)
        {
            ZobaczRecenzje.recenzja = r;
            MainWindow.Nawigacja("ZobaczRecenzje.xaml");
        }

        private void btnWypozyczKsiazke_Click(object sender, RoutedEventArgs e)
        {
            if (GlowneOkno.ZalogowanyUzytkownik.LiczbaWypozyczonychKsiazek >= 3 || GlowneOkno.ZalogowanyUzytkownik.Zaleglosci != 0)
            {
                MessageBox.Show("Blad! Nie mozna wypozyczyc wiecej ksiazek!");
            }
            else
            {
                Wypozyczenia w = new Wypozyczenia()
                {
                    DataWypozyczenia = DateTime.UtcNow,
                    DataDoOddania = DateTime.UtcNow.AddDays(14),
                    DataAktualnegoOddania = null,
                    Uzytkownik = GlowneOkno.ZalogowanyUzytkownik,
                    Ksiazka = PokazKsiazkeKsiazka
                };
                GlowneOkno.BazaDanych.Wypozyczenia.Add(w);
                GlowneOkno.ZalogowanyUzytkownik.Wypozyczenia.Add(w);
                PokazKsiazkeKsiazka.Wypozyczenia.Add(w);
                GlowneOkno.ZalogowanyUzytkownik.LiczbaWypozyczonychKsiazek++;
                PokazKsiazkeKsiazka.DostepnoscKsiazki--;
                PokazKsiazkeKsiazka.IloscWypozyczen30Dni++;
                GlowneOkno.ZalogowanyUzytkownik.WszystkieWypozyczoneKsiazki.Add(PokazKsiazkeKsiazka);
                GlowneOkno.BazaDanych.SaveChanges();
                MessageBox.Show("Wypozyczono ksiazke!");
            }


            
        }

        private void btnPowrot_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.GlownaRamka.GoBack();
        }

        private void btnNapiszRecenzje_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Nawigacja("NapiszRecenzje.xaml");
        }

        private void btnStworzPowiadomienie_Click(object sender, RoutedEventArgs e)
        {
            if (GlowneOkno.ZalogowanyUzytkownik.Powiadomienia.Any(p => p.Ksiazka == PokazKsiazkeKsiazka))
            {
                MessageBoxResult result = MessageBox.Show("Czy chcesz usunąć swoje powiadomienie o tej książce?", "Powiadomienie", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    Powiadomienia pow = GlowneOkno.ZalogowanyUzytkownik.Powiadomienia.First(pow => pow.Ksiazka == PokazKsiazkeKsiazka);
                    GlowneOkno.ZalogowanyUzytkownik.Powiadomienia.Remove(pow);
                    GlowneOkno.BazaDanych.Powiadomienia.Remove(pow);
                    PokazKsiazkeKsiazka.LiczbaOczekujacych--;
                    GlowneOkno.BazaDanych.SaveChanges();
                }
            }
            else
            {

                if (GlowneOkno.ZalogowanyUzytkownik.email != null)
                {
                    Powiadomienia p = new Powiadomienia()
                    {
                        KiedyStworzono = DateTime.UtcNow,
                        Ksiazka = PokazKsiazkeKsiazka,
                        Uzytkownik = GlowneOkno.ZalogowanyUzytkownik,
                        KiedyWyslanoMail = null
                    };
                    GlowneOkno.BazaDanych.Powiadomienia.Add(p);
                    GlowneOkno.ZalogowanyUzytkownik.Powiadomienia.Add(p);
                    PokazKsiazkeKsiazka.LiczbaOczekujacych++;
                    GlowneOkno.BazaDanych.SaveChanges();
                }
                else
                {
                    MessageBox.Show("Blad! Aby otrzymac powiadomienie musisz miec ustawiony adres email");
                }

                
            }


            
        }
    }
}
