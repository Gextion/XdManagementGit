using FL.Framework.Entity.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EficienciaEnergetica.Models
{
    /// <summary>
    /// Ciudades Model
    /// </summary>
    [Table("TiposEquipos")]
    public class TiposEquipos
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TipoEquipoID { get; set; }

        [FontAwesome("fa-battery-full")]
        [Required(ErrorMessage = "Tipo de Equipo es obligatorio")]
        [Display(Name = "Tipo de Equipo")]
        public string NombreTipoEquipo { get; set; }

        public virtual ICollection<Equipos> Equipos { get; set; }
        public virtual ICollection<AccionAhorro> AccionesAhorro { get; set; }

    }
}
