using EficienciaEnergetica.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;

namespace EficienciaEnergetica.Controllers
{
    /// <summary>
    /// ConsumoesController
    /// </summary>
    [NWebsec.Mvc.HttpHeaders.Csp.Csp(Enabled = false)]
    [NWebsec.Mvc.HttpHeaders.Csp.CspScriptSrc(Enabled = false)]
    public class ConsumosController : Controller
    {
        /// <summary>
        /// DataBase Context
        /// </summary>
        private EEContext db = new EEContext();

        /// <summary>
        /// View in List Mode
        /// <example> GET: Consumoes </example>
        /// </summary>
        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Index()
        {
            //TODO:Revisar esto
            if (Helpers.ApplicationContext.CurrentUser == null || Helpers.ApplicationContext.CurrentUser.Empresa == null)
                return View("../Authentication/Login");

            ViewBag.Caption = "Consumos";

            if (Helpers.ApplicationContext.CurrentUser.IsSuperAdmin)
                ViewBag.EmpresaID = new SelectList(db.Empresas.OrderBy(e => e.Nombre), "EmpresaID", "Nombre", Helpers.ApplicationContext.CurrentUser.EmpresaID);

            var consumo = (from c in db.Consumo.Include(c => c.Dispositivo)
                           join d in db.Dispositivos.Include(d => d.PeriodosFacturacion) on c.DispositivoID equals d.DispositivoID
                           join s in db.Sedes on d.SedeID equals s.SedeID
                           where s.EmpresaID == Helpers.ApplicationContext.CurrentUser.Empresa.EmpresaID
                           select c);

            var ConsumosList = consumo.ToList();
            ConsumosList.ForEach(c => {
                c.FuenteEnergetica = c.Dispositivo.Fuente.Fuente;
                c.UnidadMedida = c.Dispositivo.Fuente.UnidadMedida;
            });

            return View("Index", ConsumosList);
        }

        /// <summary>
        /// View in List Mode
        /// <example> GET: Dispositivos </example>
        /// <param name="idSede"></param>
        /// </summary>    
        [Authorize(Roles = "BusinessEntity")]
        public ActionResult FilterByEmpresaID(int? id)
        {
            if (id != null && id.HasValue)
            {
                var consumos = (from c in db.Consumo.Include(c => c.Dispositivo)
                               join d in db.Dispositivos.Include(d => d.PeriodosFacturacion) on c.DispositivoID equals d.DispositivoID
                               join s in db.Sedes on d.SedeID equals s.SedeID
                               where s.EmpresaID.Equals(id.Value)
                               select c).ToList();

                consumos.ForEach(c => {
                    c.FuenteEnergetica = c.Dispositivo.Fuente.Fuente;
                    c.UnidadMedida = c.Dispositivo.Fuente.UnidadMedida;
                });

                var Empresa = db.Empresas.Where(e => e.EmpresaID.Equals(id.Value)).FirstOrDefault();
                if (Empresa != null)
                {
                    ViewBag.Caption = $"Consumos de la empresa: {Empresa.Nombre}";
                }
                else
                {
                    ViewBag.Caption = "Consumos";
                }

                if (Helpers.ApplicationContext.CurrentUser.IsSuperAdmin)
                    ViewBag.EmpresaID = new SelectList(db.Empresas.OrderBy(e => e.Nombre), "EmpresaID", "Nombre", id.Value);

                return View("Index", consumos);
            }
            else
            {
                return Index();
            }
        }

        /// <summary>
        /// View in List Mode
        /// <example> GET: Dispositivos </example>
        /// <param name="idSede"></param>
        /// </summary>    
        [Authorize(Roles = "BusinessEntity")]
        public ActionResult IndexFilterByDispositivo(int? idDis)
        {
            if (idDis != null && idDis.HasValue)
            {
                object DefaultSelectedEmpresa = null;

                var Dispositivo = db.Dispositivos.Include(d => d.Sede).Where(d => d.DispositivoID.Equals(idDis.Value)).FirstOrDefault();
                if (Dispositivo != null)
                {
                    if (Dispositivo.Sede != null)
                        DefaultSelectedEmpresa = Dispositivo.Sede.EmpresaID;

                    ViewBag.Caption = $"Consumos del Dispositivo: {Dispositivo.Nombre}";
                }
                else
                {
                    ViewBag.Caption = "Consumos del Dispositivo";
                }

                if (Helpers.ApplicationContext.CurrentUser.IsSuperAdmin)
                    ViewBag.EmpresaID = new SelectList(db.Empresas.OrderBy(e => e.Nombre), "EmpresaID", "Nombre", DefaultSelectedEmpresa);

                var consumos = db.Consumo.Include(c => c.Dispositivo).Where(c => c.DispositivoID.Equals(idDis.Value)).ToList();
                consumos.ForEach(c => {
                    c.FuenteEnergetica = c.Dispositivo.Fuente.Fuente;
                    c.UnidadMedida = c.Dispositivo.Fuente.UnidadMedida;
                });

                return View("Index", consumos);
            }
            else
            {
                return Index();
            }
        }

        /// <summary>
        /// View in Detail Mode
        /// <example> GET: Consumoes/Details/5 </example>
        /// </summary>
        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Consumo consumo = db.Consumo.Find(id);
            if (consumo == null)
            {
                return HttpNotFound();
            }
            return View(consumo);
        }

        /// <summary>
        /// Action Create New Object
        /// <example> GET: Consumoes/Create </example>
        /// </summary>
        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Create()
        {
            SetViewBagData();
            return View();
        }
        
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Action Create New Object
        /// <example> POST: Consumoes/Create </example>
        /// </summary>
        [Authorize(Roles = "BusinessEntity")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SedeID,ConsumoID,DispositivoID,FechaInicial,FechaFinal,LineaBase,ConsumoPeriodo,Valor,ValorUnitario")] Consumo consumo)
        {
            if (ModelState.IsValid)
            {
                bool hasErrors = false;
                DbContextTransaction Transaction = null;

                try
                {
                    hasErrors = ModelNotIsValid(consumo);

                    if (!hasErrors)
                    {
                        Transaction = db.Database.BeginTransaction();

                        consumo.ValorUnitario = decimal.Round(consumo.ValorUnitario, 2);

                        db.Consumo.Add(consumo);
                        db.SaveChanges();

                        Transaction.Commit();
                    }
                }
                catch (Exception eX)
                {
                    if (Transaction != null)
                        Transaction.Rollback();

                    ModelState.AddModelError("", eX.Message);
                    hasErrors = true;
                }

                if (!hasErrors)
                    return RedirectToAction("Index");
            }

            SetViewBagData(consumo.SedeID, consumo.DispositivoID);
            return View(consumo);
        }
        
        /// <summary>
        /// Edit Object
        /// <example> GET: Consumoes/Edit/5 </example>
        /// </summary>
        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Consumo consumo = db.Consumo.Find(id);
            if (consumo == null)
            {
                return HttpNotFound();
            }

            var Dispositivo = db.Dispositivos.Include(d => d.PeriodosFacturacion).Include(d => d.Fuente).Where(d => d.DispositivoID.Equals(consumo.DispositivoID)).FirstOrDefault();
            if (Dispositivo != null)
            {
                if (Dispositivo.PeriodosFacturacion != null)
                    consumo.PeriodoFacturacion = Dispositivo.PeriodosFacturacion.Periodo;

                if (Dispositivo.Fuente != null)
                    consumo.UnidadMedida = Dispositivo.Fuente.UnidadMedida;

                SetViewBagData(Dispositivo.SedeID, Dispositivo.DispositivoID);
            }
            else
            {
                SetViewBagData();
            }

            return View(consumo);
        }

        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Edit Object
        /// <example> POST: Consumoes/Edit/5 </example>
        /// </summary>
        [Authorize(Roles = "BusinessEntity")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SedeID,ConsumoID,DispositivoID,FechaInicial,FechaFinal,LineaBase,PeriodoFacturacion,UnidadMedida,ConsumoPeriodo,Valor,ValorUnitario")] Consumo consumo)
        {
            if (ModelState.IsValid)
            {
                bool hasErrors = false;
                DbContextTransaction Transaction = null;

                try
                {
                    hasErrors = ModelNotIsValid(consumo);

                    if (!hasErrors)
                    {
                        Transaction = db.Database.BeginTransaction();

                        consumo.ValorUnitario = decimal.Round(consumo.ValorUnitario, 2);

                        db.Entry(consumo).State = EntityState.Modified;
                        db.SaveChanges();
                        Transaction.Commit();
                    }
                    else
                    {
                        var Dispositivo = db.Dispositivos.Include(d => d.PeriodosFacturacion).Include(d => d.Fuente).Where(d => d.DispositivoID.Equals(consumo.DispositivoID)).FirstOrDefault();
                        if (Dispositivo != null)
                        {
                            if (Dispositivo.PeriodosFacturacion != null)
                                consumo.PeriodoFacturacion = Dispositivo.PeriodosFacturacion.Periodo;

                            if (Dispositivo.Fuente != null)
                                consumo.UnidadMedida = Dispositivo.Fuente.UnidadMedida;
                        }
                    }
                }
                catch (Exception eX)
                {
                    if (Transaction != null)
                        Transaction.Rollback();

                    ModelState.AddModelError("", eX.Message);
                    hasErrors = true;
                }

                if (!hasErrors)
                    return RedirectToAction("Index");
            }

            SetViewBagData(consumo.SedeID, consumo.DispositivoID);
            return View(consumo);
        }

        /// <summary>
        /// Delete Object From AjaxJQuery
        /// </summary>
        /// <param name="id">PK Value</param>
        /// <returns></returns>        
        public ActionResult Delete(int id)
        {
            try
            {
                Consumo consumo = db.Consumo.Find(id);
                if (consumo != null)
                {
                    db.Consumo.Remove(consumo);
                    db.SaveChanges();
                }

                return new JsonResult
                {
                    Data = new { Message = string.Empty, Success = true },
                    ContentEncoding = System.Text.Encoding.UTF8,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            catch (Exception eX)
            {
                //
                // Log Exception eX
                //

                return new JsonResult
                {
                    Data = new { Message = "Error ejecutando la acción de eliminar. Por favor inténtelo de nuevo", Success = false },
                    ContentEncoding = System.Text.Encoding.UTF8,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        /// <summary>
        /// Obtener los últimos consumos del dispositivo seleccionado.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetLastConsumption(int id)
        {
            try
            {
                StringBuilder Rows = new StringBuilder();
                Rows.Append("<thead><tr><th style=\"width:20%\">Fec.Inicial</th><th style=\"width:20%\">Fec.Final</th><th style=\"width:20%\">Consumo</th><th style=\"width:20%\">Valor</th><th style=\"width:20%\">Valor Unitario</th></tr></thead>");

                decimal LineaBase = 0;
                string Periodo = string.Empty;
                string UnidadMedida = string.Empty;

                var Dispositivo = db.Dispositivos.Include(d => d.PeriodosFacturacion).Include(d => d.Fuente).Where(d => d.DispositivoID.Equals(id)).FirstOrDefault();
                if (Dispositivo != null)
                {
                    LineaBase = Dispositivo.LineaBase;

                    if (Dispositivo.PeriodosFacturacion != null)
                        Periodo = Dispositivo.PeriodosFacturacion.Periodo;

                    if (Dispositivo.Fuente != null)
                        UnidadMedida = Dispositivo.Fuente.UnidadMedida;
                }

                var Consumos = db.Consumo.Where(c => c.DispositivoID.Equals(id)).OrderByDescending(c => c.FechaInicial).Take(5).ToList();
                if (Consumos != null && Consumos.Count > 0)
                {   
                    Consumos.ForEach(c => {
                        Rows.Append($"<tr><td align=\"right\">{c.FechaInicial.ToShortDateString()}</td><td align=\"right\">{c.FechaFinal.ToShortDateString()}</td><td align=\"right\">{c.ConsumoPeriodo.ToString("N2")}</td><td align=\"right\">{c.Valor.ToString("C2")}</td><td align=\"right\">{c.ValorUnitario.ToString("C2")}</td></tr>");
                    });
                }

                return new JsonResult {
                    Data = new { HtmlRow = Rows.ToString(), LinBase = LineaBase, Per = Periodo, Uni = UnidadMedida, Success = true }, ContentEncoding = System.Text.Encoding.UTF8, JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            catch (Exception eX)
            {
                //
                // Log Exception eX
                //

                return new JsonResult
                {
                    Data = new { Message = "Error ejecutando la acción. Por favor inténtelo de nuevo", Success = false },
                    ContentEncoding = System.Text.Encoding.UTF8,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        [HttpPost]
        public ActionResult GetDeviceBySedeId(int sedeID)
        {
            try
            {
                if (sedeID == 999999 || sedeID == -1)
                {
                    var dispositivos = new SelectList( (from dis in db.Dispositivos
                                         join sede in db.Sedes on dis.SedeID equals sede.SedeID
                                         where sede.EmpresaID == Helpers.ApplicationContext.CurrentUser.Empresa.EmpresaID
                                         orderby sede.NombreSede
                                         select dis).ToList(), "DispositivoID", "Nombre", 0);

                    return new JsonResult
                    {

                        Data = new { List = dispositivos, Success = true },
                        ContentEncoding = System.Text.Encoding.UTF8,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }
                else
                {
                    return new JsonResult
                    {
                        Data = new { List = new SelectList(db.Dispositivos.OrderBy(d => d.Nombre).Where(m => m.SedeID.Equals(sedeID)).ToList(), "DispositivoID", "Nombre", 0), Success = true },
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
        public ActionResult GetDeviceBySedeIdAndFuenteId(int empID, int sedeID, int fuenteId)
        {
            try
            {
                List<Dispositivo> SelectListData = null;

                if (empID == 999999 || empID == -1)
                {
                    SelectListData = (from dis in db.Dispositivos
                                        join sede in db.Sedes on dis.SedeID equals sede.SedeID
                                        orderby sede.NombreSede
                                        select dis).ToList();
                }
                else
                {
                    SelectListData = (from dis in db.Dispositivos
                                      join sede in db.Sedes on dis.SedeID equals sede.SedeID
                                      where sede.EmpresaID.Equals(empID)
                                      orderby sede.NombreSede
                                      select dis).ToList();
                }

                if (sedeID != 999999)
                    SelectListData = SelectListData.Where(d => d.SedeID.Equals(sedeID)).ToList();

                if (fuenteId != 999999)
                    SelectListData = SelectListData.Where(d => d.FuenteID.Equals(fuenteId)).ToList();

                return new JsonResult
                {
                    Data = new { List = new SelectList(SelectListData, "DispositivoID", "Nombre", 0), Success = true },
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
        /// Set ViewBag Dispositivos
        /// </summary>
        private void SetViewBagData(int? DefaultSedeID = null, int? DefaultDispositivoID = null)
        {
            object DefSedeId = null;
            object DefDisId = null;

            if (DefaultSedeID.HasValue)
                DefSedeId = DefaultSedeID.Value;

            if (DefaultDispositivoID.HasValue)
                DefDisId = DefaultDispositivoID.Value;

            if (Helpers.ApplicationContext.CurrentUser.IsSuperAdmin)
            {
                ViewBag.DispositivoID = new SelectList(db.Dispositivos.OrderBy(d => d.Nombre), "DispositivoID", "Nombre", DefDisId);
                ViewBag.SedeID = new SelectList(db.Sedes.OrderBy(s => s.NombreSede), "SedeID", "NombreSede", DefSedeId);
            }
            else
            {
                var Dispositivos = (from d in db.Dispositivos
                                    join s in db.Sedes on d.SedeID equals s.SedeID
                                    where s.EmpresaID == Helpers.ApplicationContext.CurrentUser.Empresa.EmpresaID
                                    select d);

                ViewBag.DispositivoID = new SelectList(Dispositivos.OrderBy(d => d.Nombre), "DispositivoID", "Nombre", DefDisId);
                ViewBag.SedeID = new SelectList(db.Sedes.Where(s => s.EmpresaID == Helpers.ApplicationContext.CurrentUser.Empresa.EmpresaID).OrderBy(s => s.NombreSede), "SedeID", "NombreSede", DefSedeId);
            }
        }

        /// <summary>
        /// Validate Input Data
        /// </summary>
        /// <param name="consumo"></param>
        /// <returns></returns>
        private bool ModelNotIsValid(Consumo consumo)
        {
            bool ValidationState = false;

            if (consumo.FechaInicial > consumo.FechaFinal)
            {
                ModelState.AddModelError("", "La fecha inicial no puede ser mayor que la fecha final");
                ValidationState = true;
            }

            if (consumo.Valor < 1)
            {
                ModelState.AddModelError("", "El valor ingresado debe ser superior a cero");
                ValidationState = true;
            }

            if (consumo.ConsumoPeriodo < 1)
            {
                ModelState.AddModelError("", "El consumo ingresado debe ser superior a cero");
                ValidationState = true;
            }

            if (consumo.Valor < consumo.ConsumoPeriodo)
            {
                ModelState.AddModelError("", "El valor debe ser superior al consumo");
                ValidationState = true;
            }

            var DayDiff = (consumo.FechaFinal - consumo.FechaInicial).TotalDays;
            var Dispositvo = db.Dispositivos.Include(d => d.PeriodosFacturacion).Where(d => d.DispositivoID.Equals(consumo.DispositivoID)).FirstOrDefault();
            if (Dispositvo == null)
            {
                ModelState.AddModelError("", "Debe seleccionar un dispositivo");
                ValidationState = true;
            }
            else
            {
                if (Dispositvo.PeriodosFacturacion != null)
                {
                    if (DayDiff > Dispositvo.PeriodosFacturacion.Dias)
                    {
                        ModelState.AddModelError("", "El rango de días entre la fecha inicial y final, no es inferior a los días configurados para el periodo del dispositivo");
                        ValidationState = true;
                    }
                }
            }

            if (consumo.ConsumoPeriodo > 0)
            {
                consumo.ValorUnitario = consumo.Valor / consumo.ConsumoPeriodo;
            }
            else
            {
                consumo.ValorUnitario = 0;
            }

            return ValidationState;
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
