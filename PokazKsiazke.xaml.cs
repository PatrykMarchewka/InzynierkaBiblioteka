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
        }

        private void btnWypozyczKsiazke_Click(object sender, RoutedEventArgs e)
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
            GlowneOkno.BazaDanych.SaveChanges();
            MessageBox.Show("Udano!");
        }

        private void btnPowrot_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.GlownaRamka.GoBack();
        }
    }
}
