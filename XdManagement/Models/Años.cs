using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EficienciaEnergetica.Models
{
    [Table("Años")]
    public class Años
    {
        [Key]
        [Column("AñoID")]
        public int AnioID { get; set; }

        public int Año { get; set; }
    }
}