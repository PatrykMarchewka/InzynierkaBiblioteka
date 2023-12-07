using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InżynierkaBiblioteka.BazaDanych
{
    public class Recenzje
    {
        [System.ComponentModel.DataAnnotations.Schema.DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity), Key]
        public int IDRecenzji { get; set; }
        public DateTime DataWystawienia { get; set; }
        public int Ocena { get; set; }
        [MaxLength]
        public string? TekstRecenzji { get; set; }
        public bool Ukryta { get; set; }


        [ForeignKey("idUzytkownika")]
        public virtual Uzytkownik Uzytkownik { get; set; }
        [ForeignKey("idKsiazki")]
        public virtual Ksiazki Ksiazka { get; set; }

        public virtual ICollection<Reporty> Reporty { get; set; }

        public Recenzje()
        {
            
        }
    }
}
