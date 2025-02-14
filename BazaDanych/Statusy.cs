using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InżynierkaBiblioteka.BazaDanych
{
    //Status konta, np aktywowane, dezaktywowane, zbanowane itp
    //Status 1 = Nowy Uzytkownik
    //Status 2 = Zbanowane konto
    //Status 3 = Konto nieaktywne (brak logowania przez rok)
    public class Statusy
    {
        [System.ComponentModel.DataAnnotations.Schema.DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity), Key]
        public int idStatusu { get; set; }
        [MaxLength(255)]
        public string Nazwa { get; set; }

        public virtual ICollection<Uzytkownicy> Uzytkownicy { get; set; }

        public Statusy()
        {
            
        }
    }
}
