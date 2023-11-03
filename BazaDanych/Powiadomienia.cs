using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InżynierkaBiblioteka.BazaDanych
{
    internal class Powiadomienia
    {
        [System.ComponentModel.DataAnnotations.Schema.DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        [Key]
        public int idPowiadomienia { get; set; }
        public DateTime KiedyStworzono { get; set; }
        public DateTime? KiedyWyslanoMail { get; set; }

        [ForeignKey("idKsiazki")]
        public Ksiazki Ksiazka { get; set; }
        [ForeignKey("idUzytkownika")]
        public Uzytkownik Uzytkownik { get; set; }
    }
}
