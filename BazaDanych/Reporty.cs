using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InżynierkaBiblioteka.BazaDanych
{
    internal class Reporty
    {
        [System.ComponentModel.DataAnnotations.Schema.DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        [Key]
        public int IDReportu { get; set; }
        [ForeignKey("id")]
        public Uzytkownik idUzytkownika { get; set; }
        [ForeignKey("IDRecenzji")]
        public Recenzje idRecenzji { get; set; }
        [MaxLength]
        public string TrescRaportu { get; set; }
        public bool StatusRaportu { get; set; }

    }
}
