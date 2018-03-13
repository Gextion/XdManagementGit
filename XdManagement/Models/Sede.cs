using FL.Framework.Entity.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EficienciaEnergetica.Models
{
    [Table("Sedes")]
    public class Sede
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SedeID { get; set; }

        [FontAwesome("fa-font")]
        [Required(ErrorMessage = "Nombre de la Sede es obligatorio")]
        [MaxLength(200, ErrorMessage = "El Nombre de la Sede no puede tener más de 200 caracteres"),
         MinLength(3, ErrorMessage = "El Nombre de la Sede no puede tener menos de 3 caracteres")]
        [Display(Name = "Sede")]
        public string NombreSede { get; set; }

        [FontAwesome("fa-user-circle-o")]
        [VisibleInListView(false)]
        [MaxLength(120, ErrorMessage = "Responsable no puede tener más de 120 caracteres")]
        public string Responsable { get; set; }

        [FontAwesome("fa-list-ol")]
        [VisibleInListView(false)]
        [Required(ErrorMessage = "El Estrato es obligatorio")]
        [Range(1, 6, ErrorMessage = "Debe estar entre 1 y 6")]
        public int Estrato { get; set; }

        [FontAwesome("fa-university")]
        [Index("IX_Ciudades")]
        [Display(Name = "Ciudad")]
        public int CiudadID { get; set; }
        public virtual Ciudades Ciudad { get; set; }

        [FontAwesome("fa-map-marker")]
        [VisibleInListView(false)]
        [Required(ErrorMessage = "Dirección es obligatoria")]
        [Display(Name = "Dirección")]
        [MaxLength(120, ErrorMessage = "La Dirección no puede tener más de 120 caracteres")]
        public string Direccion { get; set; }

        [FontAwesome("fa-phone")]
        [Display(Name = "Teléfono")]
        public long? Telefono { get; set; }

        [FontAwesome("fa-mobile")]
        public long? Celular { get; set; }

        [FontAwesome("fa-envelope-o")]
        [MaxLength(100, ErrorMessage = "El Email no puede tener más de 100 caracteres")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Dirección de correo no valida")]
        public string Email { get; set; }

        [Index("IX_Empresas")]
        [Display(Name = "Empresa")]
        public int EmpresaID { get; set; }
        public virtual Empresa Empresa { get; set; }

        public  ICollection<Dispositivo> Dispositivos { get; set; }
    }
}

