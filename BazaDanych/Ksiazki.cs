using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InżynierkaBiblioteka.BazaDanych
{
    internal class Ksiazki
    {
        [System.ComponentModel.DataAnnotations.Schema.DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity), System.ComponentModel.DataAnnotations.Key]
        public int idKsiazki { get; set; } //ID Poniewaz EF Core ma problem z kluczem glownym jako String
        [MaxLength(13)]
        public string ISBN { get; set; } 
        [MaxLength(255)]
        public string TytulKsiazki { get; set; }
        [ForeignKey("idGatunku")]
        public GatunkiKsiazek GatunekKsiazki { get; set; }
        public int RokPublikacjiKsiazki { get; set; }
        [ForeignKey("idJezyka")]
        public Jezyki JezykKsiazki { get; set; }
        public int IloscStron { get; set; }
        public int DostepnoscKsiazki { get; set; }
        public int LiczbaOczekujacych { get; set; }
        public bool DoWypozyczenia { get; set; }
        public int IloscWypozyczen30Dni { get; set; }

        internal ICollection<HashKsiazkiAutorzy> Hashe;
        internal ICollection<Wypozyczenia> Wypozyczenia;
        internal ICollection<Powiadomienia> Powiadomienia;
        internal ICollection<Recenzje> Recenzje;
    }
}
