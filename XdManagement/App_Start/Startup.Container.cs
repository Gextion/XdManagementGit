namespace EficienciaEnergetica
{
    using Autofac;
    using Autofac.Integration.Mvc;
    using EficienciaEnergetica.Services;
    using Microsoft.AspNet.Identity;
    using Microsoft.Owin;
    using Microsoft.Owin.Security.Cookies;
    using Models.Security;
    using Owin;
    using System;
    using System.Configuration;
    using System.Data.Entity;
    using System.Data.Entity.Validation;
    using System.Linq;
    using System.Reflection;
    using System.Web.Mvc;
    using System.Web.Services.Description;

    /// <summary>
    /// Register types into the Autofac Inversion of Control (IOC) container. Autofac makes it easy to register common 
    /// MVC types like the <see cref="UrlHelper"/> using the <see cref="AutofacWebTypesModule"/>. Feel free to change 
    /// this to another IoC container of your choice but ensure that common MVC types like <see cref="UrlHelper"/> are 
    /// registered. See http://autofac.readthedocs.org/en/latest/integration/aspnet.html.
    /// </summary>
    public partial class Startup
    {
        private const string InitialUserName = "";
        private const string InitialUserPwd = "";

        public const string SuperAdminName = "SuperAdmin";
        public const string AdminName = "Admin";
        public const string FinalUserName = "Usuario";

        private static readonly SecurityDbContext _db = new SecurityDbContext();
        private static readonly IdentityManager _idManager = new IdentityManager();

        private readonly static string[] _initialGroupNames = { SuperAdminName, AdminName, FinalUserName };

        private readonly static string[] _SuperAdminRoleNames = { "AccessAll", "UserManagement", "BusinessEntity", "Companies" };
        private readonly static string[] _AdminRoleNames = { "BusinessEntity", "UserManagement" };
        private readonly static string[] _FinalUserName = { "BusinessEntity" };
        
        public static void ConfigureContainer(IAppBuilder app)
        {
            IContainer container = CreateContainer();
            app.UseAutofacMiddleware(container);

            // Register MVC Types 
            app.UseAutofacMvc();

            // Enable the application to use a cookie to store information for the signed in user
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                ExpireTimeSpan = TimeSpan.FromHours(12),
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Authentication/Login")
            });
            // Use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            if (bool.Parse(ConfigurationManager.AppSettings["UpdateDataBase"].ToString()))
            {
                AddGroups();
                AddRoles();
                AddUsers();
                AddRolesToGroups();
                AddUsersToGroups();
            }
        }


        public static void AddGroups()
        {
            foreach (var groupName in _initialGroupNames)
            {
                try
                {
                    _idManager.CreateGroup(groupName);
                }
                catch (GroupExistsException)
                {
                    // intentionally catched for seeding
                }
            }
        }

        /// <summary>
        /// Add Roels
        /// </summary>
        private static void AddRoles()
        {
            _idManager.CreateRole("AccessAll", "Acceso Global");
            _idManager.CreateRole("UserManagement", "Acceso a Entidades de Usuarios");
            _idManager.CreateRole("Companies", "Acceso a Maestro de Empresas");
            _idManager.CreateRole("BusinessEntity", "Acceso a Entidades del Negocio");
        }

        /// <summary>
        /// Add Users
        /// </summary>
        private static void AddUsers()
        {
            //TODO:Revisar esto
            var user = new ApplicationUser();
            user.UserName = InitialUserName;
            user.FirstName = "";
            user.LastName = "";
            user.Email = "";
            user.EmpresaID = int.Parse(ConfigurationManager.AppSettings["SuperUsrEmpresaId"]);
            
            var chkUser = _idManager.CreateUser(user, InitialUserPwd);

            //Add default User to Role Admin   
            if (chkUser != null && !chkUser.Succeeded)
                throw new DbEntityValidationException("Could not create InitialUser because: " + String.Join(", ", chkUser.Errors));
        }

        /// <summary>
        /// Add Role To Group
        /// </summary>
        private static void AddRolesToGroups()
        {
            IDbSet<Group> allGroups = _db.Groups;
            Group superAdmins = allGroups.First(g => g.Name.Equals(SuperAdminName));
            foreach (string name in _SuperAdminRoleNames)
            {
                _idManager.AddRoleToGroup(superAdmins.Id, name);
            }

            Group groupAdmins = _db.Groups.First(g => g.Name.Equals(AdminName));
            foreach (string name in _AdminRoleNames)
            {
                _idManager.AddRoleToGroup(groupAdmins.Id, name);
            }

            Group groupFinalUsers = _db.Groups.First(g => g.Name.Equals(FinalUserName));
            foreach (string name in _FinalUserName)
            {
                _idManager.AddRoleToGroup(groupFinalUsers.Id, name);
            }
        }

        /// <summary>
        /// Add User To Group
        /// </summary>
        private static void AddUsersToGroups()
        {
            ApplicationUser user = _db.Users.First(u => u.UserName == InitialUserName);
            IDbSet<Group> allGroups = _db.Groups;
            foreach (Group group in allGroups)
            {
                _idManager.AddUserToGroup(user.Id, group.Id);
            }
        }

        /// <summary>
        /// Create Container To Register Assembly
        /// </summary>
        /// <returns></returns>
        private static IContainer CreateContainer()
        {
            ContainerBuilder builder = new ContainerBuilder();
            Assembly assembly = Assembly.GetExecutingAssembly();

            RegisterServices(builder);
            RegisterMvc(builder, assembly);

            IContainer container = builder.Build();

            SetMvcDependencyResolver(container);

            return container;
        }

        /// <summary>
        /// Register Service
        /// </summary>
        /// <param name="builder"></param>
        private static void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterType<BrowserConfigService>().As<IBrowserConfigService>().InstancePerRequest();
            builder.RegisterType<CacheService>().As<ICacheService>().SingleInstance();
            builder.RegisterType<FeedService>().As<IFeedService>().InstancePerRequest();
            builder.RegisterType<LoggingService>().As<ILoggingService>().SingleInstance();
            builder.RegisterType<ManifestService>().As<IManifestService>().InstancePerRequest();
            builder.RegisterType<OpenSearchService>().As<IOpenSearchService>().InstancePerRequest();
            builder.RegisterType<RobotsService>().As<IRobotsService>().InstancePerRequest();
            builder.RegisterType<SitemapService>().As<ISitemapService>().InstancePerRequest();
            builder.RegisterType<SitemapPingerService>().As<ISitemapPingerService>().InstancePerRequest();
        }

        /// <summary>
        /// Register Container in Assembly
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="assembly"></param>
        private static void RegisterMvc(ContainerBuilder builder, Assembly assembly)
        {
            // Register Common MVC Types
            builder.RegisterModule<AutofacWebTypesModule>();

            // Register MVC Filters
            builder.RegisterFilterProvider();

            // Register MVC Controllers
            builder.RegisterControllers(assembly);
        }

        /// <summary>
        /// Sets the ASP.NET MVC dependency resolver.
        /// </summary>
        /// <param name="container">The container.</param>
        private static void SetMvcDependencyResolver(IContainer container)
        {
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}