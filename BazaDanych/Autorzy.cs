using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InżynierkaBiblioteka.BazaDanych
{
    public class Autorzy
    {
        [System.ComponentModel.DataAnnotations.Schema.DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity), Key]
        public int idAutora { get; set; }
        [MaxLength(255)]
        public string ImieAutora { get; set; }
        [MaxLength(255)]
        public string NazwiskoAutora { get; set; }

        public virtual ICollection<HashKsiazkiAutorzy> Hashe { get; set; }


        public Autorzy()
        {
                
        }
    }
}
