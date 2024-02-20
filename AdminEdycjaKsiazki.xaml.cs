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
        Regex regex = new Regex("[^0-9]+");

        public AdminEdycjaKsiazki()
        {
            InitializeComponent();
            comboGatunki.Items.Clear();
            comboJezyk.Items.Clear();
            foreach (var item in GlowneOkno.BazaDanych.GatunkiKsiazek)
            {
                comboGatunki.Items.Add(item);
            }
            foreach (var item in GlowneOkno.BazaDanych.Jezyki)
            {
                comboJezyk.Items.Add(item);
            }
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
                EdytowanaKsiazka.ISBN = txtBoxISBN.Text;
                EdytowanaKsiazka.TytulKsiazki = txtBoxTytul.Text;
                EdytowanaKsiazka.GatunekKsiazki = (BazaDanych.GatunkiKsiazek)comboGatunki.SelectedItem;
                EdytowanaKsiazka.RokPublikacjiKsiazki = int.Parse(txtBoxRok.Text);
                EdytowanaKsiazka.JezykKsiazki = (BazaDanych.Jezyki)comboJezyk.SelectedItem;
                EdytowanaKsiazka.IloscStron = int.Parse(txtBoxStrony.Text);
                EdytowanaKsiazka.DostepnoscKsiazki = int.Parse(txtboxKopie.Text);
                EdytowanaKsiazka.DoWypozyczenia = (bool)chkBoxWypozyczenie.IsChecked;
            }
        }

        private void txtBoxRegex(object sender, TextCompositionEventArgs e)
        {
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
