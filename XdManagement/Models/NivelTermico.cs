using FL.Framework.Entity.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EficienciaEnergetica.Models
{
    [Table("NivelesTermicos")]
    public class NivelTermico
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NivelTermicoID { get; set; }

        [VisibleInListView(true)]
        [FontAwesome("fa-font")]
        [Display(Name = "Nivel Térmico")]
        [Required(ErrorMessage = "Nivel Térmico es obligatorio")]
        public string NombreNivelTermico { get; set; }

        public virtual ICollection<Ciudades> Ciudades { get; set; }

    }
}