using FL.Framework.Entity.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EficienciaEnergetica.Models
{
    /// <summary>
    /// Ciudades Model
    /// </summary>
    [Table("Ciudades")]
    public class Ciudades
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CiudadID { get; set; }

        [FontAwesome("fa-font")]
        [Display(Name = "Ciudad")]
        [Required(ErrorMessage = "Nombre es obligatorio")]
        public string Ciudad { get; set; }

        [FontAwesome("fa-font")]
        [Display(Name = "Nivel Térmico")]
        [Required(ErrorMessage = "Nivel Térmico es obligatorio")]
        public int NivelTermicoID { get; set; }
        public virtual NivelTermico NivelTermico { get; set; }

        public virtual ICollection<Empresa> Empresas { get; set; }
        public virtual ICollection<Sede> Sedes { get; set; }
    }
}
