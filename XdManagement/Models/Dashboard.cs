using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EficienciaEnergetica.Models
{
    public class Dashboard
    {
        [Display(Name = "Empresa")]
        public int EmpresaID { get; set; }
        public virtual Empresa Empresa { get; set; }

        [Display(Name = "Sede")]
        public int SedeID { get; set; }
        public virtual Sede Sede { get; set; }

        [Display(Name = "Medidor")]
        public int DispositivoID { get; set; }
        public virtual Dispositivo Dispositivo { get; set; }

        [Display(Name = "Año")]
        [Column("AñoID")]
        public int AnioID { get; set; }
        public virtual Años Años { get; set; }

        [Display(Name = "Mes")]
        public int MesID { get; set; }
        public virtual Meses Meses { get; set; }

        public IndicadoresFuenteEnergetica Indicadores { get; set; }

    }
}