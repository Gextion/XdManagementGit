using System.ComponentModel.DataAnnotations;

namespace EficienciaEnergetica.Models.Reports
{
    public class RptSedes
    {
        [Display(Name = "Empresa")]
        public int EmpresaID { get; set; }
        public virtual Empresa Empresa { get; set; }

        [Display(Name = "Ciudad")]
        public int CiudadID { get; set; }
        public virtual Ciudades Ciudad { get; set; }
    }
}