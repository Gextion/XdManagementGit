using System;
using System.Linq;
using System.Data;
using System.Data.Entity;
using EficienciaEnergetica.Models;
using EficienciaEnergetica.Models.Reports;
using System.Web.Mvc;
using System.Text;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace EficienciaEnergetica.Controllers
{
    [NWebsec.Mvc.HttpHeaders.Csp.Csp(Enabled = false)]
    [NWebsec.Mvc.HttpHeaders.Csp.CspScriptSrc(Enabled = false)]
    public class ReportsController : Controller
    {
        /// <summary>
        /// DataBase Context
        /// </summary>
        private EEContext db = new EEContext();

        /// <summary>
        /// View Consumos Report
        /// </summary>
        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Consumos()
        {
            SetViewBagListData();

            RptConsumos ViewModel = new RptConsumos()
            {
                FechaInicial = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1),
                FechaFinal = DateTime.Now,
            };

            return View(ViewModel);
        }

        [Authorize(Roles = "BusinessEntity")]
        public ActionResult AhorroAcumulado()
        {
            SetViewBagListData();

            RptConsumos ViewModel = new RptConsumos()
            {
                FechaInicial = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1),
                FechaFinal = DateTime.Now,
            };

            return View(ViewModel);
        }

        /// <summary>
        /// View Consumos Report
        /// </summary>
        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Sedes()
        {
            SetViewBagListData();

            RptSedes ViewModel = new RptSedes() { };

            return View(ViewModel);
        }

        /// <summary>
        /// View Consumos Report
        /// </summary>
        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Empresas()
        {
            SetViewBagListData();

            RptEmpresas ViewModel = new RptEmpresas() { };

            return View(ViewModel);
        }

        /// <summary>   
        /// Build Report Sedes
        /// </summary>
        /// <returns></returns>        
        public ActionResult BuildRptSedes(int? EmpresaID, int? CiudadID)
        {
            try
            {
                StringBuilder RptContent = new StringBuilder();
                RptContent.Append(@"<thead><tr><th align = ""center""><b> Sede </b></th><th align = ""center""><b> Responsable </b></th><th align = ""center""><b> Empresa </b></th><th align = ""center""><b> Ciudad </b></th><th align = ""center""><b> Estrato </b></th><th align = ""center""><b> Dirección </b></th><th align = ""center""><b> Teléfono </b></th><th align = ""center""><b> Celular </b></th><th align = ""center""><b> Correo </b></th></tr></thead>");
                RptContent.Append("<tbody>");

                List<Sede> Sedes = null;

                if (EmpresaID.HasValue && EmpresaID.Value != 999999)
                {
                    if (CiudadID.HasValue && CiudadID.Value != 999999)
                    {
                        Sedes = db.Sedes.Include(s => s.Empresa).Include(s => s.Ciudad).Where(s => s.CiudadID == CiudadID.Value && s.EmpresaID == EmpresaID.Value).ToList();
                    }
                    else
                    {
                        Sedes = db.Sedes.Include(s => s.Empresa).Include(s => s.Ciudad).Where(s => s.EmpresaID == EmpresaID.Value).ToList();
                    }
                }
                else
                {
                    if (CiudadID.HasValue && CiudadID.Value != 999999)
                    {
                        Sedes = db.Sedes.Include(s => s.Empresa).Include(s => s.Ciudad).Where(s => s.CiudadID == CiudadID.Value).ToList();
                    }
                    else
                    {
                        Sedes = db.Sedes.Include(s => s.Empresa).Include(s => s.Ciudad).ToList();
                    }
                }

                var SedesList = Sedes.OrderBy(e => e.NombreSede).ThenBy(e => e.Empresa.Nombre).ThenBy(e => e.Ciudad.Ciudad);

                if (SedesList != null && SedesList.Count() > 0)
                {
                    foreach (var sede in SedesList)
                    {
                        RptContent.Append($"<tr><td>{sede.NombreSede}</td><td>{sede.Responsable}</td><td>{sede.Empresa.Nombre}</td><td>{sede.Ciudad.Ciudad}</td><td>{sede.Estrato}</td><td>{sede.Direccion}</td><td>{sede.Telefono}</td><td>{sede.Celular}</td><td>{sede.Email}</td></tr>");
                    }
                }
                
                RptContent.Append("</tbody>");

                var result = new JsonResult
                {
                    Data = new { RptContent = RptContent.ToString(), Success = true },
                    ContentEncoding = System.Text.Encoding.UTF8,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };

                result.MaxJsonLength = int.MaxValue;

                return result;
            }
            catch
            {
                return new JsonResult
                {
                    Data = new { Message = "Error ejecutando la acción solicitada. Por favor inténtelo de nuevo", Success = false },
                    ContentEncoding = Encoding.UTF8,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }
        
        /// <summary>   
        /// Build Report Empresas
        /// </summary>
        /// <returns></returns>        
        public ActionResult BuildRptEmpresas(int? CiudadID, int? SectorID)
        {
            try
            {
                StringBuilder RptContent = new StringBuilder();
                RptContent.Append(@"<thead><tr><th align = ""center""><b> Empresa </b></th><th align = ""center""><b> Nit </b></th><th align = ""center""><b> Rep. Legal </b></th><th align = ""center""><b> Sector </b></th><th align = ""center""><b> Ciudad </b></th><th align = ""center""><b> Dirección </b></th><th align = ""center""><b> Teléfono </b></th><th align = ""center""><b> Celular </b></th><th align = ""center""><b> Correo </b></th></tr></thead>");
                RptContent.Append("<tbody>");

                List<Empresa> Empresa = null;

                if (CiudadID.HasValue && CiudadID.Value != 999999)
                {
                    if (SectorID.HasValue && SectorID.Value != 999999)
                    {
                        Empresa = db.Empresas.Include(e => e.Ciudad).Include(e => e.SectorEconomico).Where(e => e.CiudadID == CiudadID.Value && e.SectorEconomicoID == SectorID).ToList();
                    }
                    else
                    {
                        Empresa = db.Empresas.Include(e => e.Ciudad).Include(e => e.SectorEconomico).Where(e => e.CiudadID == CiudadID.Value).ToList();
                    }   
                }
                else
                {
                    if (SectorID.HasValue && SectorID.Value != 999999)
                    {
                        Empresa = db.Empresas.Include(e => e.Ciudad).Include(e => e.SectorEconomico).Where(e => e.SectorEconomicoID == SectorID.Value).ToList();
                    }
                    else
                    {
                        Empresa = db.Empresas.Include(e => e.Ciudad).Include(e => e.SectorEconomico).ToList();
                    }   
                }

                var Empresas = Empresa.OrderBy(e => e.Nombre).ThenBy(e => e.Ciudad.Ciudad).ThenBy(e => e.SectorEconomico.SectorEconomico);
                
                if (Empresas != null && Empresas.Count() > 0)
                {
                    foreach (var e in Empresas)
                    {
                        RptContent.Append($"<tr><td>{e.Nombre}</td><td>{e.Nit}</td><td>{e.RepresentanteLegal}</td><td>{e.SectorEconomico.SectorEconomico}</td><td>{e.Ciudad.Ciudad}</td><td>{e.Direccion}</td><td>{e.Telefono}</td><td>{e.Celular}</td><td>{e.Email}</td></tr>");
                    }
                }

                RptContent.Append("</tbody>");

                var result = new JsonResult
                {
                    Data = new { RptContent = RptContent.ToString(), Success = true },
                    ContentEncoding = Encoding.UTF8,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };

                result.MaxJsonLength = int.MaxValue;

                return result;
            }
            catch
            {
                return new JsonResult
                {
                    Data = new { Message = "Error ejecutando la acción solicitada. Por favor inténtelo de nuevo", Success = false },
                    ContentEncoding = Encoding.UTF8,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        /// <summary>   
        /// Build Report
        /// </summary>
        /// <returns></returns>        
        public ActionResult BuildRptConsumos(int? EmpresaID, int? SedeID, int? FuenteID, int? DispositivoID, DateTime? FecIni, DateTime? FecFin)
        {
            try
            {
                StringBuilder RptContent = new StringBuilder();
                RptContent.Append(@"<thead><tr><th align = ""center""><b> Empresa </b></th><th align = ""center""><b> Sede </b></th><th align = ""center""><b> Fuente </b></th><th align = ""center""><b> Medidor </b></th><th align = ""center""><b> Fecha Inicial </b></th><th align = ""center""><b> Fecha Final </b></th><th align = ""center""><b> Consumo </b></th><th align = ""center""><b> Línea Base </b></th><th align = ""center""><b> Valor </b></th><th align = ""center""><b> Valor Unitario </b></th></tr></thead>");
                RptContent.Append("<tbody>");
                
                List<Empresa> Empresas = null;

                if (EmpresaID.HasValue)
                {
                    Empresas = db.Empresas.Include(e => e.Ciudad).Include(e => e.SectorEconomico).Where(e => e.EmpresaID.Equals(EmpresaID.Value)).ToList();
                }
                else
                {
                    Empresas = db.Empresas.Include(e => e.Sedes).Include(e => e.Ciudad).Include(e => e.SectorEconomico).ToList();
                }

                if (SedeID.HasValue && SedeID.Value != 999999)
                {
                    Empresas = (from emp in Empresas
                                join sed in db.Sedes on emp.EmpresaID equals sed.EmpresaID
                                where sed.SedeID.Equals(SedeID.Value)
                                select emp).ToList();
                }

                if (Empresas != null && Empresas.Count > 0)
                {
                    bool EmpresaAdded = false;

                    Empresas.ForEach(e =>
                    {
                        decimal TotSecConsumo = 0;
                        decimal TotSecLinBase = 0;
                        decimal TotSecValor = 0;

                        string Ciudad = "";

                        if (e.Ciudad != null)
                            Ciudad = e.Ciudad.Ciudad;

                        RptContent.Append($"<tr><td colspan=\"10\">{e.Nombre}</td></tr>");
                        
                        foreach (Sede sede in e.Sedes)
                        {
                            var Dispositivos = db.Dispositivos.Include(d => d.Fuente).Where(d => d.SedeID.Equals(sede.SedeID)).ToList();

                            if (DispositivoID.HasValue && DispositivoID.Value != 999999)
                                Dispositivos = (Dispositivos.Where(d => d.DispositivoID.Equals(DispositivoID.Value))).ToList();

                            if (FuenteID.HasValue)
                                Dispositivos = (Dispositivos.Where(d => d.FuenteID.Equals(FuenteID.Value))).ToList();

                            if (Dispositivos != null && Dispositivos.Count > 0)
                            {
                                List<string> FuentesSede = new List<string>();

                                foreach (var dispositivo in Dispositivos)
                                {
                                    if (dispositivo.Fuente != null && !String.IsNullOrEmpty(dispositivo.Fuente.Fuente))
                                    {
                                        if (!FuentesSede.Contains(dispositivo.Fuente.Fuente))
                                            FuentesSede.Add(dispositivo.Fuente.Fuente);
                                    }
                                }

                                if (FuentesSede != null && FuentesSede.Count > 0)
                                {
                                    decimal TotSedConsumo = 0;
                                    decimal TotSedLinBase = 0;
                                    decimal TotSedValor = 0;
                                    bool SedeAdded = false;
                                    
                                    FuentesSede.Sort();
                                    
                                    SedeAdded = false;
                                    
                                    foreach (string nombreFuente in FuentesSede)
                                    {
                                        var DispositivosFuente = db.Dispositivos.Include(d => d.Fuente).Include(d => d.Sede).Include(d => d.Consumos).Where(d => d.Fuente.Fuente.Equals(nombreFuente) && d.SedeID.Equals(sede.SedeID)).ToList();

                                        if (DispositivoID.HasValue && DispositivoID.Value != 999999)
                                            DispositivosFuente = (DispositivosFuente.Where(d => d.DispositivoID.Equals(DispositivoID.Value))).ToList();

                                        if (DispositivosFuente != null)
                                        {
                                            DispositivosFuente.ForEach(d =>
                                            {
                                                decimal TotConsumo = 0;
                                                decimal TotLinBase = 0;
                                                decimal TotValor = 0;

                                                List<Consumo> Consumos = d.Consumos.ToList();

                                                if (FecIni.HasValue)
                                                    Consumos = Consumos.Where(c => c.FechaInicial >= FecIni.Value).ToList();

                                                if (FecFin.HasValue)
                                                    Consumos = Consumos.Where(c => c.FechaInicial <= FecFin.Value).ToList();

                                                if (Consumos != null && Consumos.Count > 0)
                                                {
                                                    if (!SedeAdded)
                                                    {
                                                        RptContent.Append($"<tr><td colspan=\"1\"></td><td colspan=\"9\">{sede.NombreSede}</td></tr>");
                                                        SedeAdded = true;
                                                    }

                                                    RptContent.Append($"<tr><td colspan=\"2\"></td><td colspan=\"8\">{nombreFuente}</td></tr>");

                                                    foreach (Consumo consDis in Consumos)
                                                    {
                                                        TotConsumo += consDis.ConsumoPeriodo;
                                                        TotLinBase += consDis.LineaBase;
                                                        TotValor += consDis.Valor;

                                                        RptContent.Append($"<tr><td colspan=\"3\"></td><td>{d.Nombre}</td><td align=\"right\" >{consDis.FechaInicial.ToShortDateString()}</td><td align=\"right\" >{consDis.FechaFinal.ToShortDateString()}</td><td align=\"right\" >{consDis.ConsumoPeriodo.ToString("N2")}</td><td align=\"right\" >{consDis.LineaBase.ToString("N2")}</td><td align=\"right\" >{consDis.Valor.ToString("C2")}</td><td align=\"right\" >{consDis.ValorUnitario.ToString("C2")}</td></tr>");
                                                    }

                                                    TotSedConsumo += TotConsumo;
                                                    TotSedLinBase += TotLinBase;
                                                    TotSedValor += TotValor;

                                                    RptContent.Append($"<tr><td colspan=\"3\"></td><td colspan=\"3\" ><b>Total {nombreFuente} - {sede.NombreSede} - {d.Nombre}</b></td><td align=\"right\" ><b>{TotConsumo.ToString("N2")}</b></td><td align=\"right\" ><b>{TotLinBase.ToString("N2")}</b></td><td align=\"right\" ><b>{TotValor.ToString("C2")}</b></td><td align=\"right\"><b></b></td></tr>");
                                                    RptContent.Append($"<tr><td colspan=\"10\"></td></tr>");
                                                }
                                            });
                                        }
                                    }

                                    if (SedeAdded)
                                    {
                                        RptContent.Append($"<tr><td></td><td colspan=\"5\"><b>Total {sede.NombreSede}</b></td><td align=\"right\" ><b>{TotSedConsumo.ToString("N2")}</b></td><td align=\"right\" ><b>{TotSedLinBase.ToString("N2")}</b></td><td align=\"right\" ><b>{TotSedValor.ToString("C2")}</b></td><td align=\"right\"><b></b></td></tr>");
                                        RptContent.Append($"<tr><td colspan=\"10\"></td></tr>");
                                    }

                                    TotSecConsumo += TotSedConsumo;
                                    TotSecLinBase += TotSedLinBase;
                                    TotSecValor += TotSedValor;
                                }
                            }
                        }

                        RptContent.Append($"<tr><td colspan=\"6\"><b>Total {e.Nombre}</b></td><td align=\"right\" ><b>{TotSecConsumo.ToString("N2")}</b></td><td align=\"right\" ><b>{TotSecLinBase.ToString("N2")}</b></td><td align=\"right\" ><b>{TotSecValor.ToString("C2")}</b></td><td align=\"right\"><b></b></td></tr>");
                        RptContent.Append($"<tr><td colspan=\"10\"></td></tr>");
                    });
                }
                    
                RptContent.Append("</tbody>");

                var result = new JsonResult
                {
                    Data = new { RptContent = RptContent.ToString(), Success = true },
                    ContentEncoding = System.Text.Encoding.UTF8,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };

                result.MaxJsonLength = int.MaxValue;

                return result;
            }
            catch
            {
                return new JsonResult
                {
                    Data = new { Message = "Error ejecutando la acción solicitada. Por favor inténtelo de nuevo", Success = false },
                    ContentEncoding = Encoding.UTF8,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        public ActionResult BuildRptAhorroAcumulado(int? EmpresaID, int? SedeID, int? FuenteID, int? DispositivoID, DateTime? FecIni, DateTime? FecFin)
        {
            try
            {
                SqlParameter pEmpresa = new SqlParameter("@EmpresaID", EmpresaID == null ? 0: EmpresaID);
                SqlParameter pSede = new SqlParameter("@SedeID", SedeID == 999999 ? 0: SedeID);
                SqlParameter pDispositivo = new SqlParameter("@DispositivoID", DispositivoID == null ? 0 : DispositivoID );
                SqlParameter pFuente = new SqlParameter("@FuenteID", FuenteID == null ? 0 : FuenteID);
                SqlParameter pDesde = new SqlParameter("@Desde", FecIni);
                SqlParameter pHasta = new SqlParameter("@Hasta", FecFin);

                var Ahorro = db.Database.SqlQuery<RptAhorroAcumulado>(
                                    @"spReporteAhorroAcumulado @EmpresaID, @SedeID, @DispositivoID, @FuenteID, @Desde, @Hasta",
                                    pEmpresa, pSede, pDispositivo, pFuente, pDesde, pHasta).ToList();

                StringBuilder RptContent = new StringBuilder();

                RptContent.Append(@"<thead>");
                RptContent.Append(@"<tr>");
                RptContent.Append(@"<th align = ""center""><b> Empresa </b></th>");
                RptContent.Append(@"<th align = ""center""><b> Sede </b></th>");
                RptContent.Append(@"<th align = ""center""><b> Fuente </b></th>");
                RptContent.Append(@"<th align = ""center""><b> Medidor </b></th>");
                RptContent.Append(@"<th align = ""center""><b> Año </b></th>");
                RptContent.Append(@"<th align = ""center""><b> Mes </b></th>");
                RptContent.Append(@"<th align = ""center""><b> Consumo </b></th>");
                RptContent.Append(@"<th align = ""center""><b> Línea Base </b></th>");
                RptContent.Append(@"<th align = ""center""><b> Valor Período</b></th>");
                RptContent.Append(@"<th align = ""center""><b> Vr. Unitario </b></th>");
                RptContent.Append(@"<th align = ""center""><b> Ahorro Período </b></th>");
                RptContent.Append(@"<th align = ""center""><b> Ahorro Acumulado </b></th>");
                RptContent.Append(@"<th align = ""center""><b> Indicador </b></th>");
                RptContent.Append(@"</tr>");
                RptContent.Append(@"</thead>");

                RptContent.Append("<tbody>");

                if (Ahorro != null && Ahorro.Count > 0)
                {
                    string empresa = string.Empty;
                    string sede = string.Empty;

                    decimal TotSecConsumo = 0;
                    decimal TotSecLinBase = 0;
                    decimal TotSecValor = 0;

                    foreach (var item in Ahorro)
                    {
                        if (empresa != item.Empresa)
                        {
                            RptContent.Append($"<tr><td colspan=\"13\"><b> {item.Empresa} </b></td></tr>");

                            empresa = item.Empresa;
                            TotSecConsumo = 0;
                            TotSecLinBase = 0;
                            TotSecValor = 0;
                        }

                        if (sede != item.Sede)
                        {
                            sede = item.Sede;
                            RptContent.Append($"<tr><td></td><td colspan=\"12\"><b> {item.Sede} </b></td></tr>");
                        }

                        //colspan=\"2\"
                        RptContent.Append($"<tr><td colspan=\"2\"></td><td>{item.Fuente}</td>");
                        RptContent.Append($"<td>{item.Dispositivo}</td>");
                        RptContent.Append($"<td>{item.Ano}</td>");
                        RptContent.Append($"<td>{item.Mes}</td>");
                        RptContent.Append($"<td align=\"right\" >{item.ConsumoPeriodo.ToString("N2")}</td>");
                        RptContent.Append($"<td align=\"right\" >{item.LineaBaseFull.ToString("N2")}</td>");
                        RptContent.Append($"<td align=\"right\" >{item.Valor.ToString("C2")}</td>");
                        RptContent.Append($"<td align=\"right\" >{item.ValorUnitario.ToString("C2")}</td>");
                        RptContent.Append($"<td align=\"right\" >{item.AhorroPeriodo.ToString("N2")}</td>");
                        RptContent.Append($"<td align=\"right\" >{item.AhorroPeriodoAcumulado.ToString("N2")}</td>");
                        RptContent.Append($"<td align=\"right\" >{item.Indicador.ToString("P2")}</td></tr>");

                        //RptContent.Append($"<tr><td colspan=\"2\"></td><td colspan=\"3\" ><b>Total {nombreFuente} - {sede.NombreSede} - {d.Nombre}</b></td><td align=\"right\" ><b>{TotConsumo.ToString("N2")}</b></td><td align=\"right\" ><b>{TotLinBase.ToString("N2")}</b></td><td align=\"right\" ><b>{TotValor.ToString("C2")}</b></td><td align=\"right\"><b></b></td></tr>");
                        //RptContent.Append($"<tr><td colspan=\"9\"></td></tr>");

                        //RptContent.Append($"<tr><td colspan=\"5\"><b>Total {sede.NombreSede}</b></td><td align=\"right\" ><b>{TotSedConsumo.ToString("N2")}</b></td><td align=\"right\" ><b>{TotSedLinBase.ToString("N2")}</b></td><td align=\"right\" ><b>{TotSedValor.ToString("C2")}</b></td><td align=\"right\"><b></b></td></tr>");
                        //RptContent.Append($"<tr><td colspan=\"9\"></td></tr>");
                    }
                }

                RptContent.Append("</tbody>");

                var result = new JsonResult
                {
                    Data = new { RptContent = RptContent.ToString(), Success = true },
                    ContentEncoding = System.Text.Encoding.UTF8,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };

                result.MaxJsonLength = int.MaxValue;

                return result;
            }
            //catch (SqlException sqleex)
            //{
            //    return new JsonResult
            //    {
            //        Data = new { Message = "Error ejecutando la acción solicitada. Por favor inténtelo de nuevo ", Success = false },
            //        ContentEncoding = Encoding.UTF8,
            //        JsonRequestBehavior = JsonRequestBehavior.AllowGet
            //    };
            //}
            catch (Exception ex)
            {
                return new JsonResult
                {
                    Data = new { Message = "Error ejecutando la acción solicitada. Por favor inténtelo de nuevo" + ex.Message, Success = false },
                    ContentEncoding = Encoding.UTF8,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        /// <summary>
        /// Set ViewBag List Data
        /// </summary>
        private void SetViewBagListData()
        {
            ViewBag.FuenteID = new SelectList(db.FuenteEnergeticas.OrderBy(s => s.Fuente), "FuenteID", "Fuente");
            ViewBag.CiudadID = new SelectList(db.Ciudades.OrderBy(d => d.Ciudad), "CiudadID", "Ciudad", null);
            ViewBag.SectorID = new SelectList(db.SectoresEconomicos.OrderBy(d => d.SectorEconomico), "SectorEconomicoID", "SectorEconomico", null);

            if (Helpers.ApplicationContext.CurrentUser.IsSuperAdmin)
            {
                SetViewBagForEmpresas(db.Empresas.OrderBy(e => e.Nombre));
                
                ViewBag.SedeID = new SelectList(db.Sedes.OrderBy(s => s.NombreSede), "SedeID", "NombreSede", null);
                ViewBag.DispositivoID = new SelectList(db.Dispositivos.OrderBy(d => d.Nombre), "DispositivoID", "Nombre", null);
            }
            else
            {
                var Empresas = db.Empresas.Where(s => s.EmpresaID.Equals(Helpers.ApplicationContext.CurrentUser.Empresa.EmpresaID)).OrderBy(e => e.Nombre);
                SetViewBagForEmpresas(Empresas);

                ViewBag.SedeID = new SelectList(db.Sedes
                                                     .Where(s => s.EmpresaID.Equals(Helpers.ApplicationContext.CurrentUser.Empresa.EmpresaID))
                                                     .OrderBy(s => s.NombreSede), "SedeID", "NombreSede", null);

                var dispositivos = (from dis in db.Dispositivos
                                    join sede in db.Sedes on dis.SedeID equals sede.SedeID
                                    where sede.EmpresaID == Helpers.ApplicationContext.CurrentUser.Empresa.EmpresaID
                                    select dis);

                ViewBag.DispositivoID = new SelectList(dispositivos, "DispositivoID", "Nombre", null);
            }
        }

        /// <summary>
        /// Set View Bag For Empresas Field
        /// </summary>
        /// <param name="Empresas"></param>
        private void SetViewBagForEmpresas(IOrderedQueryable<Empresa> Empresas)
        {
            var DataEmpresas = (from e in Empresas.ToList()
                                select new
                                {
                                    Nombre = e.Codigo + " - " + e.Nombre,
                                    EmpresaID = e.EmpresaID
                                });

            ViewBag.EmpresaID = new SelectList(DataEmpresas, "EmpresaID", "Nombre", Helpers.ApplicationContext.CurrentUser.Empresa.EmpresaID);
        }

        /// <summary>
        /// Dispose Resources
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}