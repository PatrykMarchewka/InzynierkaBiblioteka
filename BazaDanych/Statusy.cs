using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InżynierkaBiblioteka.BazaDanych
{
    //Status konta, np aktywowane, dezaktywowane, zbanowane itp
    public class Statusy
    {
        [System.ComponentModel.DataAnnotations.Schema.DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity), Key]
        public int idStatusu { get; set; }
        [MaxLength(255)]
        public string Nazwa { get; set; }

        public virtual ICollection<Uzytkownik> Uzytkownicy { get; set; }

        public Statusy()
        {
            
        }
    }
}
