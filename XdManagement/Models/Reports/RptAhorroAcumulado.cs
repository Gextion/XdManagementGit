using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EficienciaEnergetica.Models.Reports
{
    public class RptAhorroAcumulado
    {
        public string Empresa { get; set; }
        public string Sede { get; set; }
        public string Fuente { get; set; }
        public string Dispositivo { get; set; }
        public int Ano { get; set; }
        public int MesID { get; set; }
        public string Mes { get; set; }

        //[Display(Name = "Fecha Inicial")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        //public System.DateTime FechaInicial { get; set; }

        //[Display(Name = "Fecha Final")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        //public System.DateTime FechaFinal { get; set; }

        public decimal LineaBase { get; set; }
        public decimal LineaBaseFull { get; set; }
        public decimal ConsumoPeriodo { get; set; }
        public decimal Diferencia { get; set; }
        public decimal Valor { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal AhorroPeriodo { get; set; }
        public decimal AhorroPeriodoAcumulado { get; set; }
        public decimal Indicador { get; set; }

    }
}