using FL.Framework.Entity.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EficienciaEnergetica.Models
{   
    /// <summary>
    /// Ciudades Model
    /// </summary>
    [Table("Asesores")]
    public class Asesor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AsesorID { get; set; }

        [FontAwesome("fa-barcode")]
        [Display(Name = "Código")]
        [Required(ErrorMessage = "Código es obligatorio")]
        public string Codigo { get; set; }

        [FontAwesome("fa-font")]
        [Display(Name = "Nombre Asesor")]
        [Required(ErrorMessage = "Nombre Asesor es obligatorio")]
        public string NombreAsesor { get; set; }

        //public virtual ICollection<Inventario> Inventario { get; set; }
    }
}
