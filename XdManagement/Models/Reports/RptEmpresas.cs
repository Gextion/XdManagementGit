
using System.ComponentModel.DataAnnotations;

namespace EficienciaEnergetica.Models.Reports
{
    /// <summary>
    /// Report Empresas
    /// </summary>
    public class RptEmpresas
    {
        [Display(Name = "Ciudad")]
        public int CiudadID { get; set; }
        public virtual Ciudades Ciudad { get; set; }

        [Display(Name = "Sector")]
        public int SectorID { get; set; }
        public virtual SectoresEconomicos Sector { get; set; }
    }
}