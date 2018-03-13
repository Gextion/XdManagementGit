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
    /// AsesoresController
    /// </summary>
    [NWebsec.Mvc.HttpHeaders.Csp.Csp(Enabled = false)]
    [NWebsec.Mvc.HttpHeaders.Csp.CspScriptSrc(Enabled = false)]
    public class AsesoresController : Controller
    {
        /// <summary>
        /// DataBase Context
        /// </summary>
        private EEContext db = new EEContext();

        /// <summary>
        /// View in List Mode
        /// <example> GET: Asesores </example>
        /// </summary>
        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Index()
        {
            return View(db.Asesors.ToList());
        }

        /// <summary>
        /// View in Detail Mode
        /// <example> GET: Asesores/Details/5 </example>
        /// </summary>
        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Asesor asesor = db.Asesors.Find(id);
            if (asesor == null)
            {
                return HttpNotFound();
            }
            return View(asesor);
        }

        /// <summary>
        /// Action Create New Object
        /// <example> GET: Asesores/Create </example>
        /// </summary>
        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Create()
        {
            return View();
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Action Create New Object
        /// <example> POST: Asesores/Create </example>
        /// </summary>
        [Authorize(Roles = "BusinessEntity")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AsesorID,Codigo,NombreAsesor")] Asesor asesor)
        {
            if (ModelState.IsValid)
            {
                bool hasErrors = false;
                DbContextTransaction Transaction = null;

                try
                {
                    Transaction = db.Database.BeginTransaction();

                    var Other = db.Asesors.Where(a => a.Codigo == asesor.Codigo).Count();
                    if (Other > 0)
                        throw new Exception("El Código ingresado ya se encuentra asociado a otro asesor. Debe ser un valor único.");

                    db.Asesors.Add(asesor);
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

            return View(asesor);
        }

        /// <summary>
        /// Edit Object
        /// <example> GET: Asesores/Edit/5 </example>
        /// </summary>
        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Asesor asesor = db.Asesors.Find(id);
            if (asesor == null)
            {
                return HttpNotFound();
            }
            return View(asesor);
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Edit Object
        /// <example> POST: Asesores/Edit/5 </example>
        /// </summary>
        [Authorize(Roles = "BusinessEntity")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AsesorID,Codigo,NombreAsesor")] Asesor asesor)
        {
            if (ModelState.IsValid)
            {
                bool hasErrors = false;
                DbContextTransaction Transaction = null;

                try
                {
                    Transaction = db.Database.BeginTransaction();

                    db.Entry(asesor).State = EntityState.Modified;
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
            return View(asesor);
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
                Asesor asesor = db.Asesors.Find(id);
                if (asesor != null)
                {
                    db.Asesors.Remove(asesor);
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
