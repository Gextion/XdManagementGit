using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EficienciaEnergetica.Models
{
    [Table("AccionesAhorroSedes")]
    public class AccionesAhorroSedes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccionSedeID { get; set; }
        
        [Index("IX_InventarioSedeID")]
        public int InventarioSedeID { get; set; }

        [Index("IX_TipoEquipo")]
        [Display(Name = "Tipo Equipo")]
        public int TipoEquipoID { get; set; }
        public virtual TiposEquipos TiposEquipos { get; set; }

        [Index("IX_AccionAhorro")]
        [Display(Name = "Acción Ahorro")]
        public int AccionAhorroID { get; set; }
        public virtual AccionAhorro AccionAhorro { get; set; }
    }
}