using InżynierkaBiblioteka.BazaDanych;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;


namespace InżynierkaBiblioteka
{
    public class MyDbContext : DbContext
    {
        public DbSet<Uzytkownicy> Uzytkownicy { get; set; }
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
        public DbSet<Logi> Logi { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string serwer = "InzynieriaBiblioteka.mssql.somee.com";
            string nazwaBazy = "InzynieriaBiblioteka";
            string Login = "patryk4610_SQLLogin_1";
            string Haslo = "myy4mgwo5k";
            //string serwer = "sql.bsite.net\\MSSQL2016";
            //string nazwaBazy = "patryk4610_InzynieriaBiblioteka";
            //string Login = "patryk4610_InzynieriaBiblioteka"; //Nazwa bazy danych jest taka sama jak loginn
            //string Haslo = "Logowanie";


            optionsBuilder.UseLazyLoadingProxies().UseSqlServer($"Server={serwer};Database={nazwaBazy};User Id={Login};Password={Haslo};Encrypt=False");
        }

        protected override void OnModelCreating(ModelBuilder mod)
        {
            mod.Entity<Uzytkownicy>().HasOne(u => u.Plec).WithMany(p => p.Uzytkownicy);
            mod.Entity<Uzytkownicy>().HasOne(u => u.Rola).WithMany(r => r.Uzytkownicy);
            mod.Entity<Uzytkownicy>().HasOne(u => u.StatusKonta).WithMany(s => s.Uzytkownicy);

            mod.Entity<HashKsiazkiAutorzy>().HasOne(h => h.Ksiazka).WithMany(k => k.Hashe);
            mod.Entity<HashKsiazkiAutorzy>().HasOne(h => h.Autor).WithMany(a => a.Hashe);
            //mod.Entity<HashKsiazkiAutorzy>().HasNoKey();
            mod.Entity<HashKsiazkiAutorzy>().HasKey(h => new { h.idKsiazki, h.idAutora });

            mod.Entity<Wypozyczenia>().HasOne(w => w.Uzytkownicy).WithMany(u => u.Wypozyczenia);
            mod.Entity<Wypozyczenia>().HasOne(w => w.Ksiazka).WithMany(k => k.Wypozyczenia);

            mod.Entity<Powiadomienia>().HasOne(p => p.Ksiazka).WithMany(k => k.Powiadomienia);
            mod.Entity<Powiadomienia>().HasOne(p => p.Uzytkownicy).WithMany(u => u.Powiadomienia);

            mod.Entity<Recenzje>().HasOne(r => r.Ksiazka).WithMany(k => k.Recenzje);
            mod.Entity<Recenzje>().HasOne(r => r.Uzytkownicy).WithMany(u => u.Recenzje);




            base.OnModelCreating(mod);
        }
    }
}
