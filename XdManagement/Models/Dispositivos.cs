using FL.Framework.Entity.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EficienciaEnergetica.Models
{
    [Table("Dispositivos")]
    public class Dispositivo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DispositivoID { get; set; }

        [FontAwesome("fa-font")]
        [Required(ErrorMessage = "Descripción es obligatoria")]
        [MaxLength(40, ErrorMessage = "Descripción no puede tener más de 80 caracteres"),
         MinLength(3, ErrorMessage = "Descripción no puede tener menos de 3 caracteres")]
        [Display(Name = "Medidor")]
        public string Nombre { get; set; }

        [FontAwesome("fa-home")]
        [Index("IX_Sede")]
        [Display(Name = "Sede")]
        public int SedeID { get; set; }
        public virtual Sede Sede { get; set; }

        [FontAwesome("fa-line-chart")]
        [Display(Name = "Línea Base")]
        [Required(ErrorMessage = "Línea base es obligatoria")]
        public decimal LineaBase { get; set; }

        [FontAwesome("fa-battery-three-quarters")]
        [Index("IX_Fuente")]
        [Display(Name = "Fuente Energética")]
        public int FuenteID { get; set; }
        public virtual FuenteEnergetica Fuente { get; set; }

        [FontAwesome("fa-calendar")]
        [Index("IX_PeriodoFacturacion")]
        [Display(Name = "Facturación")]
        public int PeriodoFacturacionID { get; set; }
        public virtual PeriodoFacturacion PeriodosFacturacion { get; set; }

        public ICollection<Consumo> Consumos { get; set; }

    }
}

