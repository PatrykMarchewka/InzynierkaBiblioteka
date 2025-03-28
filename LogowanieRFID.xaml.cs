﻿using System;
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
using static System.Net.Mime.MediaTypeNames;
using System.IO.Ports;
using InżynierkaBiblioteka.BazaDanych;
using System.Threading;

namespace InżynierkaBiblioteka
{
    /// <summary>
    /// Interaction logic for LogowanieRFID.xaml
    /// </summary>
    public partial class LogowanieRFID : Page
    {


        SerialPort sp = new SerialPort();

        CancellationTokenSource tokenSource = new CancellationTokenSource();
        CancellationToken token;


        public LogowanieRFID()
        {
            InitializeComponent();
            token = tokenSource.Token;
            Czytanie();
        }

        private void btnPowrot_Click(object sender, RoutedEventArgs e)
        {
            tokenSource.Cancel();
            sp.Close();
            MainWindow.GlownaRamka.GoBack();
        }

        private async void Czytanie()
        {
            string? Kod =  await CzytanieZCzytnika();
            sp.Close();

            if (Kod != null)
            {
                //Przypisanie czytnika RFID do uzytkownika
                if (GlowneOkno.ZalogowanyUzytkownik != null)
                {
                    Logi nowyLog = new Logi() { DataWystapienia = DateTime.UtcNow, TrescWiadomosci = "Dodano czytnik RFID", Uzytkownicy = GlowneOkno.ZalogowanyUzytkownik, Waznosc = 1 };
                    GlowneOkno.ZalogowanyUzytkownik.WszystkieLogi.Add(nowyLog);

                    Kod = StworzNoweKonto.StworzHash(Kod, GlowneOkno.ZalogowanyUzytkownik.salt);

                    GlowneOkno.ZalogowanyUzytkownik.RFID = Kod;
                    GlowneOkno.BazaDanych.SaveChanges();
                    MessageBox.Show("Dodano czytnik RFID");
                    MainWindow.GlownaRamka.GoBack();
                }
                else if (GlowneOkno.ZalogowanyAdministrator != null)
                {
                    Logi nowyLog = new Logi() { DataWystapienia = DateTime.UtcNow, TrescWiadomosci = "Dodano czytnik RFID", Uzytkownicy = GlowneOkno.ZalogowanyAdministrator, Waznosc = 1 };
                    GlowneOkno.ZalogowanyAdministrator.WszystkieLogi.Add(nowyLog);

                    Kod = StworzNoweKonto.StworzHash(Kod, GlowneOkno.ZalogowanyAdministrator.salt);

                    GlowneOkno.ZalogowanyAdministrator.RFID = Kod;
                    GlowneOkno.BazaDanych.SaveChanges();
                    MessageBox.Show("Dodano czytnik RFID");
                    MainWindow.GlownaRamka.GoBack();
                }
                else
                {
                    //Logowanie za pomoca zarejestrowanego RFID
                    Uzytkownicy? u = null;
                    foreach (var item in GlowneOkno.BazaDanych.Uzytkownicy)
                    {
                        if (item.RFID == StworzNoweKonto.StworzHash(Kod,item.salt))
                        {
                            u = item;
                        }
                    }

                    if (u != null)
                    {
                        if (u.Rola.idRoli == 1)
                        {
                            GlowneOkno.ZalogowanyUzytkownik = u;
                            GlowneOkno.ZalogowanyUzytkownik.DataOstatniegoLogowania = DateTime.UtcNow;
                            GlowneOkno.BazaDanych.SaveChanges();
                            MainWindow.Nawigacja("PoZalogowaniuUzytkownik.xaml");
                        }
                        else if (u.Rola.idRoli == 2)
                        {
                            GlowneOkno.ZalogowanyAdministrator = u;
                            GlowneOkno.ZalogowanyAdministrator.DataOstatniegoLogowania = DateTime.UtcNow;
                            GlowneOkno.BazaDanych.SaveChanges();
                            MainWindow.Nawigacja("Administrator/PoZalogowaniuAdmin.xaml");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Blad! Nieznany RFID");
                    }

                    
                }
            }
            else
            {
                
            }
        }

        private async Task<string> CzytanieZCzytnika()
        {
            token = tokenSource.Token;
            string? Kod = null;
            try
            {
                sp.BaudRate = 9600;
                sp.PortName = "COM8";
                sp.Open();

                try
                {
                    await Task.Run(() =>
                    {
                        Kod = sp.ReadLine();
                        token.ThrowIfCancellationRequested();

                    },token);
                    return Kod;
                }
                catch (System.OperationCanceledException ex)
                {
                    return null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Problem z inicjalizacja zadania do czytania RFID!");
                    Logi nowyLog = new Logi() { DataWystapienia = DateTime.UtcNow, TrescWiadomosci = "Problem z inicjalizacja zadania do czytania RFID", Waznosc = 10, Uzytkownicy = GlowneOkno.BazaDanych.Uzytkownicy.First() };
                    GlowneOkno.BazaDanych.Logi.Add(nowyLog);
                    GlowneOkno.BazaDanych.SaveChanges();
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nie mozna otworzyc portu, sprawdz numer portu i zamknij wszystkie aplikacje korzystajace z niego!");
                Logi nowyLog = new Logi() { DataWystapienia = DateTime.UtcNow, TrescWiadomosci = "Nie mozna otworzyc portu, sprawdz numer portu i zamknij wszystkie aplikacje korzystajace z niego", Waznosc = 10, Uzytkownicy = GlowneOkno.BazaDanych.Uzytkownicy.First() };
                GlowneOkno.BazaDanych.Logi.Add(nowyLog);
                GlowneOkno.BazaDanych.SaveChanges();
                return null;
            }
        }
    }
}
