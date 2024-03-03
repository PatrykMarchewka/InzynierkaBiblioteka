using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InżynierkaBiblioteka.BazaDanych
{
    public class Logi
    {
        [System.ComponentModel.DataAnnotations.Schema.DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity),System.ComponentModel.DataAnnotations.Key]
        public int idLoga { get; set; }
        [ForeignKey("idUzytkownika")]
        public virtual Uzytkownicy? Uzytkownicy { get; set; }
        //Waznosc
        //1 -> Zwykly log
        //10 -> Blad ktory zostal wychwycony
        //100 -> Blad ktory spowodowal wylaczenie programu
        public int Waznosc { get; set; }
        [MaxLength(255)]
        public string TrescWiadomosci { get; set; }
        public DateTime DataWystapienia { get; set; }

        public Logi()
        {
            
        }





    }
}
