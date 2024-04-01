using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InżynierkaBiblioteka.BazaDanych
{
    public class Ksiazki
    {
        [System.ComponentModel.DataAnnotations.Schema.DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity), System.ComponentModel.DataAnnotations.Key]
        public int idKsiazki { get; set; } //ID Poniewaz EF Core ma problem z kluczem glownym jako String
        [MaxLength(13)]
        public string ISBN { get; set; } 
        [MaxLength(255)]
        public string TytulKsiazki { get; set; }
        [ForeignKey("idGatunku")]
        public virtual GatunkiKsiazek GatunekKsiazki { get; set; }
        public int RokPublikacjiKsiazki { get; set; }
        [ForeignKey("idJezyka")]
        public virtual Jezyki JezykKsiazki { get; set; }
        public int IloscStron { get; set; }
        public int DostepnoscKsiazki { get; set; }
        public int LiczbaOczekujacych { get; set; }
        public bool DoWypozyczenia { get; set; }
        public int IloscWypozyczen30Dni { get; set; }

        public virtual ICollection<HashKsiazkiAutorzy> Hashe { get; set; } = new List<HashKsiazkiAutorzy>();
        public virtual ICollection<Wypozyczenia> Wypozyczenia { get; set; } = new List<Wypozyczenia>();
        public virtual ICollection<Powiadomienia> Powiadomienia { get; set; } = new List<Powiadomienia>();
        public virtual ICollection<Recenzje> Recenzje { get; set; } = new List<Recenzje>();
        public virtual ICollection<Zaleglosci> Zaleglosci { get; set; } = new List<Zaleglosci>();


        public Ksiazki()
        {
            
        }

    }
}
