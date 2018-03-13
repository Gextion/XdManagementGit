using FL.Framework.Entity.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EficienciaEnergetica.Models
{
    [Table("AccionesAhorro")]
    public class AccionAhorro
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccionAhorroID { get; set; }

        [FontAwesome("fa-font")]
        [Display(Name = "Acción Ahorro")]
        [Required(ErrorMessage = "Accion Ahorro es obligatorio")]
        public string DescripcionAccionAhorro { get; set; }

        [FontAwesome("fa-industry")]
        [Display(Name = "Tipo de Equipo")]
        [Required(ErrorMessage = "Tipo de Equipo es obligatorio")]
        public int TipoEquipoID { get; set; }
        public virtual TiposEquipos TipoEquipo { get; set; }

        public virtual ICollection<AccionesAhorroSedes> AccionesAhorroSedes { get; set; }

    }
}