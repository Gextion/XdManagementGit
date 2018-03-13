using EficienciaEnergetica.Helpers;
using EficienciaEnergetica.Models;
using EficienciaEnergetica.Models.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EficienciaEnergetica.Controllers
{
    /// <summary>
    /// Page Controller
    /// </summary>
    [NWebsec.Mvc.HttpHeaders.Csp.Csp(Enabled = false)]
	[NWebsec.Mvc.HttpHeaders.Csp.CspScriptSrc(Enabled = false)]
	public class AuthenticationController : Controller
	{
        private SecurityDbContext db = new SecurityDbContext();

        /// <summary>
        /// Basic Constructor
        /// </summary>
        public AuthenticationController() : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new SecurityDbContext()) { }))
		{
		}
		/// <summary>
		/// Inti Overload
		/// </summary>
		/// <param name="userManager"></param>
		public AuthenticationController(UserManager<ApplicationUser> userManager)
		{
			UserManager = userManager;
		}

		/// <summary>
		/// User Manager
		/// </summary>
		public UserManager<ApplicationUser> UserManager { get; private set; }

		/// <summary>
		/// Authentication Manager
		/// </summary>
		private IAuthenticationManager AuthenticationManager
		{
			get
			{
				return HttpContext.GetOwinContext().Authentication;
			}
		}

		/// <summary>
		/// Present Login View
		/// </summary>
		/// <returns></returns>
		[AllowAnonymous]
		public ActionResult Login(int? id)
		{
            var DefaultLoginModel = new LoginModel() { ImageEditorID = Guid.NewGuid().ToString() };

            if (!id.HasValue)
            {
                return View(DefaultLoginModel);
            }
            else
            {
                string Code = id.Value.ToString("0000");

                EEContext db = new EEContext();
                var empresa = db.Empresas.Where(e => e.Codigo == Code).FirstOrDefault();
                if (empresa != null)
                {
                    var viewModel = new LoginModel()
                    {
                        HaveEmpresa = true,
                        DisplayLoginCaption = empresa.Nombre,
                        EmpresaLogoURL = string.IsNullOrEmpty(empresa.LogoUrl) ? "/Images/i2e.png" : empresa.LogoUrl,
                        ImageEditorID = Guid.NewGuid().ToString()
                    };

                    return View(viewModel);
                }
                else
                {
                    return View(DefaultLoginModel);
                }
            }
		}

        [AllowAnonymous]
        public ActionResult ForgotPwd()
        {
            return View(new EficienciaEnergetica.Models.ForgotPwdModel());
        }

        [Authorize(Roles = "AccessAll")]
        public ActionResult Audit()
        {
            var audit = db.AuthenticationAudit.Include(e => e.User).OrderBy(o => o.LoginDate);
            return View(audit.ToList());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SendPwd(Models.ForgotPwdModel Model)
        {
            bool HasErrros = false;

            if (ModelState.IsValid)
            {
                var user = UserManager.FindByName(Model.UserName);
                if (user != null)
                {
                    if (string.IsNullOrEmpty(user.Email))
                    {
                        ModelState.AddModelError("", "El usuario no tiene un correo electrónico válido.");
                        HasErrros = true;
                    }
                    else
                    {
                        var newPwd = Guid.NewGuid().ToString().Substring(1, 10);
                        user.PasswordHash = new PasswordHasher().HashPassword(newPwd);
                        var Result = UserManager.Update(user);
                        if (Result.Succeeded)
                        {
                            if (!EmailHelper.SendPwdRecoveryEmail(user.Email, newPwd))
                            {
                                ModelState.AddModelError("", "No fue posible enviar el correo electrónico con la nueva contraseña. Por favor, notifique al administrador de la plataforma.");
                                HasErrros = true;
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "No fue posible modificar la contraseña del usuario. Por favor, intente de nuevo.");
                            HasErrros = true;
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "El usuario no existe.");
                    HasErrros = true;
                }
            }
            else
            {
                ModelState.AddModelError("", "Por favor, intente de nuevo.");
                HasErrros = true;
            }

            if (HasErrros)
            {
                return View("ForgotPwd", Model);
            }
            else
            {
                return View("Login");
            }
        }

        /// <summary>
        /// Action Response To Login Button
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        [HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Authenticate(EficienciaEnergetica.Models.LoginModel Model)
		{
			if (ModelState.IsValid)
			{
				var user = await UserManager.FindAsync(Model.UserName, Model.Password);
				if (user != null)
				{
					await SignInAsync(user, Model.RememberMe);
                    
                    SecurityDbContext db = new SecurityDbContext();
                    db.AuthenticationAudit.Add(new AuthenticationAudit()
                    {
                        LoginDate = DateHelper.GetColombiaDateTime(),
                        LoginIP = Request.UserHostAddress,
                        LoginBrowser = Request.UserAgent,
                        LoginPlatform = Request.Browser.Platform,
                        LoginBrowserVersion = Request.Browser.Version,
                        UserId = user.Id
                    });
                    db.SaveChanges();

                    return RedirectToAction("Dashboard", "Dashboards");
				}
				else
				{
					ModelState.AddModelError("", "Usuario y/o contraseña incorrectos.");
				}
			}

			return View("Login", Model);
		}

		/// <summary>
		/// Sign In
		/// </summary>
		/// <param name="user"></param>
		/// <param name="isPersistent"></param>
		/// <returns></returns>
		private async Task SignInAsync(ApplicationUser user, bool isPersistent)
		{
			AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
			var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
			AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
		}

		/// <summary>
		/// Log Off
		/// </summary>
		/// <returns></returns>
		public ActionResult LogOff()
		{
			AuthenticationManager.SignOut();
			return RedirectToAction("Index", "Dashboards");
		}
	}
}