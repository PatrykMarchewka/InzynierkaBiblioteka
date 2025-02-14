using InżynierkaBiblioteka.Administrator;
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
    /// Interaction logic for ZobaczZaleglosci.xaml
    /// </summary>
    public partial class ZobaczZaleglosci : Page
    {

        Setter cornerRadiusSetter = new Setter
        {
            Property = Border.CornerRadiusProperty,
            Value = new CornerRadius(10)
        };


        public ZobaczZaleglosci()
        {
            InitializeComponent();

            if (GlowneOkno.ZalogowanyAdministrator == null)
            {
                btnDodajZaleglosc.Visibility = Visibility.Hidden;
                lblZaleglosci.Visibility = Visibility.Visible;
            }
            else
            {
                btnDodajZaleglosc.Visibility = Visibility.Visible;
                lblZaleglosci.Visibility = Visibility.Hidden;
            }


            Style style = new Style(typeof(Border));
            style.Setters.Add(cornerRadiusSetter);
            foreach (var item in GlowneOkno.ZalogowanyUzytkownik.WszystkieZaleglosci)
            {
                var KopiaItemu = item;
                Button button = new Button();
                button.Margin = new Thickness(5);
                button.Foreground = new SolidColorBrush(Colors.White);
                button.FontSize = 20;
                button.HorizontalAlignment = HorizontalAlignment.Center;
                button.Resources.Add(style.TargetType, style);
                if (KopiaItemu.Zaplacono)
                {
                    button.Background = new SolidColorBrush(Colors.Gold);
                    button.Content = $" {KopiaItemu.Ksiazka?.ISBN ?? "(Brak ksiazki)"} {KopiaItemu.Ksiazka?.TytulKsiazki ?? ""} - {KopiaItemu.Zaleglosc:C} {KopiaItemu.Komentarz} ZAPLACONO ";
                }
                else
                {
                    button.Background = new SolidColorBrush(Colors.Black);
                    button.Content = $" {KopiaItemu.Ksiazka?.ISBN ?? "(Brak ksiazki)"} {KopiaItemu.Ksiazka?.TytulKsiazki ?? ""} - {KopiaItemu.Zaleglosc:C} {KopiaItemu.Komentarz} ";
                }
                Stack.Children.Add(button);
                button.Click += (s,e) => Button_Click(s,e,KopiaItemu);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e, Zaleglosci z)
        {
            if (GlowneOkno.ZalogowanyAdministrator != null)
            {
                DodajZaleglosci.Zaleglosc = z;
                MainWindow.Nawigacja("Administrator/DodajZaleglosci.xaml");

            }
        }

        private void btnPowrot_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.GlownaRamka.GoBack();
        }

        private void btnDodajZaleglosc_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Nawigacja("Administrator/DodajZaleglosci.xaml");
        }
    }
}
