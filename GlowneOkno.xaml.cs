using InżynierkaBiblioteka.BazaDanych;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for GlowneOkno.xaml
    /// </summary>
    public partial class GlowneOkno : Page
    {
        public static Uzytkownicy? ZalogowanyUzytkownik;
        public static Uzytkownicy? ZalogowanyAdministrator;
        public static MyDbContext BazaDanych = new MyDbContext();
        private Page _orginalnaStrona;


        public GlowneOkno()
        {
            InitializeComponent();


            ZalogowanyUzytkownik = null;
            ZalogowanyAdministrator = null;


            HashSet<string> brakujaceTabele = new HashSet<string>();
            try
            {
                BazaDanych.Database.EnsureCreated();
                foreach (var item in BazaDanych.Model.GetEntityTypes())
                {
                    var tabela = item.GetTableName();

                    if (!SprawdzTabele(tabela))
                    {
                        brakujaceTabele.Add(tabela);

                    }
                }
                if (brakujaceTabele.Count != 0)
                {
                    throw new Exception("Tabela");
                }
                else
                {
                    if (!SprawdzRekord("Uzytkownicy", "LoginUzytkownika", "Admin"))
                    {
                        MessageBoxResult result = MessageBox.Show("Czy chcesz dodac podstawowe dane do tabel?", "Podstawowe dane", MessageBoxButton.YesNo);
                        if (result == MessageBoxResult.Yes)
                        {
                            DodajDanePodstawowe(WszystkieTabele());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message == "Tabela")
                {
                    string wiadomosc = "Brakuje tabel:";
                    foreach (var item in brakujaceTabele)
                    {
                        wiadomosc += $" {item} ";
                    }
                    MessageBoxResult result = MessageBox.Show($"{wiadomosc} - Czy chcesz stworzyc tabele teraz?", "Brak tabel!", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        StworzTabele(brakujaceTabele);
                    }
                    else
                    {
                        Environment.Exit(0);
                    }
                }
                else
                {
                    if (MessageBox.Show("Nie mozna ustanowic polaczenia z baza danych, czy chcesz edytowac plik Connection.json?", "Blad polaczenia!", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        MainWindow.Nawigacja("PustaStrona.xaml");
                        MainWindow.opcjePolaczeniaOkno.Show();
                    }
                    else
                    {
                        Environment.Exit(0);
                    }
                }

            }
            if (!SprawdzUC())
            {
                DodajUC();
            }
            MyDbContext.ConnectionString = null;
            GC.Collect();
        }

        private void btnZalogujHaslem_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Nawigacja("LogowanieHaslem.xaml");
        }

        private void btnZalogujRFID_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Nawigacja("LogowanieRFID.xaml");
        }

        private void btnStworzNoweKonto_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Nawigacja("StworzNoweKonto.xaml");
        }

        private static bool SprawdzTabele(string tabela)
        {
            string sql = $"SELECT COUNT(*) FROM information_schema.tables WHERE table_name = '{tabela}'";
            using (var command = BazaDanych.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = sql;
                BazaDanych.Database.OpenConnection();

                using (var result = command.ExecuteReader())
                {
                    if (result.Read())
                    {
                        return result.GetInt32(0) > 0;
                    }
                }
            }
            return false;
        }

        private static HashSet<string> WszystkieTabele()
        {
            HashSet<string> wynik = new HashSet<string>();
            string sql = $"SELECT * FROM information_schema.tables";
            using (var command = BazaDanych.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = sql;
                BazaDanych.Database.OpenConnection();
                using (var result = command.ExecuteReader())
                {
                    while (result.Read())
                    {
                        wynik.Add(result.GetString(2));
                    }
                }
            }
            wynik.Remove("sysdiagrams");
            return wynik;
        }

        private static bool SprawdzRekord(string tabela, string kolumna, string tekst)
        {
            try
            {
                string sql = $"SELECT COUNT(*) FROM {tabela} WHERE {kolumna} = {tekst}";
                using (var command = BazaDanych.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = sql;
                    BazaDanych.Database.OpenConnection();
                    using (var result = command.ExecuteReader())
                    {
                        if (result.Read())
                        {
                            return result.GetInt32(0) > 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string sql = $"SELECT COUNT(*) FROM {tabela} WHERE {kolumna} = '{tekst}'";
                using (var command = BazaDanych.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = sql;
                    BazaDanych.Database.OpenConnection();
                    using (var result = command.ExecuteReader())
                    {
                        if (result.Read())
                        {
                            return result.GetInt32(0) > 0;
                        }
                    }
                }
            }
            return false;
        }

        private static void StworzTabele(HashSet<string> Tabele)
        {
            foreach (var item in Tabele)
            {
                string rawsql = "";
                switch (item)
                {
                    case "Plec":
                        rawsql = "CREATE TABLE Plec (idPlci INT PRIMARY KEY IDENTITY,Nazwa nvarchar(255) UNIQUE NOT NULL);";
                        break;
                    case "Autorzy":
                        rawsql = "CREATE TABLE Autorzy (idAutora INT PRIMARY KEY IDENTITY,ImieAutora nvarchar(255) NOT NULL,NazwiskoAutora nvarchar(255) NOT NULL);";
                        break;
                    case "Jezyki":
                        rawsql = "CREATE TABLE Jezyki (idJezyka INT PRIMARY KEY IDENTITY,Nazwa nvarchar(255) UNIQUE NOT NULL);";
                        break;
                    case "Role":
                        rawsql = "CREATE TABLE Role (idRoli INT PRIMARY KEY IDENTITY,Nazwa nvarchar(255) UNIQUE NOT NULL);";
                        break;
                    case "GatunkiKsiazek":
                        rawsql = "CREATE TABLE GatunkiKsiazek (idGatunku INT PRIMARY KEY IDENTITY,Nazwa nvarchar(255) UNIQUE NOT NULL);";
                        break;
                    case "Statusy":
                        rawsql = "CREATE TABLE Statusy(idStatusu int primary key identity,Nazwa nvarchar(255) UNIQUE NOT NULL);";
                        break;
                    case "Uzytkownicy":
                        rawsql = "CREATE TABLE Uzytkownicy (idUzytkownika INT PRIMARY KEY IDENTITY,RFID nvarchar(255) NULL,RFIDPin INT NULL,LoginUzytkownika nvarchar(255) UNIQUE NOT NULL,salt nvarchar(255) DEFAULT 'BibliotekaInzynieria' NOT NULL,hashHaslo nvarchar(255) NOT NULL,email nvarchar(255) NULL,Imie nvarchar(255) NOT NULL,Nazwisko nvarchar(255) NOT NULL,idPlci INT NOT NULL,nrTelefonu nvarchar(20) NULL,idRoli INT NOT NULL,LiczbaWypozyczonychKsiazek INT DEFAULT 0 NOT NULL,DataStworzeniaKonta DATETIME NOT NULL,idStatusu INT NOT NULL,Komentarze nvarchar(MAX) NULL,DataOstatniegoLogowania DATETIME NOT NULL,FOREIGN KEY (idPlci) REFERENCES Plec (idPlci),FOREIGN KEY (idRoli) REFERENCES Role (idRoli),FOREIGN KEY (idStatusu) REFERENCES Statusy (idStatusu));";
                        try
                        {
                            BazaDanych.Database.ExecuteSqlRaw(rawsql);
                        }
                        catch (Exception)
                        {
                        }
                        rawsql = "CREATE UNIQUE NONCLUSTERED INDEX idx_RFID_notnull ON Uzytkownicy(RFID) WHERE RFID IS NOT NULL;";

                        break;
                    case "Ksiazki":
                        rawsql = "CREATE TABLE Ksiazki (idKsiazki int primary key identity,ISBN nvarchar(13) UNIQUE NOT NULL,TytulKsiazki nvarchar(255) NOT NULL,idGatunku INT NOT NULL,RokPublikacjiKsiazki INT NOT NULL,idJezyka INT NOT NULL,IloscStron INT NOT NULL,DostepnoscKsiazki INT NOT NULL,LiczbaOczekujacych INT NOT NULL,DoWypozyczenia BIT NOT NULL,IloscWypozyczen30Dni INT NOT NULL,FOREIGN KEY (idJezyka) REFERENCES Jezyki (idJezyka),FOREIGN KEY (idGatunku) REFERENCES GatunkiKsiazek (idGatunku));";
                        break;
                    case "HashKsiazkiAutorzy":
                        rawsql = "CREATE TABLE HashKsiazkiAutorzy (idKsiazki int NOT NULL,idAutora INT NOT NULL,PRIMARY KEY (idKsiazki, idAutora),FOREIGN KEY (idKsiazki) REFERENCES Ksiazki (idKsiazki),FOREIGN KEY (idAutora) REFERENCES Autorzy (idAutora));";
                        break;
                    case "Wypozyczenia":
                        rawsql = "CREATE TABLE Wypozyczenia (idWypozyczenia INT PRIMARY KEY IDENTITY,idUzytkownika INT NOT NULL,idKsiazki int NOT NULL,DataWypozyczenia DATETIME NOT NULL,DataDoOddania DATETIME NOT NULL,DataAktualnegoOddania DATETIME NULL,FOREIGN KEY (idUzytkownika) REFERENCES Uzytkownicy (idUzytkownika),FOREIGN KEY (idKsiazki) REFERENCES Ksiazki (idKsiazki));";
                        break;
                    case "Powiadomienia":
                        rawsql = "CREATE TABLE Powiadomienia (idPowiadomienia INT PRIMARY KEY IDENTITY,idKsiazki int NOT NULL,idUzytkownika INT NOT NULL,KiedyStworzono DATETIME NOT NULL,KiedyWyslanoMail DATETIME NULL,FOREIGN KEY (idKsiazki) REFERENCES Ksiazki (idKsiazki),FOREIGN KEY (idUzytkownika) REFERENCES Uzytkownicy (idUzytkownika),CONSTRAINT UC_Powiadomienia UNIQUE (ISBNKsiazki, idUzytkownika));";
                        break;
                    case "Recenzje":
                        rawsql = "CREATE TABLE Recenzje (IDRecenzji INT PRIMARY KEY IDENTITY,idUzytkownika INT NOT NULL,idKsiazki int NOT NULL,DataWystawienia DATETIME NOT NULL,Ocena INT NOT NULL,TekstRecenzji nvarchar(MAX) NULL,Ukryta BIT NOT NULL,FOREIGN KEY (idUzytkownika) REFERENCES Uzytkownicy (idUzytkownika),FOREIGN KEY (idKsiazki) REFERENCES Ksiazki (idKsiazki),CONSTRAINT UC_Recenzje UNIQUE (idUzytkownika, ISBNKsiazki));";
                        break;
                    case "Reporty":
                        rawsql = "CREATE TABLE Reporty (IDReportu INT PRIMARY KEY IDENTITY,idUzytkownika INT NOT NULL, idRecenzji INT NOT NULL,TrescRaportu nvarchar(MAX) NOT NULL,StatusRaportu BIT NOT NULL,FOREIGN KEY (idUzytkownika) REFERENCES Uzytkownicy (idUzytkownika),FOREIGN KEY (idRecenzji) REFERENCES Recenzje (IDRecenzji));";
                        break;
                    case "Logi":
                        rawsql = "CREATE TABLE Logi(idLoga INT PRIMARY KEY IDENTITY,idUzytkownika INT NOT NULL,waznosc INT NOT NULL,TrescWiadomosci nvarchar(MAX) NOT NULL,DataWystapienia DATETIME NOT NULL,FOREIGN KEY (idUzytkownika) REFERENCES Uzytkownicy(idUzytkownika))";
                        break;
                    case "Zaleglosci":
                        rawsql = "CREATE TABLE Zaleglosci (idZaleglosci INT PRIMARY KEY IDENTITY,idUzytkownika INT NOT NULL,idKsiazki INT NULL NOT NULL,Zaleglosc DECIMAL NOT NULL,Zaplacono BIT NOT NULL,Komentarz nvarchar(255) NOT NULL,FOREIGN KEY (idUzytkownika) REFERENCES Uzytkownicy (idUzytkownika),FOREIGN KEY (idKsiazki) REFERENCES Ksiazki (idKsiazki))";
                        break;
                    default:
                        MessageBox.Show($"Nieznana tabela {item} nie moze zostac odtworzona");
                        break;
                }
                try
                {
                    //Try catch na wypadek gdyby EnsureCreated stworzyl tabele!
                    BazaDanych.Database.ExecuteSqlRaw(rawsql);
                }
                catch (Exception)
                {
                    MessageBox.Show($"Brakujaca tabela {item} nie moze zostac odtworzona");
                }
            }
            MessageBoxResult result = MessageBox.Show("Czy chcesz dodac podstawowe dane do stworzonych tabel?", "Podstawowe dane", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                DodajDanePodstawowe(Tabele);
            }
        }
        private static void DodajDanePodstawowe(HashSet<string> Tabele)
        {
            foreach (var item in Tabele)
            {
                string rawsql = "";
                switch (item)
                {
                    case "Autorzy":
                    case "Ksiazki":
                    case "HashKsiazkiAutorzy":
                    case "Wypozyczenia":
                    case "Powiadomienia":
                    case "Recenzje":
                    case "Reporty":
                    case "Logi":
                    case "Zaleglosci":
                        break;
                    case "Jezyki":
                        rawsql = "INSERT INTO Jezyki (Nazwa) VALUES ('Czeski'); INSERT INTO Jezyki (Nazwa) VALUES ('Sotho'); INSERT INTO Jezyki (Nazwa) VALUES ('Ormiański'); INSERT INTO Jezyki (Nazwa) VALUES ('Bislama'); INSERT INTO Jezyki (Nazwa) VALUES ('Hiri Motu'); INSERT INTO Jezyki (Nazwa) VALUES ('Angielski'); INSERT INTO Jezyki (Nazwa) VALUES ('Marathi'); INSERT INTO Jezyki (Nazwa) VALUES ('Guarani'); INSERT INTO Jezyki (Nazwa) VALUES ('Tetum'); INSERT INTO Jezyki (Nazwa) VALUES ('Ndebeli'); INSERT INTO Jezyki (Nazwa) VALUES ('Hiszpański'); INSERT INTO Jezyki (Nazwa) VALUES ('Oryja'); INSERT INTO Jezyki (Nazwa) VALUES ('Fidżyjski'); INSERT INTO Jezyki (Nazwa) VALUES ('Keczua'); INSERT INTO Jezyki (Nazwa) VALUES ('Bułgarski'); INSERT INTO Jezyki (Nazwa) VALUES ('Papiamento'); INSERT INTO Jezyki (Nazwa) VALUES ('Kirgizki'); INSERT INTO Jezyki (Nazwa) VALUES ('Tok Pisin'); INSERT INTO Jezyki (Nazwa) VALUES ('Mongolski'); INSERT INTO Jezyki (Nazwa) VALUES ('Tajski'); INSERT INTO Jezyki (Nazwa) VALUES ('Irlandzki'); INSERT INTO Jezyki (Nazwa) VALUES ('Afrykański'); INSERT INTO Jezyki (Nazwa) VALUES ('Filipiński'); INSERT INTO Jezyki (Nazwa) VALUES ('Włoski'); INSERT INTO Jezyki (Nazwa) VALUES ('Kurdyjski'); INSERT INTO Jezyki (Nazwa) VALUES ('Gagauz'); INSERT INTO Jezyki (Nazwa) VALUES ('Azerbejdżański'); INSERT INTO Jezyki (Nazwa) VALUES ('Malajalam'); INSERT INTO Jezyki (Nazwa) VALUES ('Islandzki'); INSERT INTO Jezyki (Nazwa) VALUES ('Malajski'); INSERT INTO Jezyki (Nazwa) VALUES ('Dzongkha'); INSERT INTO Jezyki (Nazwa) VALUES ('Fiński'); INSERT INTO Jezyki (Nazwa) VALUES ('Tsonga'); INSERT INTO Jezyki (Nazwa) VALUES ('Nowozelandzki język migowy'); INSERT INTO Jezyki (Nazwa) VALUES ('Swati'); INSERT INTO Jezyki (Nazwa) VALUES ('Luksemburski'); INSERT INTO Jezyki (Nazwa) VALUES ('Arabski'); INSERT INTO Jezyki (Nazwa) VALUES ('Tamilski'); INSERT INTO Jezyki (Nazwa) VALUES ('Nepalski'); INSERT INTO Jezyki (Nazwa) VALUES ('Dari'); INSERT INTO Jezyki (Nazwa) VALUES ('Kataloński'); INSERT INTO Jezyki (Nazwa) VALUES ('Białoruski'); INSERT INTO Jezyki (Nazwa) VALUES ('Sotho Północny'); INSERT INTO Jezyki (Nazwa) VALUES ('Paszto'); INSERT INTO Jezyki (Nazwa) VALUES ('Jidysz'); INSERT INTO Jezyki (Nazwa) VALUES ('Łotewski'); INSERT INTO Jezyki (Nazwa) VALUES ('Duński'); INSERT INTO Jezyki (Nazwa) VALUES ('Indonezyjski'); INSERT INTO Jezyki (Nazwa) VALUES ('Bengalski'); INSERT INTO Jezyki (Nazwa) VALUES ('Kaszmirski'); INSERT INTO Jezyki (Nazwa) VALUES ('Rumuński'); INSERT INTO Jezyki (Nazwa) VALUES ('Węgierski'); INSERT INTO Jezyki (Nazwa) VALUES ('Maoryski'); INSERT INTO Jezyki (Nazwa) VALUES ('Grecki'); INSERT INTO Jezyki (Nazwa) VALUES ('Montenegrynski');";
                        break;
                    case "GatunkiKsiazek":
                        rawsql = "INSERT INTO GatunkiKsiazek (Nazwa) VALUES ('Dramat'); INSERT INTO GatunkiKsiazek (Nazwa) VALUES ('Romans'); INSERT INTO GatunkiKsiazek (Nazwa) VALUES ('Komedia'); INSERT INTO GatunkiKsiazek (Nazwa) VALUES ('Dokumentalny'); INSERT INTO GatunkiKsiazek (Nazwa) VALUES ('Thriller'); INSERT INTO GatunkiKsiazek (Nazwa) VALUES ('Western');";
                        break;
                    case "Plec":
                        rawsql = "Insert into Plec (Nazwa) values ('Mezczyzna'); Insert into Plec (Nazwa) values ('Kobieta');";
                        break;
                    case "Role":
                        rawsql = "insert into Role(Nazwa) values('Uzytkownik'); insert into Role(Nazwa) values('Administrator');";
                        break;
                    case "Statusy":
                        rawsql = "insert into Statusy(Nazwa) values('Nowy uzytkownik'); insert into Statusy(Nazwa) values ('Konto zbanowane');";
                        break;
                    case "Uzytkownicy":
                        rawsql = "INSERT INTO [dbo].[Uzytkownicy] ([RFID], [RFIDPin], [LoginUzytkownika], [salt], [hashHaslo], [email], [Imie], [Nazwisko], [nrTelefonu], [LiczbaWypozyczonychKsiazek], [DataStworzeniaKonta], [idPlci], [idRoli], [idStatusu], [DataOstatniegoLogowania]) VALUES (NULL, NULL, N'Admin', N'BibliotekaInzynieria', N'48b2a14ff8ec7f3de8d8da95f774d033684764d4322855019d803ba86c3e9378', NULL, N'Admin', N'Admin', NULL, 0, GETUTCDATE(), 1, 2, 1, GETUTCDATE());";
                        break;
                    default:
                        MessageBox.Show($"Nieznana tabela {item}");
                        break;
                }
                try
                {
                    if (!String.IsNullOrWhiteSpace(rawsql))
                    {
                        BazaDanych.Database.ExecuteSqlRaw(rawsql);
                        if (item == "Uzytkownicy")
                        {
                            MessageBox.Show("Stworzono uzytkownika o roli administratora, Login = Admin, Haslo = Admin");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Dane dla tabeli {item} nie moga zostac dodane");

                }
            }
        }

        private static bool SprawdzUC()
        {

            string sql = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_NAME = 'UC_Powiadomienia'";
            using (var command = BazaDanych.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = sql;
                BazaDanych.Database.OpenConnection();
                try
                {
                    using (var result = command.ExecuteReader())
                    {
                        if (result.Read())
                        {
                            if (result.GetInt32(0) == 0)
                            {
                                return false;
                            }
                        }
                    }
                }
                catch (Microsoft.Data.SqlClient.SqlException ex)
                {
                    return false;
                }
            }
            sql = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_NAME = 'UC_Recenzje'";
            using (var command = BazaDanych.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = sql;
                BazaDanych.Database.OpenConnection();
                try
                {
                    using (var result = command.ExecuteReader())
                    {
                        if (result.Read())
                        {
                            if (result.GetInt32(0) == 0)
                            {
                                return false;
                            }
                        }
                    }
                }
                catch (Microsoft.Data.SqlClient.SqlException ex)
                {
                    return false;
                }
            }
            return true;
        }

        private static void DodajUC()
        {
            string rawsql = "";
            rawsql = "ALTER TABLE Powiadomienia ADD CONSTRAINT UC_Powiadomienia UNIQUE (idKsiazki, idUzytkownika);";
            try
            {
                BazaDanych.Database.ExecuteSqlRaw(rawsql);
            }
            catch (Exception)
            {

            }
            rawsql = "ALTER TABLE Recenzje ADD CONSTRAINT UC_Recenzje UNIQUE (idUzytkownika, idKsiazki);";
            try
            {
                BazaDanych.Database.ExecuteSqlRaw(rawsql);
            }
            catch (Exception)
            {

            }
        }
    }
}
