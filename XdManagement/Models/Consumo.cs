using FL.Framework.Entity.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EficienciaEnergetica.Models
{
    [Table("Consumos")]
    public class Consumo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ConsumoID { get; set; }

        [NotMapped]
        [FontAwesome("fa-university")]
        [Display(Name = "Sede")]
        public int SedeID { get; set; }

        [NotMapped]
        [FontAwesome("fa-calendar")]
        [Display(Name = "Facturación")]
        public string PeriodoFacturacion { get; set; }

        [NotMapped]
        [FontAwesome("fa-calendar")]
        [Display(Name = "Unidad Medidad")]
        public string UnidadMedida { get; set; }

        [NotMapped]
        [FontAwesome("fa-align-left")]
        [Display(Name = "Fuente Energética")]
        public string FuenteEnergetica { get; set; }

        [NotMapped]
        public virtual Sede Sede { get; set; }

        [FontAwesome("fa-tablet")]
        [Index("IX_Dispositivo")]
        [Display(Name = "Medidor")]
        public int DispositivoID { get; set; }
        public virtual Dispositivo Dispositivo { get; set; }

        [FontAwesome("fa-calendar")]
        [Required(ErrorMessage = "Fecha Inicial es obligatoria")]
        [Display(Name = "Fecha Inicial")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FechaInicial { get; set; }

        [FontAwesome("fa-calendar")]
        [Required(ErrorMessage = "Fecha Final es obligatoria")]
        [Display(Name = "Fecha Final")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FechaFinal { get; set; }

        [FontAwesome("fa-pie-chart")]
        [Required(ErrorMessage = "La línea base es obligatoria")]
        [Display(Name = "Linea Base")]
        //[DisplayFormat(DataFormatString = "{0:#.##}", ApplyFormatInEditMode = true)]
        [DisplayFormat(DataFormatString = "{0:N}")]
        public decimal LineaBase { get; set; }

        [FontAwesome("fa-pie-chart")]
        [Required(ErrorMessage = "El Consumo es obligatorio")]
        [Display(Name = "Consumo")]
        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "El consumo no es correcto. Debe contener máximo 2 decimales.")]
        //[DisplayFormat(DataFormatString = "{0:#.##}", ApplyFormatInEditMode = true)]
        [DisplayFormat(DataFormatString = "{0:N}")]
        public decimal ConsumoPeriodo { get; set; }

        [FontAwesome("fa-money")]
        [Required(ErrorMessage = "El valor pagado es obligatorio")]
        [Display(Name = "Valor")]
        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "El valor no es correcto. Debe contener máximo 2 decimales.")]
        //[DisplayFormat(DataFormatString = "{0:#.##}", ApplyFormatInEditMode = true)]
        [DisplayFormat(DataFormatString = "{0:N}")]
        public decimal Valor { get; set; }

        [FontAwesome("fa-money")]
        [Display(Name = "Valor Unitario")]
        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "El valor unitario no es correcto. Debe contener máximo 2 decimales.")]
        //[DisplayFormat(DataFormatString = "{0:#.##}", ApplyFormatInEditMode = true)]
        [DisplayFormat(DataFormatString = "{0:N}")]
        public decimal ValorUnitario { get; set; }

    }
}