using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EficienciaEnergetica.Models
{
    public class DatosGraficoConsumoVsLineaBase
    {
        public int Ano { get; set; }
        public string Fuente { get; set; }
        public int MesID { get; set; }
        public string Mes { get; set; }
        public decimal LineaBase { get; set; }
        public decimal ConsumoPeriodo { get; set; }
        public decimal Valor { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal Promedio { get; set; }
    }

    public class DatosGraficoAhorro
    {
        public int Ano { get; set; }
        public string Fuente { get; set; }
        public int MesID { get; set; }
        public string Mes { get; set; }
        public decimal LineaBase { get; set; }
        public decimal AhorroPeriodo { get; set; }
        public decimal AhorroPeriodoAcumulado { get; set; }
    }

    public class IndicadoresFuenteEnergetica
    {
        public string ConsumoMesS { get { return ConsumoMes.ToString("N2") + " " + UnidadMedida; } }
        public string ConsumoAnualS { get { return ConsumoAnual.ToString("N2"); } }
        public string LineaBaseMesS { get { return LineaBaseMes.ToString("N2"); } }
        public string LineaBaseAnualS { get { return LineaBaseAnual.ToString("N2"); } }
        public string AhorroMesS { get { return AhorroMes.ToString("N2") + " " + UnidadMedida; } }
        public string AhorroAnualS { get { return AhorroAnual.ToString("N2") + " " + UnidadMedida; } }
        public string PorcentajeAhorroS { get { return PorcentajeAhorro.ToString("P2");  } }

        public string FuenteEnergetica { get; set; }
        public string UnidadMedida { get; set; }
        public decimal ConsumoMes { get; set; }
        public decimal ConsumoAnual { get; set; }
        public decimal LineaBaseMes { get; set; }
        public decimal LineaBaseAnual { get; set; }
        public double AhorroMes { get; set; }
        public decimal AhorroAnual { get; set; }
        public decimal PorcentajeAhorro { get; set; }
    }
}