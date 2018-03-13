using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EficienciaEnergetica.Models;

namespace EficienciaEnergetica.Controllers
{
    /// <summary>
    /// AccionAhorroController
    /// </summary>
    [NWebsec.Mvc.HttpHeaders.Csp.Csp(Enabled = false)]
    [NWebsec.Mvc.HttpHeaders.Csp.CspScriptSrc(Enabled = false)]
    public class AccionAhorroController : Controller
    {
        /// <summary>
        /// DataBase Context
        /// </summary>
        private EEContext db = new EEContext();

        /// <summary>
        /// View in List Mode
        /// <example> GET: AccionAhorro </example>
        /// </summary>
        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Index()
        {
            var equipos = db.Equipos.Include(e => e.TipoEquipo);
            return View(db.AccionAhorro.ToList());
        }

        /// <summary>
        /// View in Detail Mode
        /// <example> GET: AccionAhorro/Details/5 </example>
        /// </summary>
        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccionAhorro accionAhorro = db.AccionAhorro.Find(id);
            if (accionAhorro == null)
            {
                return HttpNotFound();
            }
            return View(accionAhorro);
        }

        /// <summary>
        /// Action Create New Object
        /// <example> GET: AccionAhorro/Create </example>
        /// </summary>
        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Create()
        {
            ViewBag.TipoEquipoID = new SelectList(db.TiposEquipos, "TipoEquipoID", "NombreTipoEquipo");
            return View();
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Action Create New Object
        /// <example> POST: AccionAhorro/Create </example>
        /// </summary>
        [Authorize(Roles = "BusinessEntity")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AccionAhorroID,DescripcionAccionAhorro,TipoEquipoID")] AccionAhorro accionAhorro)
        {
            if (ModelState.IsValid)
            {
                bool hasErrors = false;
                DbContextTransaction Transaction = null;

                try
                {
                    Transaction = db.Database.BeginTransaction();

                    db.AccionAhorro.Add(accionAhorro);
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

            ViewBag.TipoEquipoID = new SelectList(db.TiposEquipos, "TipoEquipoID", "NombreTipoEquipo", accionAhorro.TipoEquipoID);
            return View(accionAhorro);
        }

        /// <summary>
        /// Edit Object
        /// <example> GET: AccionAhorro/Edit/5 </example>
        /// </summary>
        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccionAhorro accionAhorro = db.AccionAhorro.Find(id);
            if (accionAhorro == null)
            {
                return HttpNotFound();
            }
            ViewBag.TipoEquipoID = new SelectList(db.TiposEquipos, "TipoEquipoID", "NombreTipoEquipo", accionAhorro.TipoEquipoID);
            return View(accionAhorro);
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Edit Object
        /// <example> POST: AccionAhorro/Edit/5 </example>
        /// </summary>
        [Authorize(Roles = "BusinessEntity")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AccionAhorroID,DescripcionAccionAhorro,TipoEquipoID")] AccionAhorro accionAhorro)
        {
            if (ModelState.IsValid)
            {
                bool hasErrors = false;
                DbContextTransaction Transaction = null;

                try
                {
                    Transaction = db.Database.BeginTransaction();

                    db.Entry(accionAhorro).State = EntityState.Modified;
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
            ViewBag.TipoEquipoID = new SelectList(db.TiposEquipos, "TipoEquipoID", "NombreTipoEquipo", accionAhorro.TipoEquipoID);
            return View(accionAhorro);
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
                AccionAhorro accionAhorro = db.AccionAhorro.Find(id);
                if (accionAhorro != null)
                {
                    db.AccionAhorro.Remove(accionAhorro);
                    db.SaveChanges();
                }

                return new JsonResult {
                    Data = new { Message = string.Empty, Success = true }, ContentEncoding = System.Text.Encoding.UTF8, JsonRequestBehavior = JsonRequestBehavior.AllowGet
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
                    ContentEncoding = System.Text.Encoding.UTF8, JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
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
