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
    /// SedesController
    /// </summary>
    [NWebsec.Mvc.HttpHeaders.Csp.Csp(Enabled = false)]
    [NWebsec.Mvc.HttpHeaders.Csp.CspScriptSrc(Enabled = false)]
    public class SedesController : Controller
    {
        /// <summary>
        /// DataBase Context
        /// </summary>
        private EEContext db = new EEContext();

        /// <summary>
        /// View in List Mode
        /// <example> GET: Sedes </example>
        /// </summary>
        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Index()
        {
            //TODO:Revisar esto. No es posible generalizar? que pasa si se accede a los otras acciones por la URL
            if (Helpers.ApplicationContext.CurrentUser == null || Helpers.ApplicationContext.CurrentUser.Empresa == null)
                return View("../Authentication/Login");

            if (Helpers.ApplicationContext.CurrentUser.IsSuperAdmin)
            {
                var sedes = db.Sedes.Include(s => s.Ciudad).Include(s => s.Empresa);
                return View(sedes.ToList());
            }
            else
            {
                var sedes = db.Sedes.Include(s => s.Ciudad).Include(s => s.Empresa)
                               .Where(s => s.EmpresaID.Equals(Helpers.ApplicationContext.CurrentUser.Empresa.EmpresaID));
                return View(sedes.ToList());
            }   
        }

        /// <summary>
        /// View in Detail Mode
        /// <example> GET: Sedes/Details/5 </example>
        /// </summary>
        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sede sede = db.Sedes.Find(id);
            if (sede == null)
            {
                return HttpNotFound();
            }
            return View(sede);
        }

        /// <summary>
        /// Action Create New Object
        /// <example> GET: Sedes/Create </example>
        /// </summary>
        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Create()
        {
            SetViewBagListData(null, Helpers.ApplicationContext.CurrentUser.EmpresaID);
            return View();
        }
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Action Create New Object
        /// <example> POST: Sedes/Create </example>
        /// </summary>
        [Authorize(Roles = "BusinessEntity")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SedeID,NombreSede,Responsable,Estrato,CiudadID,Direccion,Telefono,Celular,Email,EmpresaID")] Sede sede)
        {
            if (ModelState.IsValid)
            {
                bool hasErrors = false;
                DbContextTransaction Transaction = null;

                try
                {
                    Transaction = db.Database.BeginTransaction();

                    if (sede.EmpresaID < 1)
                        sede.EmpresaID = Helpers.ApplicationContext.CurrentUser.Empresa.EmpresaID;

                    db.Sedes.Add(sede);
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

            SetViewBagListData(sede.CiudadID, sede.EmpresaID);
            return View(sede);
        }

        /// <summary>
        /// Edit Object
        /// <example> GET: Sedes/Edit/5 </example>
        /// </summary>
        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sede sede = db.Sedes.Find(id);
            if (sede == null)
            {
                return HttpNotFound();
            }

            SetViewBagListData(sede.CiudadID, sede.EmpresaID);
            return View(sede);
        }

        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Edit Object
        /// <example> POST: Sedes/Edit/5 </example>
        /// </summary>
        [Authorize(Roles = "BusinessEntity")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SedeID,NombreSede,Responsable,Estrato,CiudadID,Direccion,Telefono,Celular,Email,EmpresaID")] Sede sede)
        {
            if (ModelState.IsValid)
            {
                bool hasErrors = false;
                DbContextTransaction Transaction = null;

                try
                {
                    Transaction = db.Database.BeginTransaction();

                    if (sede.EmpresaID < 1)
                        sede.EmpresaID = Helpers.ApplicationContext.CurrentUser.Empresa.EmpresaID;

                    db.Entry(sede).State = EntityState.Modified;
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

            SetViewBagListData(sede.CiudadID, sede.EmpresaID);
            return View(sede);
        }

        /// <summary>
        /// Get Company Info
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetCompanyInfo(int? id)
        {
            try
            {
                if (!id.HasValue)
                {
                    return new JsonResult
                    {
                        Data = new { Message = string.Empty, Success = false },
                        ContentEncoding = System.Text.Encoding.UTF8,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }

                Empresa empresa = db.Empresas.Include(e => e.Ciudad).Where(e => e.EmpresaID.Equals(id.Value)).FirstOrDefault();
                if (empresa != null)
                {
                    var CityName = "";
                    var MobileNumber = "";

                    if (empresa.Ciudad != null)
                        CityName = empresa.Ciudad.Ciudad;

                    if (empresa.Celular != null)
                        MobileNumber = empresa.Celular.ToString();

                    return new JsonResult
                    {
                        Data = new {
                            City = CityName,
                            Adrress = empresa.Direccion,
                            Phone = empresa.Telefono.ToString(),
                            Mobile = MobileNumber,
                            Email = empresa.Email,
                            Success = true
                        },
                        ContentEncoding = System.Text.Encoding.UTF8,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }
                else
                {
                    return new JsonResult
                    {
                        Data = new { Message = string.Empty, Success = false },
                        ContentEncoding = System.Text.Encoding.UTF8,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }
            }
            catch
            {
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
        public ActionResult Delete(int id)
        {
            try
            {
                Sede sede = db.Sedes.Include(i => i.Dispositivos).Where(s => s.SedeID.Equals(id)).FirstOrDefault();
                if (sede != null)
                {
                    if (sede.Dispositivos.Count() > 0)
                    {
                        return new JsonResult
                        {
                            Data = new { Message = "La sede no puede eliminarse. Tiene medidores asociados.", Success = false },
                            ContentEncoding = System.Text.Encoding.UTF8, JsonRequestBehavior = JsonRequestBehavior.AllowGet
                        };
                    }
                    else
                    {
                        db.Sedes.Remove(sede);
                        db.SaveChanges();
                    }
                }

                return new JsonResult {
                    Data = new { Message = string.Empty, Success = true }, ContentEncoding = System.Text.Encoding.UTF8, JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            catch
            {
                return new JsonResult
                {
                    Data = new { Message = "Error ejecutando la acción de eliminar. Por favor inténtelo de nuevo", Success = false },
                    ContentEncoding = System.Text.Encoding.UTF8, JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        [HttpPost]
        public ActionResult GetSedesByEmpresaID(int EmpresaID)
        {
            try
            {
                if (EmpresaID == 999999)
                {
                    var dispositivos = new SelectList((from dis in db.Dispositivos
                                                       join sede in db.Sedes on dis.SedeID equals sede.SedeID
                                                       where sede.EmpresaID == Helpers.ApplicationContext.CurrentUser.Empresa.EmpresaID
                                                       orderby sede.NombreSede
                                                       select dis).ToList(), "DispositivoID", "Nombre", 0);

                    return new JsonResult
                    {

                        Data = new { dispositivos, Success = true },
                        ContentEncoding = System.Text.Encoding.UTF8,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }
                else
                {
                    return new JsonResult
                    {
                        Data = new { List = new SelectList(db.Sedes.OrderBy(s => s.NombreSede).Where(s => s.EmpresaID.Equals(EmpresaID)).ToList(), "SedeID", "NombreSede", 0), Success = true },
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

        /// <summary>
        /// Set ViewBag List Data
        /// </summary>
        private void SetViewBagListData(object DefaultCiudad = null, object DefaultEmpresa = null)
        {
            ViewBag.CiudadID = new SelectList(db.Ciudades.OrderBy(c => c.Ciudad), "CiudadID", "Ciudad", DefaultCiudad);
            ViewBag.EmpresaID = new SelectList(db.Empresas.OrderBy(e => e.Nombre), "EmpresaID", "Nombre", DefaultEmpresa);
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
