using FL.Framework.Entity.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EficienciaEnergetica.Models
{
    [Table("InventarioEquipos")]
    public class InventarioEquipos
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InventarioEquipoID { get; set; }
        
        [Index("IX_InventarioSedeID")]
        public int InventarioSedeID { get; set; }

        [Index("IX_Equipo")]
        [Display(Name = "Equipo")]
        public int EquipoID { get; set; }
        public virtual Equipos Equipos { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria")]
        [Display(Name = "Cantidad")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public Int16 Cantidad { get; set; }

        [Required(ErrorMessage = "El consumo de la semana es obligatorio")]
        [Display(Name = "Consumo Semana")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal ConsumoSemana { get; set; }

        [Required(ErrorMessage = "El consumo del sábado es obligatorio")]
        [Display(Name = "Consumo Sábado")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal ConsumoSabado { get; set; }

        [Required(ErrorMessage = "El consumo del domingo es obligatorio")]
        [Display(Name = "Consumo Domingo")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal ConsumoDomingo { get; set; }

        [NotMapped]
        [Display(Name = "Consumo Mes")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal ConsumoMes { get; set; }

        public virtual Inventario Inventario { get; set; }
    }
}