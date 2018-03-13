using System.ComponentModel.DataAnnotations;

namespace EficienciaEnergetica.Models.Reports
{
    public class RptConsumos
    {
        [Display(Name = "Empresa")]
        public int EmpresaID { get; set; }
        public virtual Empresa Empresa { get; set; }

        [Display(Name = "Sede")]
        public int SedeID { get; set; }
        public virtual Sede Sede { get; set; }

        [Display(Name = "Fuente")]
        public int FuenteID { get; set; }
        public virtual FuenteEnergetica Fuente { get; set; }

        [Display(Name = "Medidor")]
        public int DispositivoID { get; set; }
        public virtual Dispositivo Dispositivo { get; set; }

        [Display(Name = "Fecha Inicial")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime FechaInicial { get; set; }

        [Display(Name = "Fecha Final")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime FechaFinal { get; set; }
    }
}