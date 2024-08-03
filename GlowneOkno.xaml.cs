using InżynierkaBiblioteka.BazaDanych;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for GlowneOkno.xaml
    /// </summary>
    public partial class GlowneOkno : Page
    {
        public static Uzytkownicy? ZalogowanyUzytkownik;
        public static Uzytkownicy? ZalogowanyAdministrator;
        public static MyDbContext BazaDanych = new MyDbContext();
        private Page _orginalnaStrona;


        public GlowneOkno()
        {
            InitializeComponent();
            
            
            ZalogowanyUzytkownik = null;
            ZalogowanyAdministrator = null;
            try
            {
                BazaDanych.Database.EnsureCreated();
            }
            catch (Exception ex)
            {
                //TODO: Pozniej dodac osobne okno do powiadomien zamiast MessageBox
                if (MessageBox.Show("Nie mozna ustanowic polaczenia z baza danych, czy chcesz edytowac plik Connection.json?","Blad polaczenia!",MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    MainWindow.Nawigacja("PustaStrona.xaml");
                    MainWindow.opcjePolaczeniaOkno.Show();
                }
                else
                {
                    Environment.Exit(0);
                }
            }
            MyDbContext.ConnectionString = null;
            GC.Collect();
        }

        private void btnZalogujHaslem_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Nawigacja("LogowanieHaslem.xaml");
        }

        private void btnZalogujRFID_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Nawigacja("LogowanieRFID.xaml");
        }

        private void btnStworzNoweKonto_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Nawigacja("StworzNoweKonto.xaml");
        }
    }
}
