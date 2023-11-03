using InżynierkaBiblioteka.BazaDanych;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InżynierkaBiblioteka
{
    internal class MyDbContext : DbContext
    {
        public DbSet<Uzytkownik> Uzytkownik { get; set; }
        public DbSet<Ksiazki> Ksiazki { get; set; }
        public DbSet<Autorzy> Autorzy { get; set; }
        public DbSet<Wypozyczenia> Wypozyczenia { get; set; }
        public DbSet<Statusy> Statusy { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Reporty> Reporty { get; set; }
        public DbSet<Recenzje> Recenzje { get; set; }
        public DbSet<Powiadomienia> Powiadomienia { get; set; }
        public DbSet<Plec> Plec { get; set; }
        public DbSet<Jezyki> Jezyki { get; set; }
        public DbSet<HashKsiazkiAutorzy> HashKsiazkiAutorzy { get; set; }
        public DbSet<GatunkiKsiazek> GatunkiKsiazek { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string serwer = "sql.bsite.net\\MSSQL2016";
            string Login = "patryk4610_test";
            string Haslo = "test";
            //string Login = "patryk4610_InzBib"; //Nazwa bazy danych jest taka sama jak loginn
            //string Haslo = "kHKPeKLLgWy7F3";


            optionsBuilder.UseSqlServer($"Server={serwer};Database={Login};User Id={Login};Password={Haslo};Encrypt=False");
        }

        protected override void OnModelCreating(ModelBuilder mod)
        {
            mod.Entity<Uzytkownik>().HasOne(u => u.Plec).WithMany(p => p.Uzytkownicy);
            mod.Entity<Uzytkownik>().HasOne(u => u.Rola).WithMany(r => r.Uzytkownicy);
            mod.Entity<Uzytkownik>().HasOne(u => u.StatusKonta).WithMany(s => s.Uzytkownicy);

            mod.Entity<HashKsiazkiAutorzy>().HasOne(h => h.Ksiazka).WithMany(k => k.Hashe);
            mod.Entity<HashKsiazkiAutorzy>().HasOne(h => h.Autor).WithMany(a => a.Hashe);

            mod.Entity<Wypozyczenia>().HasOne(w => w.Uzytkownik).WithMany(u => u.Wypozyczenia);
            mod.Entity<Wypozyczenia>().HasOne(w => w.Ksiazka).WithMany(k => k.Wypozyczenia);

            mod.Entity<Powiadomienia>().HasOne(p => p.Ksiazka).WithMany(k => k.Powiadomienia);
            mod.Entity<Powiadomienia>().HasOne(p => p.Uzytkownik).WithMany(u => u.Powiadomienia);

            mod.Entity<Recenzje>().HasOne(r => r.Ksiazka).WithMany(k => k.Recenzje);
            mod.Entity<Recenzje>().HasOne(r => r.Uzytkownik).WithMany(u => u.Recenzje);

            mod.Entity<HashKsiazkiAutorzy>().HasNoKey();
            base.OnModelCreating(mod);
        }
    }
}
