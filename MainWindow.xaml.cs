using InżynierkaBiblioteka.BazaDanych;
using Microsoft.Extensions.Logging;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public static Frame GlownaRamka;
        public static string UserInput; //Po co to? Bo zapomnialem
        


        public MainWindow()
        {
            InitializeComponent();
            GlownaRamka = ((MainWindow)Application.Current.MainWindow).MainFrame;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Nawigacja("GlowneOkno.xaml");
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception)e.ExceptionObject;
            Logi nowyLog = new Logi() { DataWystapienia = DateTime.UtcNow, TrescWiadomosci = $"Typ bledu: {ex.GetType().Name}, wiadomosc: {ex.Message} - {ex.InnerException}, Stack: {ex.StackTrace}", Waznosc = 100 };
            if (GlowneOkno.ZalogowanyAdministrator != null)
            {
                nowyLog.Uzytkownicy = GlowneOkno.ZalogowanyAdministrator;
                GlowneOkno.ZalogowanyAdministrator.WszystkieLogi.Add(nowyLog);
            }
            else if (GlowneOkno.ZalogowanyUzytkownik != null)
            {
                nowyLog.Uzytkownicy = GlowneOkno.ZalogowanyUzytkownik;
                GlowneOkno.ZalogowanyUzytkownik.WszystkieLogi.Add(nowyLog);
            }
            GlowneOkno.BazaDanych.Logi.Add(nowyLog);
            GlowneOkno.BazaDanych.SaveChanges();
        }

        public static void Nawigacja(string UriMiejsceDocelowe)
        {
            GlownaRamka.Navigate(new Uri(UriMiejsceDocelowe, UriKind.Relative));
        }

        //public static void Nawigacja(string UriMiejsceDocelowe, object DodatkoweDane)
        //{
        //    GlownaRamka.Navigate(new Uri(UriMiejsceDocelowe, UriKind.Relative), DodatkoweDane);
        //}

        private void btnZamknij_Click(object sender, RoutedEventArgs e)
        {
            GlowneOkno.BazaDanych.Dispose();
            Environment.Exit(0);
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}
