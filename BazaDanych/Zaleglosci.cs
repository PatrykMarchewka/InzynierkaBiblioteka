using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InżynierkaBiblioteka.BazaDanych
{
    public class Zaleglosci
    {
        [System.ComponentModel.DataAnnotations.Schema.DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity), Key]
        public int idZaleglosci { get; set; }
        [ForeignKey("idUzytkownika")]
        public virtual Uzytkownicy Uzytkownicy { get; set; }
        [ForeignKey("idKsiazki")]
        public virtual Ksiazki? Ksiazka { get; set; }
        public decimal Zaleglosc { get; set; }
        public bool Zaplacono { get; set; }
        [MaxLength(255)]
        public string Komentarz { get; set; } //Dodawac admina w komentarzu jak manualnie


        public Zaleglosci()
        {
           
        }
    }
}
