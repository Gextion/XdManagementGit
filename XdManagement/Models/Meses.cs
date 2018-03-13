using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EficienciaEnergetica.Models
{
    [Table("Meses")]
    public class Meses
    {
        [Key]
        public int MesID { get; set; }

        public string Mes { get; set; }

    }
}