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
            Nawigacja("GlowneOkno.xaml");
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
