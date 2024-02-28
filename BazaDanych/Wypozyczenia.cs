using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InżynierkaBiblioteka.BazaDanych
{
    public class Wypozyczenia
    {
        [System.ComponentModel.DataAnnotations.Schema.DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity), Key]
        public int idWypozyczenia { get; set; }
        public DateTime DataWypozyczenia { get; set; }
        public DateTime DataDoOddania { get; set; }
        public DateTime? DataAktualnegoOddania { get; set; }


        [ForeignKey("idUzytkownika")]
        public virtual Uzytkownicy Uzytkownicy { get; set; }
        [ForeignKey("idKsiazki")]
        public virtual Ksiazki Ksiazka { get; set; }

        public Wypozyczenia()
        {
            
        }
    }
}
