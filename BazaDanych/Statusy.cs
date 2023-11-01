using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InżynierkaBiblioteka.BazaDanych
{
    //Status konta, np aktywowane, dezaktywowane, zbanowane itp
    internal class Statusy
    {
        [System.ComponentModel.DataAnnotations.Schema.DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity), Key]
        public int id { get; set; }
        [MaxLength(255)]
        public string Nazwa { get; set; }

        internal ICollection<Uzytkownik> Uzytkownicy;
    }
}
