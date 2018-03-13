using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FL.Framework.Entity.Attributes;

namespace EficienciaEnergetica.Models
{
    [Table("Peticiones")]
    public class Peticion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PeticionID { get; set; }

        [Required(ErrorMessage = "Fecha de Registro")]
        [Display(Name = "Fecha Registro")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FechaRegistro { get; set; }

        [FontAwesome("fa-font")]
        [Required(ErrorMessage = "Título es obligatorio")]
        [StringLength(60)]
        [Display(Name = "Título")]
        public string Titulo { get; set; }

        [VisibleInListView(false)]
        [FontAwesome("fa-align-justify")]
        [Required(ErrorMessage = "Descripción es obligatoria")]
        [DataType(DataType.MultilineText)]
        [StringLength(600)]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        [FontAwesome("fa-comments-o")]
        [Display(Name = "Tipo de Petición")]
        public TipoPeticion TipoPeticion { get; set; }

        [FontAwesome("fa-calendar")]
        [Display(Name = "Fecha Solución")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? FechaSolucion { get; set; }

        [VisibleInListView(false)]
        [FontAwesome("fa-commenting-o")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Solución")]
        public string Solucion { get; set; }

        [FontAwesome("fa-user-o")]
        [Display(Name = "Resuelta Por")]
        public string ResueltaPor { get; set; }

        [FontAwesomeAttribute("fa-user-o")]
        //[Index("IX_Usuario")]
        [Display(Name = "Usuario")]
        public string UserID { get; set; }

        [Index("IX_Empresas")]
        [Display(Name = "Empresa")]
        public int EmpresaID { get; set; }
        public virtual Empresa Empresa { get; set; }

    }

    public enum TipoPeticion
    {
        Peticion,
        Queja,
        Reclamo,
        Soporte,
        Felicitacion
    }

}