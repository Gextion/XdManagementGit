using FL.Framework.Entity.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EficienciaEnergetica.Models
{
    [Table("FuentesEnergeticas")]
    public class FuenteEnergetica
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FuenteID { get; set; }

        [FontAwesome("fa-align-left")]
        [Required(ErrorMessage = "Nombre de la Fuente Energética es obligatorio")]
        [Display(Name = "Fuente Energética")]
        public string Fuente { get; set; }

        [FontAwesome("fa-font")]
        [Required(ErrorMessage = "Unidad Medida es obligatoria")]
        [Display(Name = "Unidad Medida")]
        public string UnidadMedida { get; set; }

        public virtual ICollection<Dispositivo> Dispositivos { get; set; }
    }

}