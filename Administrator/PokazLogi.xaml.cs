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

namespace InżynierkaBiblioteka.Administrator
{
    /// <summary>
    /// Interaction logic for PokazLogi.xaml
    /// </summary>
    public partial class PokazLogi : Page
    {
        public PokazLogi()
        {
            InitializeComponent();
            Wyszukaj();
        }

        private void Wyszukaj()
        {
            Stack.Children.Clear();
            Stack.BeginInit();
            HashSet<Logi> Logi = GlowneOkno.BazaDanych.Logi.ToHashSet();
            foreach (var item in Logi)
            {
                var kopiaItemu = item;
                Label lbl = new Label();
                lbl.Content = $"{kopiaItemu.idLoga}: {kopiaItemu.TrescWiadomosci}, uzytkownik {kopiaItemu.Uzytkownicy}, waznosc {kopiaItemu.Waznosc}, data {kopiaItemu.DataWystapienia}";
                lbl.Margin = new Thickness(5);
                Stack.Children.Add(lbl);
            }
            Stack.EndInit();
        }

        private void btnPowrot_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.GlownaRamka.GoBack();
        }
    }
}
