using FL.Framework.Entity.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EficienciaEnergetica.Models
{
    [Table("SectoresEconomicos")]
    public class SectoresEconomicos
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SectorEconomicoID { get; set; }

        [FontAwesome("fa-font")]
        [Display(Name = "Sector Económico")]
        [Required(ErrorMessage = "Nombre es obligatorio")]
        public string SectorEconomico { get; set; }

        public virtual ICollection<Empresa> Empresas { get; set; }
    }
}
