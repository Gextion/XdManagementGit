using FL.Framework.Entity.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EficienciaEnergetica.Models
{
    /// <summary>
    /// Equipos Model
    /// </summary>
    public class Equipos
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EquipoID { get; set; }

        [FontAwesome("fa-cog")]
        [Display(Name = "Descripción de Equipo")]
        [Required(ErrorMessage = "Descripción es obligatorio")]
        public string DescripcionEquipo { get; set; }

        [FontAwesome("fa-industry")]
        [Display(Name = "Tipo de Equipo")]
        [Required(ErrorMessage = "Tipo de Equipo es obligatorio")]
        public int TipoEquipoID { get; set; }
        public virtual TiposEquipos TipoEquipo { get; set; }

        [FontAwesome("fa-font")]
        [Required(ErrorMessage = "Consumo es obligatorio")]
        [Display(Name = "Consumo KWH")]
        [RegularExpression(@"\d+(\.\d{1,3})?", ErrorMessage = "El valor no es correcto. Debe contener máximo 3 decimales.")]
        [DisplayFormat(DataFormatString = "{0:N3}", ApplyFormatInEditMode = true)]
        public decimal Consumo { get; set; }

        [FontAwesome("fa-bolt")]
        [Display(Name = "Watts")]
        [Required(ErrorMessage = "Los Watt's son requeridos")]
        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "El valor no es correcto. Debe contener máximo 2 decimales.")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal Watt { get; set; }
    }
}
