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
    /// SectoresEconomicosController
    /// </summary>
    [NWebsec.Mvc.HttpHeaders.Csp.Csp(Enabled = false)]
    [NWebsec.Mvc.HttpHeaders.Csp.CspScriptSrc(Enabled = false)]
    public class SectoresEconomicosController : Controller
    {
        /// <summary>
        /// DataBase Context
        /// </summary>
        private EEContext db = new EEContext();

        /// <summary>
        /// View in List Mode
        /// <example> GET: SectoresEconomicos </example>
        /// </summary>
        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Index()
        {
            return View(db.SectoresEconomicos.ToList());
        }

        /// <summary>
        /// View in Detail Mode
        /// <example> GET: SectoresEconomicos/Details/5 </example>
        /// </summary>
        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SectoresEconomicos sectoresEconomicos = db.SectoresEconomicos.Find(id);
            if (sectoresEconomicos == null)
            {
                return HttpNotFound();
            }
            return View(sectoresEconomicos);
        }

        /// <summary>
        /// Action Create New Object
        /// <example> GET: SectoresEconomicos/Create </example>
        /// </summary>
        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Create()
        {
            return View();
        }

        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Action Create New Object
        /// <example> POST: SectoresEconomicos/Create </example>
        /// </summary>
        [Authorize(Roles = "BusinessEntity")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SectorEconomicoID,SectorEconomico")] SectoresEconomicos sectoresEconomicos)
        {
            if (ModelState.IsValid)
            {
                bool hasErrors = false;
                DbContextTransaction Transaction = null;

                try
                {
                    Transaction = db.Database.BeginTransaction();

                    db.SectoresEconomicos.Add(sectoresEconomicos);
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

            return View(sectoresEconomicos);
        }

        /// <summary>
        /// Edit Object
        /// <example> GET: SectoresEconomicos/Edit/5 </example>
        /// </summary>
        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SectoresEconomicos sectoresEconomicos = db.SectoresEconomicos.Find(id);
            if (sectoresEconomicos == null)
            {
                return HttpNotFound();
            }
            return View(sectoresEconomicos);
        }

        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Edit Object
        /// <example> POST: SectoresEconomicos/Edit/5 </example>
        /// </summary>
        [Authorize(Roles = "BusinessEntity")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SectorEconomicoID,SectorEconomico")] SectoresEconomicos sectoresEconomicos)
        {
            if (ModelState.IsValid)
            {
                bool hasErrors = false;
                DbContextTransaction Transaction = null;

                try
                {
                    Transaction = db.Database.BeginTransaction();

                    db.Entry(sectoresEconomicos).State = EntityState.Modified;
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
            return View(sectoresEconomicos);
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
                SectoresEconomicos sectoresEconomicos = db.SectoresEconomicos.Find(id);
                if (sectoresEconomicos != null)
                {
                    db.SectoresEconomicos.Remove(sectoresEconomicos);
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
