using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InżynierkaBiblioteka.BazaDanych
{
    public class HashKsiazkiAutorzy
    {
        [Key]
        public int idKsiazki { get; set; }
        [Key]
        public int idAutora { get; set; }


        [ForeignKey("idKsiazki")]
        public virtual Ksiazki Ksiazka { get; set; }
        
        [ForeignKey("idAutora")]
        public virtual Autorzy Autor { get; set; }

        public HashKsiazkiAutorzy()
        {
            
        }

    }
}
