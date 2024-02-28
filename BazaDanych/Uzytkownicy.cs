using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InżynierkaBiblioteka.BazaDanych
{
    public class Uzytkownicy
    {
        [System.ComponentModel.DataAnnotations.Schema.DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity), Key]
        public int idUzytkownika { get; set; }
        public int? RFID { get; set; }
        public int? RFIDPin { get; set; }
        [MaxLength(255)]
        public string LoginUzytkownika { get; set; }
        [MaxLength(255)]
        public string salt { get; set; } = "BibliotekaInzynieria";
        [MaxLength(255)]
        public string hashHaslo { get; set; }
        [MaxLength(255)]
        public string? email { get; set; }
        [MaxLength(255)]
        public string Imie { get; set; }
        [MaxLength(255)]
        public string Nazwisko { get; set; }
       
        [MaxLength(20)]
        public string? nrTelefonu { get; set; }
       
        public int LiczbaWypozyczonychKsiazek { get; set; } = 0;
        public DateTime DataStworzeniaKonta { get; set; }
        [MaxLength]
        public string? Komentarze;
        public decimal Zaleglosci { get; set; } = 0;
        public DateTime DataOstatniegoLogowania { get; set; }



        [ForeignKey("idPlci")]
        public virtual Plec Plec { get; set; }

        [ForeignKey("idRoli")]
        public virtual Role Rola { get; set; }

        [ForeignKey("idStatusu")]
        public virtual Statusy StatusKonta { get; set; }

        public virtual ICollection<Wypozyczenia> Wypozyczenia { get; set; }
        public virtual ICollection<Powiadomienia> Powiadomienia { get; set; }
        public virtual ICollection<Recenzje> Recenzje { get; set; }
        public virtual ICollection<Ksiazki> WszystkieWypozyczoneKsiazki { get; set; }
        //To Potrzebne?
        public virtual ICollection<Logi> WszystkieLogi { get; set; }

        public HashSet<Ksiazki> WszystkieWypozyczoneKsiazkiHash()
        {
            return WszystkieWypozyczoneKsiazki.OrderBy(b => b.ISBN).DistinctBy(b => b.ISBN).ToHashSet();
        }
        public Uzytkownicy()
        {
            
        }
    }
}
