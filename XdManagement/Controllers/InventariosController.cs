using EficienciaEnergetica.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace EficienciaEnergetica.Controllers
{
    [NWebsec.Mvc.HttpHeaders.Csp.Csp(Enabled = false)]
    [NWebsec.Mvc.HttpHeaders.Csp.CspScriptSrc(Enabled = false)]
    public class InventariosController : Controller
    {
        private EEContext db = new EEContext();

        [Authorize(Roles = "AccessAll,UserManagement")]
        public ActionResult Index()
        {
            if (Helpers.ApplicationContext.CurrentUser.IsSuperAdmin)
            {
                ViewBag.EmpresaID = new SelectList(db.Empresas.OrderBy(e => e.Nombre), "EmpresaID", "Nombre", Helpers.ApplicationContext.CurrentUser.EmpresaID);

                var inventarios = (from i in db.Inventario.Include(c => c.Asesor).Include(i => i.Sede)
                                   join d in db.Ciudades.Include(d => d.NivelTermico) on i.Sede.CiudadID equals d.CiudadID
                                   select new InventarioIndexModel {
                                       InventarioSedeID = i.InventarioSedeID,
                                       NombreEmpresa = i.Empresa.Nombre,
                                       NombreSede = i.Sede.NombreSede,
                                       Dimension = i.MetrosCuadrados,
                                       NombreCiudad = d.Ciudad,
                                       Fecha = i.Fecha,
                                       NombreNivelTermico = d.NivelTermico.NombreNivelTermico,
                                       NombreAsesor = i.Asesor.NombreAsesor
                                   });

                return View("Index", inventarios);
            }
            else
            {
                var inventarios = (from i in db.Inventario.Include(c => c.Asesor).Include(i => i.Sede)
                                   join d in db.Ciudades.Include(d => d.NivelTermico) on i.Sede.CiudadID equals d.CiudadID
                                   where i.EmpresaID == Helpers.ApplicationContext.CurrentUser.Empresa.EmpresaID
                                   select new InventarioIndexModel
                                   {
                                       InventarioSedeID = i.InventarioSedeID,
                                       NombreEmpresa = i.Empresa.Nombre,
                                       NombreSede = i.Sede.NombreSede,
                                       Dimension = i.MetrosCuadrados,
                                       NombreCiudad = d.Ciudad,
                                       Fecha = i.Fecha,
                                       NombreNivelTermico = d.NivelTermico.NombreNivelTermico,
                                       NombreAsesor = i.Asesor.NombreAsesor
                                   });

                return View("Index", inventarios);
            }
        }

        public ActionResult GenerarCSV(int id)
        {
            var inventario = GetModelToPrint(id);

            if (inventario == null)
            {
                return new JsonResult
                {
                    Data = new { Message = "No es posible recuperar el inventario seleccionado. Por favor inténtelo de nuevo", Success = false },
                    ContentEncoding = Encoding.UTF8,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }

            try
            {
                string csv =  Server.MapPath(@"~/Uploads/CSV/InventarioEquipos-" + inventario.NombreSede + ".csv");

                StreamWriter sw = new StreamWriter(csv, false, Encoding.UTF8);
                string linea = "Empresa" + "; " +
                                "Codigo" + "; " +
                                "Sede" + "; " +
                                "Ciudad" + "; " +
                                "Nivel Termico" + "; " +
                                "Nombre Asesor" + "; " +
                                "Codigo Asesor" + "; " +
                                "Fecha" + "; " +
                                "ValorKWH" + "; " +
                                "Dimensión" + "; " +
                                "Tipo de Equipo" + "; " +
                                "Descripción Equipo" + "; " +
                                "Consumo" + "; " +
                                "Watts" + "; " +
                                "Consumo Semana" + "; " +
                                "Consumo Sábado" + "; " +
                                "Consumo Domingo" + "; " +
                                "Consumo Mes";

                sw.WriteLine(linea);
                foreach (var item in inventario.DetailEquipos)
                {
                    linea = item.Inventario.Empresa.Nombre + "; " +
                            item.Inventario.Empresa.Codigo + "; " +
                            item.Inventario.Sede.NombreSede + "; " +
                            item.Inventario.Empresa.Ciudad.Ciudad + "; " +
                            item.Inventario.Empresa.Ciudad.NivelTermico.NombreNivelTermico + "; " +
                            item.Inventario.Asesor.NombreAsesor + "; " +
                            item.Inventario.Asesor.Codigo + "; " +
                            item.Inventario.Fecha + "; " +
                            item.Inventario.ValorKWH + "; " +
                            item.Inventario.MetrosCuadrados + "; " +
                            item.Equipos.TipoEquipo.NombreTipoEquipo + "; " +
                            item.Equipos.DescripcionEquipo + "; " +
                            item.Equipos.Consumo + "; " +
                            item.Equipos.Watt + "; " +
                            item.ConsumoSemana + "; " +
                            item.ConsumoSabado + "; " +
                            item.ConsumoDomingo + "; " +
                            (item.ConsumoSemana + item.ConsumoSabado + item.ConsumoDomingo) * (decimal)4.33;

                    sw.WriteLine(linea);
                }
                sw.Close();

                return File(csv, System.Net.Mime.MediaTypeNames.Application.Octet, "Inventario.csv");

            }
            catch (Exception ex)
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
        /// View in Detail Mode
        /// <example> GET: Consumoes/Details/5 </example>
        /// </summary>
        [Authorize(Roles = "AccessAll,UserManagement")]
        public ActionResult Details(int? id)
        {
            if (id == null)
                return HttpNotFound();

            var objView = (from i in db.Inventario.Include(c => c.Asesor).Include(i => i.Sede)
                              join d in db.Ciudades.Include(d => d.NivelTermico) on i.Sede.CiudadID equals d.CiudadID
                              where i.InventarioSedeID == id.Value
                              select new InventarioIndexModel
                              {
                                  InventarioSedeID = i.InventarioSedeID,
                                  NombreEmpresa = i.Empresa.Nombre,
                                  NombreSede = i.Sede.NombreSede,
                                  Dimension = i.MetrosCuadrados,
                                  NombreCiudad = d.Ciudad,
                                  Fecha = i.Fecha,
                                  NombreNivelTermico = d.NivelTermico.NombreNivelTermico,
                                  NombreAsesor = i.Asesor.NombreAsesor
                              }).FirstOrDefault();

            objView.DetailEquipos = db.InventarioEquipos.Include(i => i.Equipos).Where(i => i.InventarioSedeID == objView.InventarioSedeID).ToList();
            objView.DetailAcciones = db.AccionesAhorroSedes.Include(aa => aa.TiposEquipos).Include(aa => aa.AccionAhorro).Where(aa => aa.InventarioSedeID == objView.InventarioSedeID).ToList();

            if (objView == null)
                return HttpNotFound();

            return View(objView);
        }

        /// <summary>
        /// Print View By Inventario ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "AccessAll,UserManagement")]
        public ActionResult Print(int id)
        {
            var inventario = GetModelToPrint(id);

            if (inventario == null)
            {
                return new JsonResult
                {
                    Data = new { Message = "No es posible recuperar el inventario seleccionado. Por favor inténtelo de nuevo", Success = false },
                    ContentEncoding = Encoding.UTF8,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }

            try
            {
                var RptContent = GetInventarioContentToPrint(inventario);

                inventario.DetailAcciones = null;
                inventario.DetailEquipos = null;
                inventario.Content = RptContent.Content;
                inventario.ContentResumen = RptContent.ContentResumen;

                return View("Print", inventario);
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
        /// Send From Email
        /// </summary>
        /// <param name="CliID">Client ID</param>
        /// <returns></returns>        
        public ActionResult DownloadPDF(int iID)
        {
            try
            {
                var inventario = GetModelToPrint(iID);
                if (inventario == null)
                    return null;

                string FileName = $"Inventario_{Guid.NewGuid().ToString()}.pdf";

                var RptContent = GetInventarioContentToPrint(inventario, false);

                string Style = "<style>table {border-collapse: collapse;} .spTable {border: 1px solid black;border-bottom: 1px solid #000000 !important;} .spRow {padding: 2px; border: 1px solid black;border-bottom: 1px solid #000000 !important;} .spCellSimple {padding: 2px; border: 1px solid black;border-bottom: 1px solid #000000 !important;} .spCell {background-color: lightgray;}</style>";
                string HeaderParams = $"<center><h1>Inventario Equipos Eléctricos al {inventario.FechaShort}</h1></center><br /><table style='width: 100%'><tr><td valign='top' style='width: 45%; padding: 5px;'><table style='width: 100%; padding: 5px;'><tr><td style='padding: 3px;'><b>Código: </b>{inventario.CodigoEmpresa}</td><td style='padding: 3px;'><b>Empresa: </b>{inventario.NombreEmpresa}</td></tr><tr><td style='padding: 3px;'><b>Sector: </b>{inventario.SectorEconomico}</td><td style='padding: 3px;'><b>Prestador: </b>{inventario.PrestadoraServicio}</td></tr><tr><td style='padding: 3px;'><b>Ciudad: </b>{inventario.NombreCiudad}</td><td style='padding: 3px;'><b>Nivel Térmico: </b>{inventario.NombreNivelTermico}</td></tr><tr><td style='padding: 3px;'><b>Código Asesor: </b>{inventario.CodigoAsesor}</td><td style='padding: 3px;'><b>Nombre Asesor: </b>{inventario.NombreAsesor}</td></tr><tr><td style='padding: 3px;'><b>Valor KWH: </b>{inventario.ValorKWHStr}</td><td style='padding: 3px;'><b>Consumo KWH: </b>{inventario.ConsumoStr}</td></tr><tr><td style='padding: 3px;'><b>Dimensión: </b>{inventario.MetrosCuadradosStr}</td><td></td></tr></table></td><td valign='top' style='width: 5%; padding: 3px;'></td><td valign='top' style='width: 50%; padding: 3px;'><div id='contentResumen' class='table-responsive'>{RptContent.ContentResumen.ToString()}</div></td></tr></table><br /><h2>Equipos</h2><br />"; 
                string AllRpt = $"{Style} {HeaderParams} <div id='customprintarea'><div id='contentRaw' class='table-responsive'>{RptContent.Content.ToString()}</div></div>";

                byte[] res = null;
                using (MemoryStream ms = new MemoryStream())
                {
                    var config = new TheArtOfDev.HtmlRenderer.PdfSharp.PdfGenerateConfig();
                    config.PageOrientation = PdfSharp.PageOrientation.Landscape;
                    config.PageSize = PdfSharp.PageSize.A4;
                    config.SetMargins(15);

                    var pdf = TheArtOfDev.HtmlRenderer.PdfSharp.PdfGenerator.GeneratePdf(AllRpt, config);
                    pdf.Save(ms);
                    res = ms.ToArray();
                }

                if (res == null || res.Length == 0)
                    return View("Error");

                return File(res, System.Net.Mime.MediaTypeNames.Application.Octet, "Inventario.pdf");
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Get Inventario Content to Print
        /// </summary>
        /// <param name="inventario"></param>
        /// <returns></returns>
        private InternalPrintModel GetInventarioContentToPrint(InventarioPrintModel inventario, bool IncludeBR = true)
        {
            StringBuilder RptContent = new StringBuilder();
            StringBuilder RptResumenContent = new StringBuilder();

            List<string> TiposDeEquipos = new List<string>();
            List<InternalResumenModel> Resumen = new List<InternalResumenModel>();

            foreach (var item in inventario.DetailEquipos)
            {
                if (!TiposDeEquipos.Contains(item.Equipos.TipoEquipo.NombreTipoEquipo))
                    TiposDeEquipos.Add(item.Equipos.TipoEquipo.NombreTipoEquipo);
            }

            StringBuilder strRowTemp = new StringBuilder();
            int TotalUnds = 0;
            decimal TotalConsumoSemana = 0;
            decimal TotalConsumoSabado = 0;
            decimal TotalConsumoDomingo = 0;
            decimal TotalConsumoMes = 0;
            bool ActionTitleAdded = false;

            TiposDeEquipos.Sort();

            if (!IncludeBR)
            {
                RptContent.Append("<table width='100%' id='rptTable' class='table table-hover spTable'><thead><tr class='spRow'><td class='spCellSimple'></td><td class='spCellSimple'></td><td class='spCellSimple'></td><td align='center' class='spCell spCellSimple'><b>UNDS</b></td><td align='center' class='spCell spCellSimple'><b>CONS. DIA L-V</b></td><td align='center' class='spCell spCellSimple'><b>CONS. DIA SAB</b></td><td align='center' class='spCell spCellSimple'><b>CONS. DIA DOM</b></td><td align='center' class='spCell spCellSimple'><b>CONS. MES</b></td></tr></thead>");
            }
            else
            {
                RptContent.Append("<table width='100%' id='rptTable' class='table table-hover spTable'><thead><tr class='spRow'><td class='spCellSimple'></td><td class='spCellSimple'></td><td class='spCellSimple'></td><td align='center' class='spCell spCellSimple'><b>UNDS</b></td><td align='center' class='spCell spCellSimple'><b>CONSUMO <br />DIA L-V</b></td><td align='center' class='spCell spCellSimple'><b>CONSUMO <br />DIA SAB</b></td><td align='center' class='spCell spCellSimple'><b>CONSUMO <br />DIA DOM</b></td><td align='center' class='spCell spCellSimple'><b>CONSUMO <br />MES</b></td></tr></thead>");
            }

            RptContent.Append("<tbody>");

            TiposDeEquipos.ForEach(TipoEquipo => {

                strRowTemp = new StringBuilder();
                TotalUnds = 0;
                TotalConsumoSemana = 0;
                TotalConsumoSabado = 0;
                TotalConsumoDomingo = 0;
                TotalConsumoMes = 0;
                ActionTitleAdded = false;

                foreach (var item in inventario.DetailEquipos)
                {
                    if (item.Equipos.TipoEquipo.NombreTipoEquipo.Equals(TipoEquipo))
                    {
                        TotalUnds += item.Cantidad;

                        TotalConsumoSemana += item.ConsumoSemana;
                        TotalConsumoSabado += item.ConsumoSabado;
                        TotalConsumoDomingo += item.ConsumoDomingo;
                        TotalConsumoMes += (item.ConsumoSemana + item.ConsumoSabado + item.ConsumoDomingo) * 4.33M;

                        strRowTemp.Append($"<tr class='spRow'><td class='spCellSimple'>{item.Equipos.DescripcionEquipo.Replace("\"", "\\\"")}</td><td class='spCellSimple' align='center'>{item.Equipos.Watt.ToString("N2")}</td><td class='spCellSimple' align='center'>{item.Equipos.Consumo.ToString("N3")}</td><td class='spCellSimple' align='center'>{item.Cantidad.ToString("N2")}</td><td class='spCellSimple' align='center'>{item.ConsumoSemana.ToString("N2")}</td><td class='spCellSimple' align='center'>{item.ConsumoSabado.ToString("N2")}</td><td class='spCellSimple' align='center'>{item.ConsumoDomingo.ToString("N2")}</td><td class='spCellSimple' align='center'>{((item.ConsumoSemana + item.ConsumoSabado + item.ConsumoDomingo) * 4.33M).ToString("N2")}</td></tr>");
                    }
                }

                RptContent.Append($"<tr class='spRow'><td class='spCell spCellSimple'><b>{TipoEquipo.ToUpper()}</b></td><td class='spCell spCellSimple' align='center'><b>W</b></td><td class='spCell spCellSimple' align='center'><b>CONSUMO KWH</b></td><td class='spCell spCellSimple' align='center'><b>{TotalUnds.ToString("N2")}</b></td><td class='spCell spCellSimple' align='center'><b>{TotalConsumoSemana.ToString("N2")}</b></td><td class='spCell spCellSimple' align='center'><b>{TotalConsumoSabado.ToString("N2")}</b></td><td class='spCell spCellSimple' align='center'><b>{TotalConsumoDomingo.ToString("N2")}</b></td><td class='spCell spCellSimple' align='center'><b>{TotalConsumoMes.ToString("N2")}</b></td></tr>");
                RptContent.Append(strRowTemp.ToString());

                Resumen.Add(new InternalResumenModel() { Tipo = TipoEquipo.ToUpper(), ConsumoMes = TotalConsumoMes, Participacion = 0 });

                foreach (var item in inventario.DetailAcciones)
                {
                    if (item.TiposEquipos.NombreTipoEquipo.Equals(TipoEquipo))
                    {
                        if (!ActionTitleAdded)
                        {
                            if (!IncludeBR)
                            {
                                RptContent.Append($"<tr><td class='spCell spCellSimple' colspan='8'>ACCIONES DE AHORRO - {TipoEquipo.ToUpper()}</td></tr>");
                            }
                            else
                            {
                                RptContent.Append($"<tr><td class='spCell spCellSimple' colspan='8'><b>ACCIONES DE AHORRO- {TipoEquipo.ToUpper()}</b></td></tr>");
                            }
                            ActionTitleAdded = true;
                        }

                        RptContent.Append($"<tr><td class='spCellSimple' colspan='8'>{item.AccionAhorro.DescripcionAccionAhorro}</td></tr>");
                    }
                }

                RptContent.Append("<tr><td colspan='8'></td></tr>");

            });

            RptContent.Append("</tbody>");
            RptContent.Append("</table>");

            if (Resumen != null && Resumen.Count > 0)
            {
                decimal TotalValue = 0;
                Resumen.ForEach(line => TotalValue += line.ConsumoMes);
                Resumen.ForEach(line => line.Participacion = ((line.ConsumoMes * 100) / TotalValue));

                RptResumenContent = new StringBuilder();
                RptResumenContent.Append("<table width='100%' id='rptTableRes' class='table table-hover spTable'><thead><tr class='spRow'><td class='spCell spCellSimple'><b>TIPO DE EQUIPO</b></td><td align='right' class='spCell spCellSimple'><b>CONSUMO MES</b></td><td align='right' class='spCell spCellSimple'><b>%</b></td></tr></thead>");
                RptResumenContent.Append("<tbody>");

                foreach (var item in Resumen)
                {
                    RptResumenContent.Append($"<tr><td>{item.Tipo}</td><td align='right'>{item.ConsumoMes.ToString("N2")}</td><td align='right'>{item.Participacion.ToString("N2")}</td></tr>");
                }

                RptResumenContent.Append($"<tr><td><b>TOTAL</b></td><td align='right'>{TotalValue.ToString("N2")}</td><td></td></tr>");

                RptResumenContent.Append("</tbody>");
                RptResumenContent.Append("</table>");
            }

            InternalPrintModel model = new InternalPrintModel()
            {
                Content = RptContent.ToString(),
                ContentResumen = RptResumenContent.ToString()
            };

            return model;
        }

        /// <summary>
        /// Get Model To Print
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private InventarioPrintModel GetModelToPrint(int id)
        {
            var inventario = (from i in db.Inventario.Include(c => c.Asesor).Include(i => i.Sede)
                              join d in db.Ciudades.Include(d => d.NivelTermico) on i.Sede.CiudadID equals d.CiudadID
                              where i.InventarioSedeID == id
                              select new InventarioPrintModel
                              {
                                  InventarioSedeID = i.InventarioSedeID,
                                  CodigoEmpresa = i.Empresa.Codigo,
                                  NombreEmpresa = i.Empresa.Nombre,
                                  SectorEconomico = i.Empresa.SectorEconomico.SectorEconomico,
                                  NombreSede = i.Sede.NombreSede,
                                  PrestadoraServicio = i.PrestadoraServicio,
                                  Consumo = i.Consumo,
                                  ValorKWH = i.ValorKWH,
                                  MetrosCuadrados = i.MetrosCuadrados,
                                  NombreCiudad = d.Ciudad,
                                  Fecha = i.Fecha,
                                  NombreNivelTermico = d.NivelTermico.NombreNivelTermico,
                                  NombreAsesor = i.Asesor.NombreAsesor,
                                  CodigoAsesor = i.Asesor.Codigo
                              }).FirstOrDefault();
            
            if (inventario != null)
            {
                inventario.ConsumoStr = inventario.Consumo.ToString("N2");
                inventario.ValorKWHStr = inventario.ValorKWH.ToString("C2");
                inventario.MetrosCuadradosStr = inventario.MetrosCuadrados.ToString("N0");
                inventario.DetailEquipos = db.InventarioEquipos.Include(i => i.Equipos).Where(i => i.InventarioSedeID == inventario.InventarioSedeID).ToList();
                inventario.DetailAcciones = db.AccionesAhorroSedes.Include(aa => aa.TiposEquipos).Include(aa => aa.AccionAhorro).Where(aa => aa.InventarioSedeID == inventario.InventarioSedeID).ToList();
            }

            return inventario;
        }

        /// <summary>
        /// Filter View By EmpresaID (int? id)
        /// </summary>    
        [Authorize(Roles = "AccessAll,UserManagement")]
        public ActionResult FilterByEmpresaID(int? id)
        {
            if (id != null && id.HasValue)
            {
                var Empresa = db.Empresas.Where(e => e.EmpresaID.Equals(id.Value)).FirstOrDefault();
                if (Empresa != null)
                {
                    ViewBag.Caption = $"Inventarios de la empresa: {Empresa.Nombre}";
                }
                else
                {
                    ViewBag.Caption = "Inventarios";
                }

                if (Helpers.ApplicationContext.CurrentUser.IsSuperAdmin)
                    ViewBag.EmpresaID = new SelectList(db.Empresas.OrderBy(e => e.Nombre), "EmpresaID", "Nombre", id.Value);

                var inventarios = (from i in db.Inventario.Include(c => c.Asesor).Include(i => i.Sede)
                                   join d in db.Ciudades.Include(d => d.NivelTermico) on i.Sede.CiudadID equals d.CiudadID
                                   where i.EmpresaID == id.Value
                                   select new InventarioIndexModel
                                   {
                                       InventarioSedeID = i.InventarioSedeID,
                                       NombreEmpresa = i.Empresa.Nombre,
                                       NombreSede = i.Sede.NombreSede,
                                       Dimension = i.MetrosCuadrados,
                                       NombreCiudad = d.Ciudad,
                                       Fecha = i.Fecha,
                                       NombreNivelTermico = d.NivelTermico.NombreNivelTermico,
                                       NombreAsesor = i.Asesor.NombreAsesor
                                   });

                return View("Index", inventarios);
            }
            else
            {
                return Index();
            }
        }

        /// <summary>
        /// Delete Object From AjaxJQuery
        /// </summary>
        /// <param name="id">PK Value</param>
        /// <returns></returns>        
        public ActionResult Delete(int id)
        {
            DbContextTransaction Transaction = null;

            try
            {
                Transaction = db.Database.BeginTransaction();

                Inventario inventario = db.Inventario.Include(c => c.InventarioEquipos).Include(i => i.AccionesAhorroSedes).Where(c => c.InventarioSedeID == id).FirstOrDefault();
                if (inventario != null)
                {
                    if (inventario.InventarioEquipos != null && inventario.InventarioEquipos.Count > 0)
                    {
                        List<InventarioEquipos> ItemsToDelete = inventario.InventarioEquipos.ToList();

                        foreach (var InventarioItem in ItemsToDelete)
                            db.InventarioEquipos.Remove(InventarioItem);
                    }

                    if (inventario.AccionesAhorroSedes != null && inventario.AccionesAhorroSedes.Count > 0)
                    {
                        List<AccionesAhorroSedes> ItemsToDelete = inventario.AccionesAhorroSedes.ToList();

                        foreach (var ActionToDelete in ItemsToDelete)
                            db.AccionesAhorroSedes.Remove(ActionToDelete);
                    }

                    db.Inventario.Remove(inventario);
                    db.SaveChanges();
                }

                Transaction.Commit();

                return new JsonResult
                {
                    Data = new { Message = string.Empty, Success = true },
                    ContentEncoding = System.Text.Encoding.UTF8,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            catch
            {
                if (Transaction != null)
                    Transaction.Rollback();

                return new JsonResult
                {
                    Data = new { Message = "Error ejecutando la acción de eliminar. Por favor inténtelo de nuevo", Success = false },
                    ContentEncoding = System.Text.Encoding.UTF8,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        /// <summary>
        /// Delete Object From AjaxJQuery
        /// </summary>
        /// <param name="id">PK Value</param>
        /// <returns></returns>        
        public ActionResult DeleteEquipo(int id)
        {
            try
            {
                InventarioEquipos ItemDetail = db.InventarioEquipos.Find(id);
                if (ItemDetail != null)
                {
                    db.InventarioEquipos.Remove(ItemDetail);
                    db.SaveChanges();

                    return new JsonResult
                    {
                        Data = new { Success = true },
                        ContentEncoding = System.Text.Encoding.UTF8,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }

                return new JsonResult
                {
                    Data = new { Message = "Por favor inténtelo de nuevo", Success = false },
                    ContentEncoding = System.Text.Encoding.UTF8,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            catch
            {
                //
                // Log Exception eX
                //

                return new JsonResult
                {
                    Data = new { Message = "Error ejecutando la acción de eliminar. Por favor inténtelo de nuevo.", Success = false },
                    ContentEncoding = System.Text.Encoding.UTF8,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        /// <summary>
        /// Delete Object From AjaxJQuery
        /// </summary>
        /// <param name="id">PK Value</param>
        /// <returns></returns>        
        public ActionResult DeleteActions(int id)
        {
            try
            {
                AccionesAhorroSedes ItemDetail = db.AccionesAhorroSedes.Find(id);
                if (ItemDetail != null)
                {
                    db.AccionesAhorroSedes.Remove(ItemDetail);
                    db.SaveChanges();

                    return new JsonResult
                    {
                        Data = new { Success = true },
                        ContentEncoding = System.Text.Encoding.UTF8,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }

                return new JsonResult
                {
                    Data = new { Message = "Por favor inténtelo de nuevo", Success = false },
                    ContentEncoding = System.Text.Encoding.UTF8,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            catch
            {
                //
                // Log Exception eX
                //

                return new JsonResult
                {
                    Data = new { Message = "Error ejecutando la acción de eliminar. Por favor inténtelo de nuevo.", Success = false },
                    ContentEncoding = System.Text.Encoding.UTF8,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        /// <summary>
        /// Action Create New Object
        /// </summary>
        [Authorize(Roles = "AccessAll,UserManagement")]
        public ActionResult Manage(int? id)
        {
            if (id.HasValue)
            {
                var Inventario = db.Inventario.Include(c => c.InventarioEquipos).Include(c => c.AccionesAhorroSedes).Where(c => c.InventarioSedeID == id).FirstOrDefault();
                if (Inventario != null)
                {
                    Inventario.Fecha = Helpers.DateHelper.GetColombiaDateTime();
                    Inventario.IsSaved = true;
                    Inventario.ItemTiposEquipos = db.TiposEquipos.OrderBy(te => te.NombreTipoEquipo).ToList();
                    Inventario.ItemCantidad = 1;

                    if (Inventario.InventarioEquipos != null && Inventario.InventarioEquipos.Count > 0)
                    {
                        foreach (var item in Inventario.InventarioEquipos)
                            item.ConsumoMes = (item.ConsumoSemana + item.ConsumoSabado + item.ConsumoDomingo) * 4.33M;
                    }

                    SetViewBagData(Inventario.SedeID, Inventario.AsesorID, Inventario.EmpresaID);
                }

                return View(Inventario);
            }
            else
            {
                SetViewBagData();

                Inventario Cot = new Inventario()
                {
                    Fecha = Helpers.DateHelper.GetColombiaDateTime(),
                    IsSaved = false
                };

                return View(Cot);
            }
        }

        /// <summary>
        /// Set ViewBag Dispositivos
        /// </summary>
        private void SetViewBagData(int? SedeID = null, int? AsesorID = null, int? EmpID = null)
        {
            object DefSedeID = null;
            object DefAsesorID = null;
            object DefEmpID = Helpers.ApplicationContext.CurrentUser.EmpresaID;

            if (SedeID.HasValue)
                DefSedeID = SedeID.Value;

            if (AsesorID.HasValue)
                DefAsesorID = AsesorID.Value;

            if (EmpID.HasValue)
                DefEmpID = EmpID.Value;

            if (Helpers.ApplicationContext.CurrentUser.IsSuperAdmin)
            {
                ViewBag.EmpresaID = new SelectList(db.Empresas.OrderBy(e => e.Nombre), "EmpresaID", "Nombre", DefEmpID);
            }
            else
            {
                ViewBag.EmpresaID = new SelectList(db.Empresas.Where(e => e.EmpresaID == Helpers.ApplicationContext.CurrentUser.Empresa.EmpresaID).OrderBy(e => e.Nombre), "EmpresaID", "Nombre", DefEmpID);
            }

            if (EmpID.HasValue)
            {
                var Sedes = db.Sedes.Where(e => e.EmpresaID == EmpID.Value).OrderBy(e => e.NombreSede);
                if (!SedeID.HasValue && Sedes != null && Sedes.Any())
                    DefSedeID = Sedes.FirstOrDefault().SedeID;

                ViewBag.SedeID = new SelectList(Sedes, "SedeID", "NombreSede", DefSedeID);
            }
            else
            {
                var Sedes = db.Sedes.Where(e => e.EmpresaID == Helpers.ApplicationContext.CurrentUser.Empresa.EmpresaID).OrderBy(e => e.NombreSede);
                if (!SedeID.HasValue && Sedes != null && Sedes.Any())
                    DefSedeID = Sedes.FirstOrDefault().SedeID;

                ViewBag.SedeID = new SelectList(Sedes, "SedeID", "NombreSede", DefSedeID);
            }

            var Asesores = db.Asesors.OrderBy(d => d.NombreAsesor);
            if (!AsesorID.HasValue && Asesores != null && Asesores.Any())
                DefAsesorID = Asesores.FirstOrDefault().AsesorID;

            ViewBag.AsesorID = new SelectList(Asesores, "AsesorID", "NombreAsesor", DefAsesorID);

            ViewBag.ItemProductoID = new SelectList(db.Equipos.OrderBy(s => s.DescripcionEquipo), "EquipoID", "DescripcionEquipo", null);

            var TiposDeEquipos = db.TiposEquipos.OrderBy(s => s.NombreTipoEquipo);
            TiposEquipos DefTipoEquipo = null;

            if (TiposDeEquipos.Any())
                DefTipoEquipo = TiposDeEquipos.FirstOrDefault();

            ViewBag.ItemTiposEquipos = new SelectList(TiposDeEquipos, "TipoEquipoID", "NombreTipoEquipo", DefTipoEquipo);
            ViewBag.ItemTiposEquiposAction = new SelectList(TiposDeEquipos, "TipoEquipoID", "NombreTipoEquipo", DefTipoEquipo);

            if (DefTipoEquipo != null)
            {
                ViewBag.ItemEquipoID = new SelectList(db.Equipos.Where(e => e.TipoEquipoID == DefTipoEquipo.TipoEquipoID).OrderBy(e => e.DescripcionEquipo), "EquipoID", "DescripcionEquipo", null);

                var AccionesAhorro = db.AccionAhorro.Where(a => a.TipoEquipoID == DefTipoEquipo.TipoEquipoID).OrderBy(s => s.DescripcionAccionAhorro);
                AccionAhorro DefAccion = null;

                if (AccionesAhorro.Any())
                    DefAccion = AccionesAhorro.FirstOrDefault();

                ViewBag.ItemAccionesAhorro = new SelectList(AccionesAhorro, "AccionAhorroID", "DescripcionAccionAhorro", DefAccion);
            }
            else
            {
                ViewBag.ItemEquipoID = new SelectList(db.Equipos.OrderBy(e => e.DescripcionEquipo), "EquipoID", "DescripcionEquipo", null);


                var AccionesAhorro = db.AccionAhorro.OrderBy(s => s.DescripcionAccionAhorro);
                AccionAhorro DefAccion = null;

                if (AccionesAhorro.Any())
                    DefAccion = AccionesAhorro.FirstOrDefault();

                ViewBag.ItemAccionesAhorro = new SelectList(AccionesAhorro, "AccionAhorroID", "DescripcionAccionAhorro", DefAccion);
            }
        }

        [HttpPost]
        public ActionResult GetSedesByEmpId(int empID)
        {
            try
            {
                if (empID == 999999 || empID == -1)
                {
                    var dispositivos = new SelectList((from sed in db.Sedes
                                                       where sed.EmpresaID == Helpers.ApplicationContext.CurrentUser.Empresa.EmpresaID
                                                       orderby sed.NombreSede
                                                       select sed).ToList(), "SedeID", "NombreSede", 0);

                    return new JsonResult { Data = new { List = dispositivos, Success = true }, ContentEncoding = System.Text.Encoding.UTF8, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                else
                {
                    return new JsonResult
                    {
                        Data = new { List = new SelectList(db.Sedes.OrderBy(d => d.NombreSede).Where(m => m.EmpresaID == empID).ToList(), "SedeID", "NombreSede", 0), Success = true },
                        ContentEncoding = System.Text.Encoding.UTF8,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }
            }
            catch
            {
                return new JsonResult
                {
                    Data = new { Message = "Error ejecutando la acción. Por favor inténtelo de nuevo", Success = false },
                    ContentEncoding = System.Text.Encoding.UTF8,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        [HttpPost]
        public ActionResult GetEquiposByTipoID(int tipoEquipoID)
        {
            try
            {
                return new JsonResult
                {
                    Data = new { List = new SelectList(db.Equipos.OrderBy(d => d.DescripcionEquipo).Where(m => m.TipoEquipoID == tipoEquipoID).ToList(), "EquipoID", "DescripcionEquipo", 0), Success = true },
                    ContentEncoding = System.Text.Encoding.UTF8,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            catch
            {
                return new JsonResult
                {
                    Data = new { Message = "Error ejecutando la acción. Por favor inténtelo de nuevo", Success = false },
                    ContentEncoding = System.Text.Encoding.UTF8,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        [HttpPost]
        public ActionResult GetAccionesByTipoID(int tipoEquipoID)
        {
            try
            {
                return new JsonResult
                {
                    Data = new { List = new SelectList(db.AccionAhorro.OrderBy(d => d.DescripcionAccionAhorro).Where(m => m.TipoEquipoID == tipoEquipoID).ToList(), "AccionAhorroID", "DescripcionAccionAhorro", 0), Success = true },
                    ContentEncoding = System.Text.Encoding.UTF8,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            catch
            {
                return new JsonResult
                {
                    Data = new { Message = "Error ejecutando la acción. Por favor inténtelo de nuevo", Success = false },
                    ContentEncoding = System.Text.Encoding.UTF8,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        /// <summary>
        /// Action Create New Object
        /// <example> POST: Consumoes/Create </example>
        /// </summary>
        [Authorize(Roles = "AccessAll,UserManagement")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save([Bind(Include = "InventarioSedeID,IsSaved,EmpresaID,SedeID,AsesorID,Fecha,PrestadoraServicio,Consumo,ValorKWH,MetrosCuadrados,HorasSemana,HorasSabado,HorasDomingo")] Inventario inventario)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (inventario.EmpresaID < 1)
                        inventario.EmpresaID = Helpers.ApplicationContext.CurrentUser.EmpresaID;

                    if (!inventario.IsSaved)
                    {
                        db.Inventario.Add(inventario);
                        db.SaveChanges();
                    }
                    else
                    {
                        db.Entry(inventario).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    inventario = db.Inventario.Include(c => c.InventarioEquipos).Where(c => c.InventarioSedeID == inventario.InventarioSedeID).FirstOrDefault();
                    if (inventario != null && inventario.InventarioEquipos != null && inventario.InventarioEquipos.Count > 0)
                    {
                        foreach (var item in inventario.InventarioEquipos)
                        {
                            Equipos eq = db.Equipos.Where(e => e.EquipoID == item.EquipoID).FirstOrDefault();
                            if (eq != null)
                            {
                                decimal Semana = (item.Cantidad * eq.Consumo) * inventario.HorasSemana;
                                decimal Sabado = (item.Cantidad * eq.Consumo) * inventario.HorasSabado;
                                decimal Domingo = (item.Cantidad * eq.Consumo) * inventario.HorasDomingo;

                                item.ConsumoSemana = Semana;
                                item.ConsumoSabado = Sabado;
                                item.ConsumoDomingo = Domingo;
                                item.ConsumoMes = (item.ConsumoSemana + item.ConsumoSabado + item.ConsumoDomingo) * 4.33M;

                                db.Entry(item).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                        }
                    }

                    inventario = db.Inventario.Include(c => c.InventarioEquipos).Include(i => i.AccionesAhorroSedes).Where(c => c.InventarioSedeID == inventario.InventarioSedeID).FirstOrDefault();
                    inventario.InventarioEquipos.OrderBy(ie => ie.Equipos.TipoEquipo.NombreTipoEquipo).OrderBy(ie => ie.Equipos.DescripcionEquipo);
                }
                catch (Exception eX)
                {
                    ModelState.AddModelError("", eX.Message);
                }
            }

            SetViewBagData(inventario.SedeID, inventario.AsesorID, inventario.EmpresaID);
            return View("Manage", inventario);
        }
        
        /// <summary>
        /// Save Model
        /// </summary>
        /// <returns></returns>        
        [HttpPost]
        public ActionResult SaveNewEq(InventarioEditViewModel Model)
        {
            DbContextTransaction Transaction = null;

            try
            {
                if (Model != null)
                {
                    if (Model.EmpID < 1)
                        Model.EmpID = Helpers.ApplicationContext.CurrentUser.EmpresaID;

                    if (!Model.IsSaved)
                    {
                        Transaction = db.Database.BeginTransaction();

                        Inventario NewModel = new Inventario();
                        NewModel.Fecha = Model.Fec;
                        NewModel.EmpresaID = Model.EmpID;
                        NewModel.SedeID = Model.SedeID;
                        NewModel.AsesorID = Model.AsesID;
                        NewModel.PrestadoraServicio = Model.PrestardoraServ;
                        NewModel.Consumo = Model.Cons;
                        NewModel.ValorKWH = Model.ValK;
                        NewModel.MetrosCuadrados = Model.MetCua;
                        NewModel.HorasSemana = Model.HorSem;
                        NewModel.HorasSabado = Model.HorSab;
                        NewModel.HorasDomingo = Model.HorDom;

                        NewModel.IsSaved = true;

                        db.Inventario.Add(NewModel);
                        db.SaveChanges();

                        Equipos eq = db.Equipos.Where(e => e.EquipoID == Model.EqID).FirstOrDefault();
                        if (eq != null)
                        {
                            decimal Semana = (Model.Can * eq.Consumo) * NewModel.HorasSemana;
                            decimal Sabado = (Model.Can * eq.Consumo) * NewModel.HorasSabado;
                            decimal Domingo = (Model.Can * eq.Consumo) * NewModel.HorasDomingo;

                            InventarioEquipos NewItemModel = new InventarioEquipos()
                            {
                                InventarioSedeID = NewModel.InventarioSedeID,
                                EquipoID = Model.EqID,
                                Cantidad = Model.Can,
                                ConsumoSemana = Semana,
                                ConsumoSabado = Sabado,
                                ConsumoDomingo = Domingo,
                                ConsumoMes = (Semana + Sabado + Domingo) * 4.33M
                            };

                            db.InventarioEquipos.Add(NewItemModel);
                            db.SaveChanges();
                        }

                        Transaction.Commit();

                        return new JsonResult
                        {
                            Data = new { CotID = NewModel.InventarioSedeID, Success = true },
                            ContentEncoding = System.Text.Encoding.UTF8,
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet
                        };
                    }
                    else
                    {
                        var ProductExist = db.InventarioEquipos.Where(cd => cd.InventarioSedeID == Model.PkID && cd.EquipoID == Model.EqID).Any();
                        if (ProductExist)
                        {
                            return new JsonResult
                            {
                                Data = new { Message = "Este equipo ya fue agregado al inventario. Seleccione otro equipo.", Success = false },
                                ContentEncoding = System.Text.Encoding.UTF8,
                                JsonRequestBehavior = JsonRequestBehavior.AllowGet
                            };
                        }

                        var InventarioSaved = db.Inventario.Where(c => c.InventarioSedeID == Model.PkID).FirstOrDefault();
                        var Equipo = db.Equipos.Include(e => e.TipoEquipo).Where(p => p.EquipoID == Model.EqID).FirstOrDefault();

                        if (InventarioSaved != null && Equipo != null)
                        {
                            Equipos eq = db.Equipos.Where(e => e.EquipoID == Model.EqID).FirstOrDefault();
                            if (eq != null)
                            {
                                decimal Semana = (Model.Can * eq.Consumo) * InventarioSaved.HorasSemana;
                                decimal Sabado = (Model.Can * eq.Consumo) * InventarioSaved.HorasSabado;
                                decimal Domingo = (Model.Can * eq.Consumo) * InventarioSaved.HorasDomingo;

                                InventarioEquipos NewItemModel = new InventarioEquipos()
                                {
                                    InventarioSedeID = Model.PkID,
                                    EquipoID = Model.EqID,
                                    Cantidad = Model.Can,
                                    ConsumoSemana = Semana,
                                    ConsumoSabado = Sabado,
                                    ConsumoDomingo = Domingo,
                                    ConsumoMes = ((Semana * 4) + (Sabado * 4) + (Domingo * 4)) * 4.33M
                                };

                                db.InventarioEquipos.Add(NewItemModel);
                                db.SaveChanges();

                                string Actions = $"<td align='center'><a class=\"fa fa-pencil-square-o\" href=\"#\" title=\"Editar\" onclick=\"EditEquipo({NewItemModel.InventarioEquipoID}); return false;\" ></a> | <a class=\"fa fa-trash-o\" href=\"#\" data-backdrop=\"static\" data-href=\"Inventario\" data-pkid=\"{NewItemModel.InventarioEquipoID}\" data-toggle=\"modal\" data-target=\"#confirm-delete\" title=\"Eliminar\"></a></td>";
                                string MessageVal = $"<tr id=\"del{NewItemModel.InventarioEquipoID}\"><td>{Equipo.TipoEquipo.NombreTipoEquipo}</td><td>{Equipo.DescripcionEquipo}</td><td align=\"right\">{Equipo.Consumo}</td><td align=\"right\">{Model.Can.ToString("N0")}</td><td align=\"right\">{Semana.ToString("N2")}</td><td align=\"right\">{Sabado.ToString("N2")}</td><td align=\"right\">{Domingo.ToString("N2")}</td><td align=\"right\">{NewItemModel.ConsumoMes.ToString("N2")}</td>{Actions}</tr>";

                                return new JsonResult
                                {
                                    Data = new { Message = MessageVal, Success = true },
                                    ContentEncoding = System.Text.Encoding.UTF8,
                                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                                };
                            }
                        }
                    }
                }

                return new JsonResult
                {
                    Data = new { Message = "Los datos no se reconocen correctamente. Por favor inténtelo de nuevo.", Success = false },
                    ContentEncoding = System.Text.Encoding.UTF8,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            catch
            {
                if (Transaction != null)
                    Transaction.Rollback();

                //
                // Log Exception eX
                //

                return new JsonResult
                {
                    Data = new { Message = "Error ejecutando la acción. Por favor inténtelo de nuevo.", Success = false },
                    ContentEncoding = System.Text.Encoding.UTF8,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        /// <summary>
        /// Save Model
        /// </summary>
        /// <returns></returns>        
        [HttpPost]
        public ActionResult SaveNewAction(InventarioEditViewModel Model)
        {
            DbContextTransaction Transaction = null;

            try
            {
                if (Model != null)
                {
                    if (Model.EmpID < 1)
                        Model.EmpID = Helpers.ApplicationContext.CurrentUser.EmpresaID;

                    if (!Model.IsSaved)
                    {
                        Transaction = db.Database.BeginTransaction();

                        Inventario NewModel = new Inventario();
                        NewModel.Fecha = Model.Fec;
                        NewModel.EmpresaID = Model.EmpID;
                        NewModel.SedeID = Model.SedeID;
                        NewModel.AsesorID = Model.AsesID;
                        NewModel.PrestadoraServicio = Model.PrestardoraServ;
                        NewModel.Consumo = Model.Cons;
                        NewModel.ValorKWH = Model.ValK;
                        NewModel.MetrosCuadrados = Model.MetCua;
                        NewModel.HorasSemana = Model.HorSem;
                        NewModel.HorasSabado = Model.HorSab;
                        NewModel.HorasDomingo = Model.HorDom;

                        NewModel.IsSaved = true;

                        db.Inventario.Add(NewModel);
                        db.SaveChanges();

                        AccionesAhorroSedes NewActionModel = new AccionesAhorroSedes();
                        NewActionModel.InventarioSedeID = NewModel.InventarioSedeID;
                        NewActionModel.TipoEquipoID = Model.TipEq;
                        NewActionModel.AccionAhorroID = Model.Act;

                        db.AccionesAhorroSedes.Add(NewActionModel);
                        db.SaveChanges();

                        Transaction.Commit();

                        return new JsonResult
                        {
                            Data = new { CotID = NewModel.InventarioSedeID, Success = true },
                            ContentEncoding = System.Text.Encoding.UTF8,
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet
                        };
                    }
                    else
                    {
                        var ProductExist = db.AccionesAhorroSedes.Where(cd => cd.InventarioSedeID == Model.PkID && cd.TipoEquipoID == Model.TipEq && cd.AccionAhorroID == Model.Act).Any();
                        if (ProductExist)
                        {
                            return new JsonResult
                            {
                                Data = new { Message = "Esta acción ya fue agregado al inventario. Seleccione otro tipo / acción.", Success = false },
                                ContentEncoding = System.Text.Encoding.UTF8,
                                JsonRequestBehavior = JsonRequestBehavior.AllowGet
                            };
                        }

                        var InventarioSaved = db.Inventario.Where(c => c.InventarioSedeID == Model.PkID).FirstOrDefault();
                        var TipoEquipo = db.TiposEquipos.Where(p => p.TipoEquipoID == Model.TipEq).FirstOrDefault();
                        var Action = db.AccionAhorro.Where(p => p.AccionAhorroID == Model.Act).FirstOrDefault();

                        if (InventarioSaved != null && TipoEquipo != null && Action != null)
                        {
                            AccionesAhorroSedes NewActionModel = new AccionesAhorroSedes();
                            NewActionModel.InventarioSedeID = InventarioSaved.InventarioSedeID;
                            NewActionModel.TipoEquipoID = Model.TipEq;
                            NewActionModel.AccionAhorroID = Model.Act;

                            db.AccionesAhorroSedes.Add(NewActionModel);
                            db.SaveChanges();

                            string Actions = $"<td align='center'><a class=\"fa fa-trash-o\" href=\"#\" data-backdrop=\"static\" data-href=\"Inventario\" data-pkid=\"{NewActionModel.AccionSedeID}\" data-toggle=\"modal\" data-target=\"#confirm-delete-action\" title=\"Eliminar\"></a></td>";
                            string MessageVal = $"<tr id=\"act{NewActionModel.AccionSedeID}\"><td>{TipoEquipo.NombreTipoEquipo}</td><td>{Action.DescripcionAccionAhorro}</td>{Actions}</tr>";

                            return new JsonResult
                            {
                                Data = new { Message = MessageVal, Success = true },
                                ContentEncoding = System.Text.Encoding.UTF8,
                                JsonRequestBehavior = JsonRequestBehavior.AllowGet
                            };
                        }
                    }
                }

                return new JsonResult
                {
                    Data = new { Message = "Los datos no se reconocen correctamente. Por favor inténtelo de nuevo.", Success = false },
                    ContentEncoding = System.Text.Encoding.UTF8,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            catch
            {
                if (Transaction != null)
                    Transaction.Rollback();

                //
                // Log Exception eX
                //

                return new JsonResult
                {
                    Data = new { Message = "Error ejecutando la acción. Por favor inténtelo de nuevo.", Success = false },
                    ContentEncoding = System.Text.Encoding.UTF8,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        [HttpPost]
        public ActionResult GetEquiposRowDetail(int InvEqID)
        {
            try
            {
                var ItemDetail = db.InventarioEquipos.Include(i => i.Equipos).Where(cd => cd.InventarioEquipoID == InvEqID).FirstOrDefault();
                if (ItemDetail != null)
                {
                    return new JsonResult
                    {
                        Data = new
                        {
                            Can = ItemDetail.Cantidad,
                            TipEqID = ItemDetail.Equipos.TipoEquipoID,
                            EqID = ItemDetail.EquipoID,
                            Success = true
                        },
                        ContentEncoding = System.Text.Encoding.UTF8,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }

                return new JsonResult
                {
                    Data = new { Message = "El item no se reconoce. Por favor inténtelo de nuevo.", Success = true },
                    ContentEncoding = System.Text.Encoding.UTF8,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            catch
            {
                return new JsonResult
                {
                    Data = new { Message = "Error ejecutando la acción. Por favor inténtelo de nuevo.", Success = false },
                    ContentEncoding = System.Text.Encoding.UTF8,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        [HttpPost]
        public ActionResult UpdateEquipoDetail(int invEqID, Int16 can)
        {
            try
            {
                var DetailRow = db.InventarioEquipos.Include(ie => ie.Inventario).Include(ie => ie.Equipos).Where(ie => ie.InventarioEquipoID == invEqID).FirstOrDefault();
                if (DetailRow != null)
                {
                    DetailRow.Cantidad = can;

                    Equipos eq = db.Equipos.Where(e => e.EquipoID == DetailRow.EquipoID).FirstOrDefault();
                    if (eq != null)
                    {
                        decimal Semana = (can * eq.Consumo);
                        decimal Sabado = (can * eq.Consumo);
                        decimal Domingo = (can * eq.Consumo);

                        DetailRow.ConsumoSemana = Semana;
                        DetailRow.ConsumoSabado = Sabado;
                        DetailRow.ConsumoDomingo = Domingo;
                        DetailRow.ConsumoMes = ((Semana * 4) + (Sabado * 4) + (Domingo * 4)) * 4.33M;
                        
                        db.Entry(DetailRow).State = EntityState.Modified;
                        db.SaveChanges();

                        string Actions = $"<td><a class=\"fa fa-pencil-square-o\" href=\"#\" title=\"Editar\" onclick=\"EditEquipo({DetailRow.InventarioEquipoID}); return false;\" ></a> | <a class=\"fa fa-trash-o\" href=\"#\" data-backdrop=\"static\" data-href=\"Inventario\" data-pkid=\"{DetailRow.InventarioEquipoID}\" data-toggle=\"modal\" data-target=\"#confirm-delete\" title=\"Eliminar\"></a></td>";
                        string MessageVal = $"<tr id=\"del{DetailRow.InventarioEquipoID}\"><td>{DetailRow.Equipos.TipoEquipo.NombreTipoEquipo}</td><td>{DetailRow.Equipos.DescripcionEquipo}</td><td align=\"right\">{DetailRow.Equipos.Consumo.ToString("N3")}</td><td align=\"right\">{DetailRow.Cantidad.ToString("N0")}</td><td align=\"right\">{Semana.ToString("N2")}</td><td align=\"right\">{Sabado.ToString("N2")}</td><td align=\"right\">{Domingo.ToString("N2")}</td><td align=\"right\">{DetailRow.ConsumoMes.ToString("N2")}</td>{Actions}</tr>";

                        return new JsonResult
                        {
                            Data = new { Message = MessageVal, Success = true },
                            ContentEncoding = System.Text.Encoding.UTF8,
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet
                        };
                    }
                    else
                    {
                        return new JsonResult
                        {
                            Data = new { Message = "Los datos no se reconocen correctamente. Por favor inténtelo de nuevo.", Success = false },
                            ContentEncoding = System.Text.Encoding.UTF8,
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet
                        };
                    }
                }
                else
                {
                    return new JsonResult
                    {
                        Data = new { Message = "Los datos no se reconocen correctamente. Por favor inténtelo de nuevo.", Success = false },
                        ContentEncoding = System.Text.Encoding.UTF8,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }
            }
            catch
            {
                //
                // Log Exception eX
                //

                return new JsonResult
                {
                    Data = new { Message = "Error ejecutando la acción. Por favor inténtelo de nuevo.", Success = false },
                    ContentEncoding = System.Text.Encoding.UTF8,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }
    }
}