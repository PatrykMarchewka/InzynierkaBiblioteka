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

namespace InżynierkaBiblioteka.Administrator
{
    /// <summary>
    /// Interaction logic for AdminWyszukajUzytkownikow.xaml
    /// </summary>
    public partial class AdminWyszukajUzytkownikow : Page
    {
        HashSet<Uzytkownicy> Lista;

        Setter cornerRadiusSetter = new Setter
        {
            Property = Border.CornerRadiusProperty,
            Value = new CornerRadius(10)
        };

        //Setter backgroundSetter = new Setter
        //{
        //    Property = Button.BackgroundProperty,
        //    Value = new SolidColorBrush(Colors.Black)
        //};

        //Setter foregroundSetter = new Setter
        //{
        //    Property = Button.ForegroundProperty,
        //    Value = new SolidColorBrush(Colors.White)
        //};



        public AdminWyszukajUzytkownikow()
        {
            InitializeComponent();

            Wyszukaj();
        }

        private void Wyszukaj()
        {
            Style style = new Style(typeof(Border));
            style.Setters.Add(cornerRadiusSetter);

            //style.Setters.Add(backgroundSetter);
            //style.Setters.Add(foregroundSetter);


            Stack.Children.Clear();
            Stack.BeginInit();
            //TODO: Wywala blad bo pomagalem patrycji i wprowadzila zle dane, poprawic baze aby pola byly NOT NULL wtedy bedzie dzialac
            Lista = GlowneOkno.BazaDanych.Uzytkownicy.Where(u => EF.Functions.Like(u.Imie, $"%{txtBoxWyszukaj.Text}%") || EF.Functions.Like(u.Nazwisko, $"%{txtBoxWyszukaj.Text}%") || EF.Functions.Like(u.email, $"%{txtBoxWyszukaj.Text}%") || EF.Functions.Like(u.LoginUzytkownika, $"%{txtBoxWyszukaj.Text}%")).ToHashSet();

            
            if (chkBoxZaleglosci.IsChecked == true)
            {
                //Lista = Lista.Where(u => u.WszystkieZaleglosci.Any(z => z.Uzytkownicy == u && z.Zaplacono == false)).ToHashSet();
                Lista = Lista.Where(u => !u.WszystkieZaleglosci.All(z => z.Zaplacono == true)).ToHashSet();
            }
            if (chkBoxZbanowani.IsChecked == true)
            {
                Lista = Lista.Where(u => u.StatusKonta.idStatusu == 2).ToHashSet();
            }
            if (chkBoxAdmini.IsChecked == true)
            {
                Lista = Lista.Where(u => u.Rola.idRoli == 2).ToHashSet();
            }


            foreach (var item in Lista)
            {
                var kopiaItemu = item;
                Button button = new Button();
                button.Margin = new Thickness(5);
                if (kopiaItemu.Rola.idRoli == 2)
                {
                    //Admin
                    button.Background = new SolidColorBrush(Colors.Gold);
                }
                else if(kopiaItemu.StatusKonta.idStatusu == 2)
                {
                    //Zbanowany
                    button.Background = new SolidColorBrush(Colors.Red);
                }
                else
                {
                    //Inny
                    button.Background = new SolidColorBrush(Colors.Black);
                }
                button.Foreground = new SolidColorBrush(Colors.White);
                
                button.Content = $" Login: {kopiaItemu.LoginUzytkownika} Imie: {kopiaItemu.Imie} {kopiaItemu.Nazwisko} Email: {kopiaItemu.email} ";
                button.FontSize = 20;
                button.HorizontalAlignment = HorizontalAlignment.Center;
                button.Resources.Add(style.TargetType, style);
                button.Click += (s,e) => Button_Click(s,e,kopiaItemu);
                Stack.Children.Add(button);
            }
            Stack.EndInit();
        }

        private void Button_Click(object sender, RoutedEventArgs e, Uzytkownicy u)
        {
            GlowneOkno.ZalogowanyUzytkownik = u;
            MainWindow.Nawigacja("PoZalogowaniuUzytkownik.xaml");
        }

        private void txtBoxWyszukaj_TextChanged(object sender, TextChangedEventArgs e)
        {
            Wyszukaj();
        }

        private void btnPowrot_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.GlownaRamka.GoBack();
        }

        private void chkBoxZbanowani_Checked(object sender, RoutedEventArgs e)
        {
            Wyszukaj();
        }

        private void chkBoxZaleglosci_Checked(object sender, RoutedEventArgs e)
        {
            Wyszukaj();
        }

        private void chkBoxAdmini_Checked(object sender, RoutedEventArgs e)
        {
            Wyszukaj();
        }
    }
}
