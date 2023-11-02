using InżynierkaBiblioteka.BazaDanych;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Printing.IndexedProperties;
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
    /// Interaction logic for StworzNoweKonto.xaml
    /// </summary>
    public partial class StworzNoweKonto : Page
    {
        private static int WybranaPlecID = 1;
        private static List<string> ListaPlci = new List<string>();
        public StworzNoweKonto()
        {
            InitializeComponent();
            try
            {
                using (var MyDatabase = new MyDbContext())
                {
                    foreach (var item in MyDatabase.Plec.OrderBy(x => x.id))
                    {
                        ListaPlci.Add(item.Nazwa);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Blad! {ex.Message}");
                MainWindow.Nawigacja("GlowneHaslo.xaml");
            }
            
            comboBoxPlec.ItemsSource = ListaPlci;
            comboBoxPlec.SelectedIndex = 0;

        }

        private static string StworzHash(string Haslo, string SolString = "BibliotekaInzynieria")
        {
            byte[] SolIHaslo = Encoding.UTF8.GetBytes(SolString + Haslo);
            byte[] Hash;
            using(HashAlgorithm hs = SHA256.Create())
            {
                Hash = hs.ComputeHash(SolIHaslo);
            }
            StringBuilder HashJakoString = new StringBuilder();
            for (int i = 0; i < Hash.Length; i++)
            {
                HashJakoString.Append(Hash[i].ToString("x2"));
            }

            return HashJakoString.ToString();
        }

        private void comboBoxPlec_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBoxPlec.SelectedItem.ToString() == "Inna")
            {
                txtBoxPlec.Visibility = Visibility.Visible;
            }
            else
            {
                WybranaPlecID = comboBoxPlec.SelectedIndex + 1;
            }
            
        }

        private void btnStworzUzytkownika_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(txtBoxEmail.Text) || String.IsNullOrEmpty(txtBoxHaslo.Text) || String.IsNullOrEmpty(txtBoxImie.Text) || String.IsNullOrEmpty(txtBoxLogin.Text) || String.IsNullOrEmpty(txtBoxNazwisko.Text) || String.IsNullOrEmpty(txtBoxNrTelefonu.Text) || (comboBoxPlec.SelectedItem.ToString() == "Inna" && String.IsNullOrEmpty(txtBoxPlec.Text)))
            {
                MessageBox.Show("Blad! Wypelnij wszystkie wymagane pola");
            }
            else
            {
                if (comboBoxPlec.SelectedItem.ToString() == "Inna")
                {
                    try
                    {
                        using (var MyDatabase = new MyDbContext())
                        {
                            MyDatabase.Plec.Add(new Plec() { Nazwa = txtBoxPlec.Text });
                            MyDatabase.SaveChanges();
                            WybranaPlecID = MyDatabase.Plec.First(p => p.Nazwa == txtBoxPlec.Text).id;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Blad! {ex.Message}");
                        MainWindow.Nawigacja("GlowneHaslo.xaml");
                    }
                    
                }

                try
                {
                    using (var MyDatabase = new MyDbContext())
                    {
                        MyDatabase.Uzytkownik.Add(new Uzytkownik() { }); //TODO: Do zakonczenia
                        MyDatabase.SaveChanges();
                        MessageBox.Show("Dodano Uzytkownika");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Blad! {ex.Message}");
                    MainWindow.Nawigacja("GlowneHaslo.xaml");
                }
            }
        }
    }
}
