using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InżynierkaBiblioteka.BazaDanych
{
    public class Role
    {
        [System.ComponentModel.DataAnnotations.Schema.DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity), Key]
        public int idRoli { get; set; }
        [MaxLength(255)]
        public string Nazwa { get; set; }

        public virtual ICollection<Uzytkownicy> Uzytkownicy { get; set; }

        public Role()
        {
            
        }
    }
}
