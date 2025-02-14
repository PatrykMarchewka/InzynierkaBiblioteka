using InżynierkaBiblioteka.BazaDanych;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
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
        public static OpcjePolaczeniaOkno opcjePolaczeniaOkno = new OpcjePolaczeniaOkno();
        


        public MainWindow()
        {
            InitializeComponent();
            GlownaRamka = ((MainWindow)Application.Current.MainWindow).MainFrame;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            if (File.Exists(Directory.GetCurrentDirectory() + @"\Connection.json"))
            {
                string tekst = File.ReadAllText(Directory.GetCurrentDirectory() + @"\Connection.json");
                JSONStructure JSON = JsonSerializer.Deserialize<JSONStructure>(tekst);
                MyDbContext.ConnectionString = $"Server={JSON.Serwer};Database={JSON.NazwaBazy};User Id={JSON.Login};Password={JSON.Haslo};Encrypt=False;";
                tekst = null;
                JSON = null;
                Nawigacja("GlowneOkno.xaml");
            }
            else
            {
                opcjePolaczeniaOkno.Show();
            }


            
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception)e.ExceptionObject;
            Logi nowyLog = new Logi() { DataWystapienia = DateTime.UtcNow, TrescWiadomosci = $"Typ bledu: {ex.GetType().Name}, wiadomosc: {ex.Message} - {ex.InnerException}, Stack: {ex.StackTrace}", Waznosc = 100, Uzytkownicy = GlowneOkno.BazaDanych.Uzytkownicy.First() };
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
