using Microsoft.AspNet.Identity.EntityFramework;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace EficienciaEnergetica.Models.Security
{
    /// <summary>
    /// Application User
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
            : base()
        {
            this.Groups = new HashSet<ApplicationUserGroup>();
        }

        [Required(ErrorMessage = "Nombre es obligatorio")]
        [Column("FirstName")]
        [Display(Name = "Nombres")]
        public string FirstName { get; set; }

        [Display(Name = "Apellidos")]
        [Column("LastName")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Empresa es obligatorio")]
        [Display(Name = "Compañía")]
        [Column("EmpresaID")]
        public int EmpresaID { get; set; }

        [NotMapped]
        public string PasswordConfirm { get; set; }

        [Display(Name = "Logo")]
        public string LogoUrl { get; set; }

        /// <summary>
        /// User Full Name
        /// </summary>
        [NotMapped]
        public string FullName { get { return $"{FirstName} {LastName}"; } }

        /// <summary>
        /// IS Super ADmin
        /// </summary>
        [NotMapped]
        public bool IsSuperAdmin
        {
            get
            {
                return UserContainGroup(Startup.SuperAdminName);
            }
        }
        
        /// <summary>
        /// Is ADmin
        /// </summary>
        [NotMapped]
        public bool IsAdmin
        {
            get
            {
                return UserContainGroup(Startup.AdminName);
            }
        }

        /// <summary>
        /// User Full Name
        /// </summary>
        [NotMapped]
        public string RolName
        {
            get
            {

                string RolesNames = string.Empty;

                if (Groups != null && Groups.Count > 0)
                {
                    Groups.ToList().ForEach(gr =>
                    {

                        if (gr.Group != null)
                        {
                            if (string.IsNullOrEmpty(RolesNames))
                            {
                                RolesNames = gr.Group.Name;
                            }
                            else
                            {
                                RolesNames = $"{RolesNames}, {gr.Group.Name}";
                            }
                        }
                    });
                }

                if (!string.IsNullOrEmpty(RolesNames))
                    return RolesNames;

                return "Usuario";
            }
        }

        /// <summary>
        /// Grupos del Usuario
        /// </summary>
        public virtual ICollection<ApplicationUserGroup> Groups { get; set; }

        /// <summary>
        /// Authentication Audit
        /// </summary>
        public virtual ICollection<AuthenticationAudit> AuthenticationAudit { get; set; }

        /// <summary>
        /// Empresa de Usuario
        /// </summary>
        public virtual Empresa Empresa { get; set; }

        /// <summary>
        /// Add User
        /// </summary>
        /// <param name="FirstName"></param>
        /// <param name="LastName"></param>
        /// <param name="Email"></param>
        /// <param name="EmpresaId"></param>
        /// <param name="Password"></param>
        public static UserCreateResult AddUser(string UserName, string Password, string FirstName, string LastName, string Email, int EmpresaId)
        {
            try
            {
                var user = new ApplicationUser();
                user.UserName = UserName;
                user.FirstName = string.IsNullOrEmpty(FirstName) ? UserName : FirstName;
                user.LastName = string.IsNullOrEmpty(LastName) ? UserName : LastName;
                user.Email = string.IsNullOrEmpty(Email) ? "emailtemp@org.co" : Email;
                user.EmpresaID = EmpresaId;

                IdentityManager idManager = new IdentityManager();
                var chkUser = idManager.CreateUser(user, Password);
                if (chkUser.Succeeded)
                {
                    SecurityDbContext Db = new SecurityDbContext();
                    var group = Db.Groups.Where(gr => gr.Name.Equals(Startup.AdminName)).FirstOrDefault();
                    if (group != null)
                        idManager.AddUserToGroup(user.Id, group.Id);

                    return new UserCreateResult() { Succeeded = true };
                }
                else
                {
                    return new UserCreateResult() { Succeeded = false, Errors = chkUser.Errors.ToList() };
                }
            }
            catch (Exception eX)
            {
                List<string> iErrors = new List<string>();
                iErrors.Add(eX.Message);

                System.Data.Entity.Validation.DbEntityValidationException EntityErrors = eX as System.Data.Entity.Validation.DbEntityValidationException;
                if (EntityErrors != null && EntityErrors.EntityValidationErrors != null && EntityErrors.EntityValidationErrors.Count() > 0)
                {
                    EntityErrors.EntityValidationErrors.ToList().ForEach(err =>
                    {
                        if (!err.IsValid && err.ValidationErrors != null && err.ValidationErrors.Count > 0)
                        {
                            err.ValidationErrors.ToList().ForEach(vE => iErrors.Add($"Propiedad: {vE.PropertyName} - Error: {vE.ErrorMessage}"));
                        }
                    });
                }

                return new UserCreateResult() { Succeeded = false, Errors = iErrors };
            }
        }

        /// <summary>
        /// Delete Iser
        /// </summary>
        /// <param name="User"></param>
        public static UserDeleteResult ClearUserGroups(ApplicationUser User)
        {
            try
            {
                if (User != null)
                {
                    IdentityManager idManager = new IdentityManager();
                    idManager.ClearGroups(User);

                    return new UserDeleteResult() { Succeeded = true };
                }
            }
            catch (Exception eX)
            {
                List<string> iErrors = new List<string>();
                iErrors.Add(eX.Message);

                System.Data.Entity.Validation.DbEntityValidationException EntityErrors = eX as System.Data.Entity.Validation.DbEntityValidationException;
                if (EntityErrors != null && EntityErrors.EntityValidationErrors != null && EntityErrors.EntityValidationErrors.Count() > 0)
                {
                    EntityErrors.EntityValidationErrors.ToList().ForEach(err =>
                    {
                        if (!err.IsValid && err.ValidationErrors != null && err.ValidationErrors.Count > 0)
                        {
                            err.ValidationErrors.ToList().ForEach(vE => iErrors.Add($"Propiedad: {vE.PropertyName} - Error: {vE.ErrorMessage}"));
                        }
                    });
                }

                return new UserDeleteResult() { Succeeded = false, Errors = iErrors };
            }

            return new UserDeleteResult() { Succeeded = true };
        }

        /// <summary>
        /// Find User
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        internal static ApplicationUser FindUser(string UserName)
        {
            IdentityManager idManager = new IdentityManager();
            return idManager.GetUser(UserName);
        }

        /// <summary>
        /// User Contain Group
        /// </summary>
        /// <param name="GroupName"></param>
        /// <returns></returns>
        private bool UserContainGroup(string GroupName)
        {
            if (Groups != null && Groups.Count > 0)
            {
                foreach (var group in Groups)
                {
                    if (group.Group.Name.Equals(GroupName))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }


    public class UserCreateResult : BaseResult { }
    public class UserDeleteResult : BaseResult { }

    public class BaseResult
    {
        public BaseResult()
        {
            Errors = new List<string>();
        }

        public bool Succeeded { get; set; }
        public List<string> Errors { get; set; }
    }
}