using EficienciaEnergetica.Models;
using EficienciaEnergetica.Models.Security;
using System;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace EficienciaEnergetica.Controllers
{
    /// <summary>
    /// EmpresasController
    /// </summary>
    [NWebsec.Mvc.HttpHeaders.Csp.Csp(Enabled = false)]
    [NWebsec.Mvc.HttpHeaders.Csp.CspScriptSrc(Enabled = false)]
    public class EmpresasController : Controller
    {
        /// <summary>
        /// DataBase Context
        /// </summary>
        private EEContext db = new EEContext();

        /// <summary>
        /// View in List Mode
        /// <example> GET: Empresas </example>
        /// </summary>
        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Index()
        {
            if (Helpers.ApplicationContext.CurrentUser == null || Helpers.ApplicationContext.CurrentUser.Empresa == null)
                return View("../Authentication/Login");

            if (Helpers.ApplicationContext.CurrentUser.IsSuperAdmin)
            {
                var empresas = db.Empresas.Include(e => e.Ciudad).Include(e => e.SectorEconomico);
                return View(empresas.ToList());
            }
            else
            {
                var empresas = db.Empresas.Include(e => e.Ciudad).Include(e => e.SectorEconomico).Where(e => e.EmpresaID.Equals(Helpers.ApplicationContext.CurrentUser.Empresa.EmpresaID));
                return View(empresas.ToList());
            }
        }

        /// <summary>
        /// View in Detail Mode
        /// <example> GET: Empresas/Details/5 </example>
        /// </summary>
        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Empresa empresa = db.Empresas.Find(id);
            if (empresa == null)
            {
                return HttpNotFound();
            }
            return View(empresa);
        }

        /// <summary>
        /// Action Create New Object
        /// <example> GET: Empresas/Create </example>
        /// </summary>
        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Create()
        {
            ViewBag.CiudadID = new SelectList(db.Ciudades, "CiudadID", "Ciudad");
            ViewBag.SectorEconomicoID = new SelectList(db.SectoresEconomicos, "SectorEconomicoID", "SectorEconomico");
            return View();
        }

        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Action Create New Object
        /// <example> POST: Empresas/Create </example>
        /// </summary>
        [Authorize(Roles = "BusinessEntity")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Exclude = "EmpresaPhoto", Include = "EmpresaID,Codigo,Nit,Nombre,RazonSocial,RepresentanteLegal,CiudadID,SectorEconomicoID,Direccion,Telefono,Celular,Email,SitioWeb,LogoUrl")] Empresa empresa)
        {
            if (ModelState.IsValid)
            {
                bool hasErrors = false;
                
                try
                {
                    #region System User Validations

                    if (empresa.Codigo.ToString().Length != 4)
                        throw new Exception("El Código debe tener 4 caracteres.");

                    var CountCode = db.Empresas.Where(e => e.Codigo.Equals(empresa.Codigo)).Count();
                    if (CountCode > 0)
                        throw new Exception("El Código ingresado ya se encuentra asociado a otra empresa. Debe ser un valor único.");

                    if (empresa.Nit.ToString().Length < 5)
                        throw new Exception("El Nit debe tener al menos 5 caracteres.");

                    var CountNit = db.Empresas.Where(e => e.Nit.Equals(empresa.Nit)).Count();
                    if (CountNit > 0)
                        throw new Exception("El Nit ingresado ya se encuentra asociado a otra empresa. Debe ser un valor único.");
                    #endregion

                    if (string.IsNullOrEmpty(empresa.Email))
                        throw new Exception("El email es un valor requerido.");

                    var CountEmails = db.Empresas.Where(e => e.Email.ToLower().Trim().Equals(empresa.Email.ToLower().Trim())).Count();
                    if (CountEmails > 0)
                        throw new Exception("El Email ingresado ya se encuentra asociado a otra empresa. Debe ser un valor único.");

                    string LogoUrl = GetEmpresaLogoUrl(empresa.LogoUrl);
                    if (!string.IsNullOrEmpty(LogoUrl))
                        empresa.LogoUrl = LogoUrl;

                    db.Empresas.Add(empresa);
                    db.SaveChanges();
                    
                    //Create System User
                    var ResultUsr = ApplicationUser.AddUser(empresa.Codigo.ToString(), empresa.Nit.ToString(), empresa.Nombre, empresa.Nit.ToString(), empresa.Email, empresa.EmpresaID);
                    if (!ResultUsr.Succeeded)
                    {
                        ModelState.AddModelError("", String.Join(", ", ResultUsr.Errors.Select(u => u.ToString())));
                        hasErrors = true;
                    }
                }
                catch (Exception eX)
                {
                    ModelState.AddModelError("", eX.Message);
                    hasErrors = true;
                }
                if (!hasErrors)
                    return RedirectToAction("Index");
            }

            ViewBag.CiudadID = new SelectList(db.Ciudades, "CiudadID", "Ciudad", empresa.CiudadID);
            ViewBag.SectorEconomicoID = new SelectList(db.SectoresEconomicos, "SectorEconomicoID", "SectorEconomico", empresa.SectorEconomicoID);
            return View(empresa);
        }

        /// <summary>
        /// Edit Object
        /// <example> GET: Empresas/Edit/5 </example>
        /// </summary>
        [Authorize(Roles = "BusinessEntity")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Empresa empresa = db.Empresas.Find(id);
            if (empresa == null)
            {
                return HttpNotFound();
            }
            ViewBag.CiudadID = new SelectList(db.Ciudades, "CiudadID", "Ciudad", empresa.CiudadID);
            ViewBag.SectorEconomicoID = new SelectList(db.SectoresEconomicos, "SectorEconomicoID", "SectorEconomico", empresa.SectorEconomicoID);
            return View(empresa);
        }

        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Edit Object
        /// <example> POST: Empresas/Edit/5 </example>
        /// </summary>
        [Authorize(Roles = "BusinessEntity")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Exclude = "EmpresaPhoto", Include = "EmpresaID,Codigo,Nit,Nombre,RazonSocial,RepresentanteLegal,CiudadID,SectorEconomicoID,Direccion,Telefono,Celular,Email,SitioWeb,LogoUrl")] Empresa empresa)
        {
            if (ModelState.IsValid)
            {
                bool hasErrors = false;
                DbContextTransaction Transaction = null;

                try
                {
                    Transaction = db.Database.BeginTransaction();

                    string LogoUrl = GetEmpresaLogoUrl(empresa.LogoUrl);
                    if (!string.IsNullOrEmpty(LogoUrl))
                        empresa.LogoUrl = LogoUrl;

                    db.Entry(empresa).State = EntityState.Modified;
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
            ViewBag.CiudadID = new SelectList(db.Ciudades, "CiudadID", "Ciudad", empresa.CiudadID);
            ViewBag.SectorEconomicoID = new SelectList(db.SectoresEconomicos, "SectorEconomicoID", "SectorEconomico", empresa.SectorEconomicoID);
            return View(empresa);
        }

        /// <summary>
        /// Delete Object From AjaxJQuery
        /// </summary>
        /// <param name="id">PK Value</param>
        /// <returns></returns>        
        public ActionResult Delete(int id)
        {
            DbContextTransaction Transaction = null;
            //DbContextTransaction SecurityTransaction = null;

            try
            {
                Empresa empresa = db.Empresas.Include(e => e.Sedes).Include(e => e.Peticións).Where(e => e.EmpresaID.Equals(id)).FirstOrDefault();
                if (empresa != null)
                {
                    if (empresa.Sedes != null && empresa.Sedes.Count > 0)
                    {
                        return new JsonResult
                        {
                            Data = new { Message = "No es posible eliminar una empresa que tiene sedes asociadas.", Success = false },
                            ContentEncoding = System.Text.Encoding.UTF8,
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet
                        };
                    }

                    if (empresa.Peticións != null && empresa.Peticións.Count > 0)
                    {
                        return new JsonResult
                        {
                            Data = new { Message = "No es posible eliminar una empresa que tiene peticiones asociadas.", Success = false },
                            ContentEncoding = System.Text.Encoding.UTF8,
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet
                        };
                    }

                    Transaction = db.Database.BeginTransaction();

                    SecurityDbContext dbSecurity = new SecurityDbContext();
                    //SecurityTransaction = dbSecurity.Database.BeginTransaction();

                    var UsersToDelete = dbSecurity.Users.Include(u => u.Groups).Where(u => u.EmpresaID == empresa.EmpresaID).ToList();
                    if (UsersToDelete != null && UsersToDelete.Count > 0)
                    {
                        for (int i = 0; i < UsersToDelete.Count; i++)
                        {
                            var ResultUsr = ApplicationUser.ClearUserGroups(UsersToDelete[i]);
                            if (!ResultUsr.Succeeded)
                            {
                                return new JsonResult
                                {
                                    Data = new { Message = String.Join(", ", ResultUsr.Errors.Select(u => u.ToString())), Success = false },
                                    ContentEncoding = System.Text.Encoding.UTF8,
                                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                                };
                            }
                            else
                            {
                                //ApplicationUser editUserViewModel = dbSecurity.Users.Find(UsersToDelete[i].Id);
                                dbSecurity.Users.Remove(UsersToDelete[i]);
                            }
                        }

                        dbSecurity.SaveChanges();
                    }
                    
                    db.Empresas.Remove(empresa);
                    db.SaveChanges();

                    Transaction.Commit();
                    //SecurityTransaction.Commit();
                }
                else
                {
                    return new JsonResult
                    {
                        Data = new { Message = "No es posible identificar la empresa. Por favor, intente de nuevo.", Success = false },
                        ContentEncoding = System.Text.Encoding.UTF8,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }

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

                //if (SecurityTransaction != null)
                //    SecurityTransaction.Rollback();

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
        /// Get Empresa Logo Url in Request
        /// </summary>
        /// <returns></returns>
        private string GetEmpresaLogoUrl(string CurrentName)
        {
            if (Request.Files != null && Request.Files.Count > 0)
            {
                var Inputfile = Request.Files["EmpresaPhoto"];
                if (Inputfile != null && Inputfile.ContentLength > 0)
                {
                    var fileExtension = Path.GetFileName(Inputfile.FileName.Substring(Inputfile.FileName.LastIndexOf('.')));
                    string RelativePath = $"/Uploads/Logos/Empresas/{Guid.NewGuid().ToString()}{fileExtension}";

                    if (!String.IsNullOrEmpty(CurrentName))
                    {
                        try
                        {
                            if (System.IO.File.Exists(CurrentName))
                                System.IO.File.Delete(CurrentName);
                        }
                        catch { }
                    }

                    var path = Server.MapPath($"~{RelativePath}");
                    Inputfile.SaveAs(path);

                    return $"{Request.Url.GetLeftPart(UriPartial.Authority)}{RelativePath}";
                }
            }

            return string.Empty;
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
