using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FL.Framework.Entity.Attributes;

namespace EficienciaEnergetica.Models
{
    [Table("PeriodosFacturacion")]
    public class PeriodoFacturacion
    {
        [Key]   
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PeriodoFacturacionID { get; set; }

        [FontAwesome("fa-font")]
        [Required(ErrorMessage = "Período de Facturación es obligatorio")]
        [Display(Name = "Período de Facturación")]
        public string Periodo { get; set; }

        [FontAwesome("fa-calendar")]
        [Required(ErrorMessage = "Dias del Período es obligatorio")]
        [Display(Name = "Dias")]
        public short Dias { get; set; }

        public virtual ICollection<Dispositivo> Dispositivos { get; set; }
    }
}