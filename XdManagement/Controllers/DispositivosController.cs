using EficienciaEnergetica.Models;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace EficienciaEnergetica.Controllers
{
    /// <summary>
    /// DispositivosController
    /// </summary>
    [NWebsec.Mvc.HttpHeaders.Csp.Csp(Enabled = false)]
    [NWebsec.Mvc.HttpHeaders.Csp.CspScriptSrc(Enabled = false)]
    public class DispositivosController : Controller
    {
        /// <summary>
        /// DataBase Context
        /// </summary>
        private EEContext db = new EEContext();

        /// <summary>
        /// View in List Mode
        /// <example> GET: Dispositivos </example>
        /// </summary>
        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Index()
        {
            if (Helpers.ApplicationContext.CurrentUser == null || Helpers.ApplicationContext.CurrentUser.Empresa == null)
                return View("../Authentication/Login");

            ViewBag.Caption = "Medidores";

            if (Helpers.ApplicationContext.CurrentUser.IsSuperAdmin)
            {
                var dispositivos = db.Dispositivos.Include(d => d.Fuente).Include(d => d.PeriodosFacturacion).Include(d => d.Sede);
                return View(dispositivos.ToList());
            }
            else
            {
                var dispositivos = (from dis in db.Dispositivos.Include(d => d.Fuente).Include(d => d.PeriodosFacturacion).Include(d => d.Sede)
                                    join sede in db.Sedes on dis.SedeID equals sede.SedeID
                                    where sede.EmpresaID == Helpers.ApplicationContext.CurrentUser.Empresa.EmpresaID
                                    select dis);

                return View(dispositivos.ToList());
            }
        }

        /// <summary>
        /// View in List Mode
        /// <example> GET: Dispositivos </example>
        /// <param name="idSede"></param>
        /// </summary>    
        [Authorize(Roles = "BusinessEntity")]
        public ActionResult IndexFilterBySede(int? idSede)
        {
            if (idSede != null && idSede.HasValue)
            {
                var Sede = db.Sedes.Where(s => s.SedeID.Equals(idSede.Value)).FirstOrDefault();
                if (Sede != null)
                {
                    ViewBag.Caption = $"Dispositivos de {Sede.NombreSede}";
                }
                else
                {
                    ViewBag.Caption = "Dispositivos de la Sede";
                }

                var dispositivos = db.Dispositivos.Include(d => d.Fuente).Include(d => d.PeriodosFacturacion).Include(d => d.Sede).Where( d => d.SedeID.Equals(idSede.Value));
                return View("Index", dispositivos.ToList());
            }
            else
            {
                return Index();
            }
        }

        /// <summary>
        /// View in Detail Mode
        /// <example> GET: Dispositivos/Details/5 </example>
        /// </summary>
        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dispositivo dispositivo = db.Dispositivos.Find(id);
            if (dispositivo == null)
            {
                return HttpNotFound();
            }
            return View(dispositivo);
        }

        /// <summary>
        /// Action Create New Object
        /// <example> GET: Dispositivos/Create </example>
        /// </summary>
        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Create()
        {
            SetViewBagListData();
            return View();
        }
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Action Create New Object
        /// <example> POST: Dispositivos/Create </example>
        /// </summary>
        [Authorize(Roles = "BusinessEntity")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DispositivoID,Nombre,SedeID,LineaBase,FuenteID,PeriodoFacturacionID")] Dispositivo dispositivo)
        {
            if (ModelState.IsValid)
            {
                bool hasErrors = false;
                DbContextTransaction Transaction = null;

                try
                {
                    Transaction = db.Database.BeginTransaction();

                    db.Dispositivos.Add(dispositivo);
                    db.SaveChanges();

                    Transaction.Commit();
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

            SetViewBagListData(dispositivo.FuenteID, dispositivo.PeriodoFacturacionID, dispositivo.SedeID);
            return View(dispositivo);
        }

        /// <summary>
        /// Edit Object
        /// <example> GET: Dispositivos/Edit/5 </example>
        /// </summary>
        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Dispositivo dispositivo = db.Dispositivos.Find(id);
            if (dispositivo == null)
                return HttpNotFound();

            SetViewBagListData(dispositivo.FuenteID, dispositivo.PeriodoFacturacionID, dispositivo.SedeID);
            return View(dispositivo);
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Edit Object
        /// <example> POST: Dispositivos/Edit/5 </example>
        /// </summary>
        [Authorize(Roles = "BusinessEntity")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DispositivoID,Nombre,SedeID,LineaBase,FuenteID,PeriodoFacturacionID")] Dispositivo dispositivo)
        {
            if (ModelState.IsValid)
            {
                bool hasErrors = false;
                DbContextTransaction Transaction = null;

                try
                {
                    Transaction = db.Database.BeginTransaction();

                    db.Entry(dispositivo).State = EntityState.Modified;
                    db.SaveChanges();
                    Transaction.Commit();
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

            SetViewBagListData(dispositivo.FuenteID, dispositivo.PeriodoFacturacionID, dispositivo.SedeID);
            return View(dispositivo);
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
                Dispositivo dispositivo = db.Dispositivos.Include(d => d.Consumos).Where(d => d.DispositivoID.Equals(id)).FirstOrDefault();
                if (dispositivo != null)
                {
                    if (dispositivo.Consumos != null && dispositivo.Consumos.Count > 0)
                    {
                        return new JsonResult
                        {
                            Data = new { Message = "No es posible eliminar un dispositivo que tiene consumos asociados.", Success = false },
                            ContentEncoding = System.Text.Encoding.UTF8,
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet
                        };
                    }

                    db.Dispositivos.Remove(dispositivo);
                    db.SaveChanges();

                    return new JsonResult
                    {
                        Data = new { Message = string.Empty, Success = true },
                        ContentEncoding = System.Text.Encoding.UTF8,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }
                else
                {
                    return new JsonResult
                    {
                        Data = new { Message = "No es posible identificar el dispositivo. Por favor, intente de nuevo.", Success = false },
                        ContentEncoding = System.Text.Encoding.UTF8,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }
                
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
        /// Set ViewBag SedeId
        /// </summary>
        private void SetViewBagListData(object DefaultFuente = null, object DefaultPeriodo = null, object DefaultSede = null)
        {
            var Fuentes = (from fu in db.FuenteEnergeticas select fu).AsEnumerable()
                           .Select(x => new {
                               FuenteID = x.FuenteID,
                               Fuente = $"{x.Fuente} - {x.UnidadMedida}"
                           }).ToList().OrderBy(f => f.Fuente);

            ViewBag.FuenteID = new SelectList(Fuentes, "FuenteID", "Fuente", DefaultFuente);
            ViewBag.PeriodoFacturacionID = new SelectList(db.PeriodoFacturacions.OrderBy(p => p.Periodo), "PeriodoFacturacionID", "Periodo", DefaultPeriodo);

            if (Helpers.ApplicationContext.CurrentUser.IsSuperAdmin)
            {
                ViewBag.SedeID = new SelectList(db.Sedes.OrderBy(s => s.NombreSede), "SedeID", "NombreSede", DefaultSede);
            }
            else
            {
                ViewBag.SedeID = new SelectList(db.Sedes.Where(s => s.EmpresaID == Helpers.ApplicationContext.CurrentUser.Empresa.EmpresaID).OrderBy(s => s.NombreSede), "SedeID", "NombreSede", DefaultSede);
            }
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
    