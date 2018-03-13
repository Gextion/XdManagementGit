using EficienciaEnergetica.Models;
using System.Linq;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data;
using System.Data.SqlTypes;
using System.Web.Script.Serialization;

namespace EficienciaEnergetica.Controllers
{
    /// <summary>
    /// CSP Config
    /// https://docs.nwebsec.com/en/stable/nwebsec/Configuring-csp.html
    /// </summary>
    [NWebsec.Mvc.HttpHeaders.Csp.Csp(Enabled = false)]
    [NWebsec.Mvc.HttpHeaders.Csp.CspScriptSrc(Enabled = false)]
    public class DashboardsController : Controller
    {
        private EEContext db = new EEContext();
        Dashboard dashboard = new Dashboard();

        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Dashboard()
        {
            SetViewBagListData();

            return View(new Dashboard());
        }

        [Authorize(Roles = "BusinessEntity")]
        public ActionResult DashboardConsumos(int? anio, int? mes, int? empresa, int? sede, int? dispositivo, int? fuente)
        {
            fuente = 1;

            SqlParameter pAño = new SqlParameter("@Año", anio);
            SqlParameter pMes = new SqlParameter("@Mes", mes);
            SqlParameter pEmpresa = new SqlParameter("@EmpresaID", empresa);
            SqlParameter pSede = new SqlParameter("@SedeID", sede);
            SqlParameter pDispositivo = new SqlParameter("@DispositivoID", dispositivo);
            SqlParameter pFuente = new SqlParameter("@FuenteID", fuente);

            var Consumos = db.Database.SqlQuery<DatosGraficoConsumoVsLineaBase>(
                                @"spDashboard_ConsumoVsLineaBase @Año, @Mes, @EmpresaID, @SedeID, @DispositivoID, @FuenteID", 
                                pAño, pMes, pEmpresa, pSede, pDispositivo, pFuente).ToList();

            var indicadores = new JavaScriptSerializer().Serialize(
                              db.Database.SqlQuery<IndicadoresFuenteEnergetica>(
                                @"spDashboardIndicadores @Año, @Mes, @EmpresaID, @SedeID, @DispositivoID, @FuenteID",
                                new SqlParameter("@Año", anio),
                                new SqlParameter("@Mes", mes),
                                new SqlParameter("@EmpresaID", empresa),
                                new SqlParameter("@SedeID", sede),
                                new SqlParameter("@DispositivoID", dispositivo),
                                new SqlParameter("@FuenteID", fuente)).ToList()
                              );

            var lineData = @"{
                ""labels"":" + $"[\"{ string.Join("\",\"", Consumos.Select(c => c.Mes)) }\"]" + @",
                ""datasets"": [
                    {
                        ""label"": ""Consumo"",
                        ""backgroundColor"": ""rgba(26,179,148,0.5)"",
                        ""borderColor"": ""rgba(26,179,148,0.7)"",
                        ""pointBackgroundColor"": ""rgba(26,179,148,1)"",
                        ""pointBorderColor"": ""#fff"",
                        ""data"": " + $"[{ string.Join(",", Consumos.Select(c => c.ConsumoPeriodo)) }]" + @"
                    },
                    {
                        ""label"": ""Linea Base"",
                        ""backgroundColor"": ""rgba(220,220,220,0.5)"",
                        ""borderColor"": ""rgba(220,220,220,1)"",
                        ""pointBackgroundColor"": ""rgba(220,220,220,1)"",
                        ""pointBorderColor"": ""#fff"",
                        ""data"": " + $"[{ string.Join(",", Consumos.Select(c => c.LineaBase)) }]" + @"
                    }
                ]
            }";

            return new JsonResult { Data = new { datasource = lineData, datosindicadores = indicadores, success = true }, ContentEncoding = System.Text.Encoding.UTF8, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [Authorize(Roles = "BusinessEntity")]
        public ActionResult DashboardAhorro(int? anio, int? mes, int? empresa, int? sede, int? dispositivo, int? fuente)
        {
            fuente = 0;

            SqlParameter pAño = new SqlParameter("@Año", anio);
            SqlParameter pMes = new SqlParameter("@Mes", mes);
            SqlParameter pEmpresa = new SqlParameter("@EmpresaID", empresa);
            SqlParameter pSede = new SqlParameter("@SedeID", sede);
            SqlParameter pDispositivo = new SqlParameter("@DispositivoID", dispositivo);
            SqlParameter pFuente = new SqlParameter("@FuenteID", fuente);

            var Ahorro = db.Database.SqlQuery<DatosGraficoAhorro>(
                                @"spDashboard_AhorroEnergia @Año, @Mes, @EmpresaID, @SedeID, @DispositivoID, @FuenteID",
                                pAño, pMes, pEmpresa, pSede, pDispositivo, pFuente).ToList();

            var lineData = @"{
                ""labels"":" + $"[\"{ string.Join("\",\"", Ahorro.Select(c => c.Mes)) }\"]" + @",
                ""datasets"": [
                    {
                        ""label"": ""Ahorro"",
                        ""backgroundColor"": ""rgba(26,179,148,0.5)"",
                        ""borderColor"": ""rgba(26,179,148,0.7)"",
                        ""pointBackgroundColor"": ""rgba(26,179,148,1)"",
                        ""pointBorderColor"": ""#fff"",
                        ""data"": " + $"[{ string.Join(",", Ahorro.Select(c => c.AhorroPeriodoAcumulado)) }]" + @"
                    },
                    {
                        ""label"": ""Meta de Ahorro"",
                        ""backgroundColor"": ""rgba(220,220,220,0.5)"",
                        ""borderColor"": ""rgba(220,220,220,1)"",
                        ""pointBackgroundColor"": ""rgba(220,220,220,1)"",
                        ""pointBorderColor"": ""#fff"",
                        ""data"": " + $"[{ string.Join(",", Ahorro.Select(c => c.LineaBase)) }]" + @"
                    }
                ]
            }";

            return new JsonResult { Data = new { datasource = lineData, success = true }, ContentEncoding = System.Text.Encoding.UTF8, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        private void SetViewBagListData()
        {
            if (Helpers.ApplicationContext.CurrentUser.IsSuperAdmin)
            {
                ViewBag.EmpresaID = new SelectList(db.Empresas.OrderBy(e => e.Nombre), "EmpresaID", "Nombre", Helpers.ApplicationContext.CurrentUser.EmpresaID);
                ViewBag.SedeID = new SelectList(db.Sedes.OrderBy(s => s.NombreSede), "SedeID", "NombreSede");
                ViewBag.DispositivoID = new SelectList(db.Dispositivos.OrderBy(d => d.Nombre), "DispositivoID", "Nombre");
            }
            else
            {
                ViewBag.EmpresaID = new SelectList(db.Empresas
                                                     .Where(s => s.EmpresaID.Equals(Helpers.ApplicationContext.CurrentUser.Empresa.EmpresaID)).OrderBy(e => e.Nombre), "EmpresaID", "Nombre", Helpers.ApplicationContext.CurrentUser.EmpresaID);
                ViewBag.SedeID = new SelectList(db.Sedes
                                                     .Where(s => s.EmpresaID.Equals(Helpers.ApplicationContext.CurrentUser.Empresa.EmpresaID))
                                                     .OrderBy(s => s.NombreSede), "SedeID", "NombreSede");

                var dispositivos = (from dis in db.Dispositivos 
                                    join sede in db.Sedes on dis.SedeID equals sede.SedeID
                                    where sede.EmpresaID == Helpers.ApplicationContext.CurrentUser.Empresa.EmpresaID
                                    select dis);

                ViewBag.DispositivoID = new SelectList(dispositivos, "DispositivoID", "Nombre");

            }

            ViewBag.AnioID = new SelectList(db.Años,"AnioID", "Año", System.DateTime.Now.Year);
            ViewBag.MesID = new SelectList(db.Meses,"MesID","Mes");
          
        }

    }
}