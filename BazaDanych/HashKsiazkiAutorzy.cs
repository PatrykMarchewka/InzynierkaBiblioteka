using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InżynierkaBiblioteka.BazaDanych
{
    internal class HashKsiazkiAutorzy
    {
        [System.ComponentModel.DataAnnotations.Schema.DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity), Key]
        public int id { get; set; }
        public string ISBN { get; set; }
        public int idAutora { get; set; }


        [ForeignKey("ISBN")]
        public Ksiazki Ksiazka { get; set; }
        [ForeignKey("idAutora")]
        public Autorzy Autor { get; set; }

    }
}
