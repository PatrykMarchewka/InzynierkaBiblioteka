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
    /// Interaction logic for RaportujRecenzje.xaml
    /// </summary>
    public partial class RaportujRecenzje : Page
    {
        public RaportujRecenzje()
        {
            InitializeComponent();
            Reporty report = GlowneOkno.BazaDanych.Reporty.FirstOrDefault(r => r.Recenzje == ZobaczRecenzje.recenzja && r.Reportujacy == GlowneOkno.ZalogowanyUzytkownik);
            if (report != null)
            {
                txtBoxReport.Text = report.TrescRaportu;
            }
        }

        private void btnPowrot_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.GlownaRamka.GoBack();
        }

        private void btnWyslijReport_Click(object sender, RoutedEventArgs e)
        {
            Reporty report;
            if (GlowneOkno.BazaDanych.Reporty.Any(r => r.Recenzje == ZobaczRecenzje.recenzja && r.Reportujacy == GlowneOkno.ZalogowanyUzytkownik))
            {
                report = GlowneOkno.BazaDanych.Reporty.First(r => r.Recenzje == ZobaczRecenzje.recenzja && r.Reportujacy == GlowneOkno.ZalogowanyUzytkownik);
                report.TrescRaportu = txtBoxReport.Text;
                report.StatusRaportu = false;
                Logi nowylog = new Logi() { DataWystapienia = DateTime.UtcNow, Uzytkownicy = GlowneOkno.ZalogowanyUzytkownik, TrescWiadomosci = "Uzytkownik zmienil tresc zgloszenia", Waznosc = 1 };
                GlowneOkno.BazaDanych.Logi.Add(nowylog);
            }
            else
            {
                report = new Reporty() { Recenzje = ZobaczRecenzje.recenzja, StatusRaportu = false, TrescRaportu = txtBoxReport.Text, Reportujacy = GlowneOkno.ZalogowanyUzytkownik };
                GlowneOkno.BazaDanych.Reporty.Add(report);
                Logi nowylog = new Logi() { DataWystapienia = DateTime.UtcNow, Uzytkownicy = GlowneOkno.ZalogowanyUzytkownik, TrescWiadomosci = "Uzytkownik dodal zgloszenie", Waznosc = 1 };
                GlowneOkno.BazaDanych.Logi.Add(nowylog);
            }
            GlowneOkno.BazaDanych.SaveChanges();
            MessageBox.Show("Wyslano report");
            MainWindow.GlownaRamka.GoBack();
        }
    }
}
