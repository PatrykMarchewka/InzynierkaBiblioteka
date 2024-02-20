using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InżynierkaBiblioteka.BazaDanych
{
    public class Reporty
    {
        [System.ComponentModel.DataAnnotations.Schema.DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        [Key]
        public int IDReportu { get; set; }
        [ForeignKey("IDRecenzji")]
        public virtual Recenzje Recenzje { get; set; }
        [MaxLength]
        public string TrescRaportu { get; set; }
        public bool StatusRaportu { get; set; }
        [ForeignKey("idUzytkownika")]
        public virtual Uzytkownik Reportujacy { get; set; }

        public Reporty()
        {
            
        }

    }
}
