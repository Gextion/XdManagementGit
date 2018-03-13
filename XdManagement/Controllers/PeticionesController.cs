using EficienciaEnergetica.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace EficienciaEnergetica.Controllers
{
    /// <summary>
    /// PeticionsController
    /// </summary>
    [NWebsec.Mvc.HttpHeaders.Csp.Csp(Enabled = false)]
    [NWebsec.Mvc.HttpHeaders.Csp.CspScriptSrc(Enabled = false)]
    public class PeticionesController : Controller
    {
        /// <summary>
        /// DataBase Context
        /// </summary>
        private EEContext db = new EEContext();

        /// <summary>
        /// View in List Mode
        /// <example> GET: Peticions </example>
        /// </summary>
        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Index()
        {
            var peticións = db.Peticións.Include(p => p.Empresa);
            return View(peticións.ToList());
        }

        /// <summary>
        /// View in Detail Mode
        /// <example> GET: Peticions/Details/5 </example>
        /// </summary>
        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Peticion peticion = db.Peticións.Find(id);
            if (peticion == null)
            {
                return HttpNotFound();
            }
            return View(peticion);
        }

        /// <summary>
        /// Action Create New Object
        /// <example> GET: Peticions/Create </example>
        /// </summary>
        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Create()
        {
            Peticion ViewMode = new Peticion() { FechaRegistro = DateTime.Today };

            SetViewBagListData(Helpers.ApplicationContext.CurrentUser.EmpresaID);
            
            return View(ViewMode);
        }

        /// <summary>
        /// Action Create New Object
        /// <example> POST: Peticions/Create </example>
        /// </summary>
        [Authorize(Roles = "BusinessEntity")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PeticionID,FechaRegistro,Titulo,Descripcion,TipoPeticion,FechaSolucion,Solucion,ResueltaPor,UserID,EmpresaID")] Peticion peticion)
        {
            if (ModelState.IsValid)
            {
                bool hasErrors = false;
                DbContextTransaction Transaction = null;

                try
                {
                    if (!Helpers.ApplicationContext.CurrentUser.IsSuperAdmin)
                        peticion.EmpresaID = Helpers.ApplicationContext.CurrentUser.EmpresaID;

                        Transaction = db.Database.BeginTransaction();

                    db.Peticións.Add(peticion);
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

            SetViewBagListData(peticion.EmpresaID);

            return View(peticion);
        }

        /// <summary>
        /// Edit Object
        /// <example> GET: Peticions/Edit/5 </example>
        /// </summary>
        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Peticion peticion = db.Peticións.Find(id);
            if (peticion == null)
            {
                return HttpNotFound();
            }

            SetViewBagListData(peticion.EmpresaID);

            return View(peticion);
        }

        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Edit Object
        /// <example> POST: Peticions/Edit/5 </example>
        /// </summary>
        [Authorize(Roles = "BusinessEntity")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PeticionID,FechaRegistro,Titulo,Descripcion,TipoPeticion,FechaSolucion,Solucion,ResueltaPor,UserID,EmpresaID")] Peticion peticion)
        {
            if (ModelState.IsValid)
            {
                bool hasErrors = false;
                DbContextTransaction Transaction = null;

                try
                {
                    if (!Helpers.ApplicationContext.CurrentUser.IsSuperAdmin)
                        peticion.EmpresaID = Helpers.ApplicationContext.CurrentUser.EmpresaID;

                    Transaction = db.Database.BeginTransaction();

                    db.Entry(peticion).State = EntityState.Modified;
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

            SetViewBagListData(peticion.EmpresaID);

            return View(peticion);
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
                Peticion peticion = db.Peticións.Find(id);
                if (peticion != null)
                {
                    db.Peticións.Remove(peticion);
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
        /// Set ViewBag ListData
        /// </summary>
        private void SetViewBagListData(object DefaultEmpresaID = null)
        {
            if (Helpers.ApplicationContext.CurrentUser.IsSuperAdmin)
            {
                ViewBag.EmpresaID = new SelectList(db.Empresas.OrderBy(e => e.Nombre), "EmpresaID", "Nombre", DefaultEmpresaID);
            }
            else
            {
                ViewBag.EmpresaID = new SelectList(db.Empresas.Where(e => e.EmpresaID == Helpers.ApplicationContext.CurrentUser.EmpresaID).OrderBy(e => e.Nombre), "EmpresaID", "Nombre", DefaultEmpresaID);
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
