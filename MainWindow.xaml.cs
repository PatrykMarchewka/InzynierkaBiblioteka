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

        public static string DzisiejszaData()
        {
            //Dzisiejsza data zapisywana w formacie UTC aby byla taka sama nawet gdyby komputer byl w innej strefie czasowej
            return DateTime.UtcNow.ToString("dd-MM-yyyy HH-mm-ss");
        }

        public static void Nawigacja(string UriMiejsceDocelowe)
        {
            GlownaRamka.Navigate(new Uri(UriMiejsceDocelowe, UriKind.Relative));
        }

        private void btnZamknij_Click(object sender, RoutedEventArgs e)
        {
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
