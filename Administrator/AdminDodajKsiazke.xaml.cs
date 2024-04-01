using InżynierkaBiblioteka.BazaDanych;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
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
    /// Interaction logic for AdminDodajKsiazke.xaml
    /// </summary>
    public partial class AdminDodajKsiazke : Page
    {
        public AdminDodajKsiazke()
        {
            InitializeComponent();
        }

        private void btnWyszukajKsiazke_Click(object sender, RoutedEventArgs e)
        {
            HashSet<Ksiazki> Lista = new HashSet<Ksiazki>();
            Lista.Clear();
            Stack.Children.Clear();
            Lista = GlowneOkno.BazaDanych.Ksiazki.Where(b => EF.Functions.Like(b.TytulKsiazki, $"%{txtBoxKsiazka.Text}%") || EF.Functions.Like(b.ISBN, $"%{txtBoxKsiazka.Text}%")).ToHashSet();
            if(Lista.Count == 0)
            {
                MessageBoxResult result = MessageBox.Show("Nieznaleziono ksiazki w bazie danych, czy chcesz utworzyc nowa ksiazke?", "Brak wynikow", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    MainWindow.Nawigacja("Administrator/AdminEdycjaKsiazki.xaml");
                }
            }
            foreach (var item in Lista)
            {
                var KopiaItemu = item;
                Button button = new Button();
                button.Content = $"{item.ISBN} - {item.TytulKsiazki}";
                button.Margin = new Thickness(5);
                button.Click += (s, e) => Button_Click(s, e, KopiaItemu);
                Stack.Children.Add(button);
            }

        }

        private void Button_Click(object s, RoutedEventArgs e, Ksiazki kopiaItemu)
        {
            MainWindow.Nawigacja("Administrator/AdminEdycjaKsiazki.xaml");
            AdminEdycjaKsiazki.EdytowanaKsiazka = kopiaItemu;
        }

        private void btnPowrot_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.GlownaRamka.GoBack();
        }
    }
}
