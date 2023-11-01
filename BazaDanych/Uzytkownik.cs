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
    internal class Uzytkownik
    {
        [System.ComponentModel.DataAnnotations.Schema.DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity), Key]
        public int id { get; set; }
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
        public int idPlci { get; set; }
        public int idRoli { get; set; }
        public int idStatusu { get; set; }

        [MaxLength]
        public string? Komentarze;



        [ForeignKey("id")]
        public Plec Plec { get; set; }

        [ForeignKey("id")]
        public Role Rola { get; set; }

        [ForeignKey("id")]
        public Statusy StatusKonta { get; set; }

        internal ICollection<Wypozyczenia> Wypozyczenia;
        internal ICollection<Powiadomienia> Powiadomienia;
        internal ICollection<Recenzje> Recenzje;
    }
}
