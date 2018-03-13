using FL.Framework.Entity.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EficienciaEnergetica.Models
{
    [Table("Empresas")]
    public class Empresa
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmpresaID { get; set; }

        [VisibleInListView(true)]
        [FontAwesomeAttribute("fa-check-circle-o")]
        [Required(ErrorMessage = "Código es obligatorio")]
        [Index(IsUnique = true)]
        [DisplayFormat(DataFormatString = "{0:####}", ApplyFormatInEditMode = true)]
        [MaxLength(4, ErrorMessage = "El código no puede tener más de 4 caracteres")]
        public string Codigo { get; set; }

        [VisibleInListView(false)]
        [FontAwesomeAttribute("fa-bullseye")]
        [Required(ErrorMessage = "Nit es obligatorio")]
        [Index(IsUnique = true)]
        [Range(999, 9999999999)]
        public long Nit { get; set; }

        [FontAwesome("fa-font")]
        [Required(ErrorMessage = "Nombre de la Empresa es obligatoria")]
        [MaxLength(200, ErrorMessage = "El Nombre de la Empresa no puede tener más de 200 caracteres"),
         MinLength(3, ErrorMessage = "El Nombre de la Empresa no puede tener menos de 3 caracteres")]
        [Display(Name = "Empresa / Razón Social")]
        public string Nombre { get; set; }

        [VisibleInListView(false)]
        [Required(ErrorMessage = "Representante Legal es obligatorio")]
        [FontAwesomeAttribute("fa-user-circle-o")]
        [MaxLength(120, ErrorMessage = "El Representante Legal no puede tener más de 120 caracteres")]
        [Display(Name = "Representante Legal")]
        public string RepresentanteLegal { get; set; }

        [FontAwesome("fa-university")]
        [Index("IX_Ciudades")]
        [Display(Name = "Ciudad")]
        public int CiudadID { get; set; }
        public virtual Ciudades Ciudad { get; set; }

        [VisibleInListView(false)]
        [FontAwesome("fa-money")]
        [Index("IX_SectorEconomico")]
        [Display(Name = "Sector Economico")]
        public int SectorEconomicoID { get; set; }
        public virtual SectoresEconomicos SectorEconomico { get; set; }

        [FontAwesome("fa-map-marker")]
        [Display(Name = "Dirección")]
        [Required(ErrorMessage = "Dirección es obligatoria")]
        [MaxLength(120, ErrorMessage = "La Dirección no puede tener más de 120 caracteres")]
        public string Direccion { get; set; }

        [FontAwesome("fa-phone")]
        [Required(ErrorMessage = "Teléfono es obligatorio")]
        [Display(Name = "Teléfono")]
        public long Telefono { get; set; }

        [FontAwesome("fa-mobile")]
        public long? Celular { get; set; }

        [Required(ErrorMessage = "Email es obligatorio")]
        [FontAwesome("fa-envelope-o")]
        [MaxLength(100, ErrorMessage = "El Email no puede tener más de 100 caracteres")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Dirección de correo no valida")]
        public string Email { get; set; }

        [VisibleInListView(false)]
        [FontAwesome("fa-globe")]
        [MaxLength(300, ErrorMessage = "El Sitio Web no puede tener más de 300 caracteres")]
        [Display(Name = "Sitio Web")]
        public string SitioWeb { get; set; }

        [VisibleInListView(false)]
        [FontAwesome("fa-file-image-o")]
        [Display(Name = "Logo")]
        public string LogoUrl { get; set; }

        public virtual ICollection<Sede> Sedes { get; set; }
        public virtual ICollection<Peticion> Peticións { get; set; }
        public virtual ICollection<Inventario> Inventario { get; set; }
    }
}

