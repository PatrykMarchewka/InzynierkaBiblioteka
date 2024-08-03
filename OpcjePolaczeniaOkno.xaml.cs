using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
using System.Windows.Shapes;

namespace InżynierkaBiblioteka
{
    /// <summary>
    /// Interaction logic for OpcjePolaczeniaOkno.xaml
    /// </summary>
    public partial class OpcjePolaczeniaOkno : Window
    {
        private static bool OnClosingBool = true;
        public OpcjePolaczeniaOkno()
        {
            InitializeComponent();
            if (File.Exists(Directory.GetCurrentDirectory() + @"\Connection.json"))
            {
                JSONStructure JSON = JsonSerializer.Deserialize<JSONStructure>(File.ReadAllText(Directory.GetCurrentDirectory() + @"\Connection.json"));
                txtBoxSerwer.Text = JSON.Serwer;
                txtBoxNazwaBazy.Text = JSON.NazwaBazy;
                txtBoxLogin.Text = JSON.Login;
                txtBoxHaslo.Text = JSON.Haslo;
                JSON = null;
            }

            this.Closing += OpcjePolaczeniaOkno_Closing;
        }

        private void OpcjePolaczeniaOkno_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            if (OnClosingBool)
            {
                Environment.Exit(0);
            }
            else
            {
                e.Cancel = true;
                this.Hide();
            }
        }

        private void btnZapisz_Click(object sender, RoutedEventArgs e)
        {
            JSONStructure JSON = new JSONStructure
            {
                Serwer = txtBoxSerwer.Text,
                NazwaBazy = txtBoxNazwaBazy.Text,
                Login = txtBoxLogin.Text,
                Haslo = txtBoxHaslo.Text
            };

            MyDbContext.ConnectionString = $"Server={JSON.Serwer};Database={JSON.NazwaBazy};User Id={JSON.Login};Password={JSON.Haslo};Encrypt=False;";
            bool connected;
            using (MyDbContext db = new MyDbContext())
            {
                connected = db.Database.CanConnect();
            }
            
            if (!connected)
            {
                MyDbContext.ConnectionString = null;
                JSON = null;
                MessageBox.Show("Nie mozna polaczyc sie z baza danych!", "BLAD!");
                return;
            }
            using (StreamWriter sw = new StreamWriter(Directory.GetCurrentDirectory() + @"\Connection.json"))
            {
                sw.Write(JsonSerializer.Serialize(JSON));
            };
            JSON = null;


            if(GlowneOkno.BazaDanych != null)
            {
                GlowneOkno.BazaDanych.Dispose();
                GlowneOkno.BazaDanych = null;
            }
            GlowneOkno.BazaDanych = new MyDbContext();
            MainWindow.Nawigacja("GlowneOkno.xaml");
            OnClosingBool = false;
            this.Close();
            OnClosingBool = true;
        }







    }
}
