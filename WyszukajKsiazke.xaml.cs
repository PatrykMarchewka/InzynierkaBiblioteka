using InżynierkaBiblioteka.BazaDanych;
using Microsoft.EntityFrameworkCore;
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
    /// Interaction logic for WyszukajKsiazke.xaml
    /// </summary>
    public partial class WyszukajKsiazke : Page
    {
        HashSet<Ksiazki> Lista;

        public WyszukajKsiazke()
        {
            InitializeComponent();
            if (GlowneOkno.BazaDanych.Ksiazki.Count() == 0)
            {
                MessageBox.Show("Aktualnie brak ksiazek, wroc pozniej!");
                MainWindow.GlownaRamka.GoBack();
            }
            else
            {
                comboGatunki.BeginInit();
                comboJezyki.BeginInit();
                comboGatunki.Items.Clear();
                comboJezyki.Items.Clear();
                comboGatunki.Items.Add("Brak zaznaczenia");
                comboJezyki.Items.Add("Brak zaznaczenia");
                foreach (var item in GlowneOkno.BazaDanych.GatunkiKsiazek.OrderBy(g => g.Nazwa).ToHashSet())
                {
                    comboGatunki.Items.Add(item.Nazwa);
                }


                foreach (var item in GlowneOkno.BazaDanych.Jezyki)
                {
                    comboJezyki.Items.Add(item.Nazwa);
                }
                comboGatunki.EndInit();
                comboJezyki.EndInit();

                SliderRok.Minimum = GlowneOkno.BazaDanych.Ksiazki.Min(k => k.RokPublikacjiKsiazki);
                SliderRok.Maximum = DateTime.Now.Year;
                SliderRok.Value = SliderRok.Maximum;
                SliderStrony.Maximum = GlowneOkno.BazaDanych.Ksiazki.Max(k => k.IloscStron);
                SliderStrony.Value = SliderStrony.Maximum;
                lblRok.Content = SliderRok.Value;
                lblStrony.Content = SliderStrony.Value;
                Wyszukaj();
            }
            
        }

        private void Wyszukaj()
        {
            Stack.Children.Clear();
            Stack.BeginInit();
            //Wyszukiwanie
            //Dostepnosc, Do wypozyczenia
            Lista = GlowneOkno.BazaDanych.Ksiazki.Where(k => EF.Functions.Like(k.TytulKsiazki, $"%{txtBoxWyszukaj.Text}%") || EF.Functions.Like(k.ISBN, $"%{txtBoxWyszukaj.Text}%")).ToHashSet();
            //Dodawanie po autorze
            //Lista.UnionWith(GlowneOkno.BazaDanych.Ksiazki.Where(k => k.Hashe == GlowneOkno.BazaDanych.HashKsiazkiAutorzy.Where(h => EF.Functions.Like(h.Autor.ImieAutora, $"%{txtBoxWyszukaj.Text}%") || EF.Functions.Like(h.Autor.NazwiskoAutora, $"%{txtBoxWyszukaj.Text}%"))).ToHashSet());
            Lista.UnionWith(GlowneOkno.BazaDanych.Ksiazki.Where(k => GlowneOkno.BazaDanych.HashKsiazkiAutorzy.Any(h => h.Ksiazka == k && (EF.Functions.Like(h.Autor.ImieAutora, $"%{txtBoxWyszukaj.Text}%") || EF.Functions.Like(h.Autor.NazwiskoAutora, $"%{txtBoxWyszukaj.Text}%")))).ToHashSet());
            
            

            if (comboGatunki.SelectedIndex > 0)
            {
                Lista = Lista.Where(b => b.GatunekKsiazki.Nazwa == comboGatunki.SelectedItem.ToString()).ToHashSet();
            }
            if (comboJezyki.SelectedIndex > 0)
            {
                Lista = Lista.Where(b => b.JezykKsiazki.Nazwa == comboJezyki.SelectedItem.ToString()).ToHashSet();
            }
            Lista = Lista.Where(b => b.RokPublikacjiKsiazki <= SliderRok.Value).ToHashSet();
            Lista = Lista.Where(b => b.IloscStron <= SliderStrony.Value).ToHashSet();

            if (CheckBoxDostepne.IsChecked == true)
            {
                Lista = Lista.Where(b => b.DostepnoscKsiazki > 0).ToHashSet();
            }

            if (CheckBoxDoWypozyczenia.IsChecked == true)
            {
                Lista = Lista.Where(b => b.DoWypozyczenia == true).ToHashSet();
            }

            Lista = Lista.OrderBy(b => b.TytulKsiazki).ToHashSet();


            foreach (var item in Lista)
            {

                var KopiaItemu = item;
                Button button = new Button();
                if (item.DoWypozyczenia == false)
                {
                    button.Content = $"{item.TytulKsiazki} - Tylko w bibliotece!";
                }
                else if (item.DostepnoscKsiazki == 0)
                {
                    button.Content = $"{item.TytulKsiazki} - Liczba oczekujacych: {item.LiczbaOczekujacych}";
                }
                else
                {
                    button.Content = $"{item.TytulKsiazki} - Dostepnych: {item.DostepnoscKsiazki}";
                }
                button.Margin = new Thickness(5);

                button.Click += (s, e) => Button_Click(s, e, KopiaItemu);
                Stack.Children.Add(button);
            }
            Stack.EndInit();
        }

        private void txtBoxWyszukaj_TextChanged(object sender, TextChangedEventArgs e)
        {
            Wyszukaj();
        }

        private void Button_Click(object sender, RoutedEventArgs e, Ksiazki ksiazka)
        {
            MainWindow.Nawigacja("PokazKsiazke.xaml");
            PokazKsiazke.PokazKsiazkeKsiazka = ksiazka;
        }

        private void comboGatunki_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Wyszukaj();
            
        }

        private void comboJezyki_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Wyszukaj();
            
        }

        private void SliderRok_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (lblRok != null && e.OldValue != 0)
            {
                lblRok.Content = SliderRok.Value;
                Wyszukaj();
            }
            
        }

        private void SliderStrony_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (lblStrony != null && e.OldValue != 0)
            {
                lblStrony.Content = SliderStrony.Value;
                Wyszukaj();
            }
            
        }

        private void CheckBoxDostepne_Checked(object sender, RoutedEventArgs e)
        {
            if (CheckBoxDostepne != null && CheckBoxDoWypozyczenia != null)
            {
                Wyszukaj();
            }
            
        }

        private void CheckBoxDoWypozyczenia_Checked(object sender, RoutedEventArgs e)
        {
            if (CheckBoxDostepne != null && CheckBoxDoWypozyczenia != null)
            {
                Wyszukaj();
            }
        }

        private void btnPowrot_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.GlownaRamka.GoBack();
        }
    }
}
