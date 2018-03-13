using System.Collections.Generic;
using System.Web;
using System.Linq;
using EficienciaEnergetica.Models.Security;
using System;

namespace EficienciaEnergetica.Helpers
{
    /// <summary>
    /// Application Context
    /// </summary>
    public sealed class ApplicationContext
    {
        private static string logoEditorID = Guid.NewGuid().ToString();
        private static Dictionary<string, ApplicationUser> applicationUsers = new Dictionary<string, ApplicationUser>();

        /// <summary>
        /// Empresa Logo Editor ID
        /// </summary>
        public static string LogoEditorID
        {
            get { return logoEditorID; }
        }

        /// <summary>
        /// Get Current User
        /// </summary>
        public static ApplicationUser CurrentUser
        {
            get
            {
                return GetUser(HttpContext.Current.User.Identity.Name);
            }
        }

        /// <summary>
        /// Load DB User Data
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        private static ApplicationUser GetUser(string username)
        {
            ApplicationUser user = null;

            if (!string.IsNullOrEmpty(username))
            {
                lock (applicationUsers)
                {
                    if (!applicationUsers.TryGetValue(username, out user))
                    {
                        SecurityDbContext _db = new SecurityDbContext();
                        user = _db.Users.Where(Usr => Usr.UserName.Equals(username)).FirstOrDefault();
                        applicationUsers.Add(username, user);
                    }
                }
            }

            if (user == null)
            {
                user = new ApplicationUser();
                user.FirstName = "Anónimo";
            }

            return user;
        }
    }
}