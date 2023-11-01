﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
    /// Interaction logic for LogowanieHaslem.xaml
    /// </summary>
    public partial class LogowanieHaslem : Page
    {
        public LogowanieHaslem()
        {
            InitializeComponent();
        }

        private static void WeryfikacjaHasla(string Haslo, string SolString, string Hash)
        {
            byte[] SolIHaslo = Encoding.UTF8.GetBytes(SolString + Haslo);

            byte[] HashDoSprawdzenia;

            using (HashAlgorithm hs = SHA256.Create())
            {
                HashDoSprawdzenia = hs.ComputeHash(SolIHaslo);
            }

            StringBuilder HashJakoString = new StringBuilder();
            for (int i = 0; i < HashDoSprawdzenia.Length; i++)
            {
                HashJakoString.Append(HashDoSprawdzenia[i].ToString("x2"));
            }

            StringComparer stringComparer = StringComparer.OrdinalIgnoreCase;
            if (stringComparer.Compare(Hash, HashJakoString.ToString()) == 0)
            {
                MessageBox.Show("Dobre haslo");
            }
            else
            {
                MessageBox.Show("Zle haslo");
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            using (var MyDatabase = new MyDbContext())
            {

                MyDatabase.Plec.Add(new BazaDanych.Plec() { Nazwa="aa"});
                MyDatabase.SaveChanges();

            }

            //using (var MyDatabase = new MyDbContext())
            //{
            //    var sqlQuery = "SELECT * FROM Uzytkownik";
            //    var sqldwa = "Insert INTO Plec VALUES ('Ja')";
            //    var wynik = MyDatabase.Database.ExecuteSqlRaw(sqldwa);
            //    MessageBox.Show(wynik.ToString());
            //    MyDatabase.SaveChanges();
            //}


            //WeryfikacjaHasla(txtBoxZalogujHaslemHaslo.Text, "Sol", "To z bazy");
        }

        private void btnPowrot_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.GlownaRamka.GoBack();
        }
    }
}
