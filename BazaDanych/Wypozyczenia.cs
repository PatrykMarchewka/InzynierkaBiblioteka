using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InżynierkaBiblioteka.BazaDanych
{
    internal class Wypozyczenia
    {
        [System.ComponentModel.DataAnnotations.Schema.DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity), Key]
        public int id { get; set; }
        public int idUzytkownika { get; set; }
        public string ISBNKsiazki { get; set; }
        public DateTime DataWypozyczenia { get; set; }
        public DateTime DataDoOddania { get; set; }
        public DateTime? DataAktualnegoOddania { get; set; }


        [ForeignKey("id")]
        public Uzytkownik Uzytkownik { get; set; }
        [ForeignKey("ISBN")]
        public Ksiazki Ksiazka { get; set; }
    }
}
