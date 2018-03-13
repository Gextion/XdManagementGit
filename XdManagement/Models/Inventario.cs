using FL.Framework.Entity.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EficienciaEnergetica.Models
{
    [Table("Inventario")]
    public class Inventario
    {
        /// <summary>
        /// Basic Constructor
        /// </summary>
        public Inventario()
        {
            Fecha = Helpers.DateHelper.GetColombiaDateTime();
            ItemCantidad = 1;
            HorasSemana = 1;
            HorasSabado = 1;
            HorasDomingo = 1;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InventarioSedeID { get; set; }

        [FontAwesome("fa-home")]
        [Index("IX_Empresas")]
        [Display(Name = "Empresa")]
        public int EmpresaID { get; set; }
        public virtual Empresa Empresa { get; set; }

        [FontAwesome("fa-home")]
        [Index("IX_Sede")]
        [Display(Name = "Sede")]
        public int SedeID { get; set; }
        public virtual Sede Sede { get; set; }

        [FontAwesome("fa-user-o")]
        [Index("IX_Asesor")]
        [Display(Name = "Asesor")]
        public int AsesorID { get; set; }
        public virtual Asesor Asesor { get; set; }

        [FontAwesome("fa-calendar")]
        [Required(ErrorMessage = "Fecha")]
        [Display(Name = "Fecha")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Fecha { get; set; }

        [FontAwesome("fa-arrows-alt")]
        [Required(ErrorMessage = "La empresa prestadora del servicio es obligatoria")]
        [Display(Name = "Prestador Servicio")]
        public string PrestadoraServicio { get; set; }

        [FontAwesome("fa-money")]
        [Required(ErrorMessage = "El consumo es obligatorio")]
        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "El valor no es correcto. Debe contener máximo 2 decimales.")]
        [DisplayFormat(DataFormatString = "{0:N}")]
        public decimal Consumo { get; set; }

        [FontAwesome("fa-money")]
        [Required(ErrorMessage = "El valor es obligatorio")]
        [Display(Name = "Valor KWH")]
        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "El valor no es correcto. Debe contener máximo 2 decimales.")]
        [DisplayFormat(DataFormatString = "{0:N}")]
        public decimal ValorKWH { get; set; }

        [FontAwesome("fa-arrows-alt")]
        [Required(ErrorMessage = "Los metros cuadrados son obligatorios")]
        [Display(Name = "Metros Cuadrados")]
        [DisplayFormat(DataFormatString = "{0:N}")]
        public Int16 MetrosCuadrados { get; set; }

        [FontAwesome("fa-clock-o")]
        [Required(ErrorMessage = "Las horas de la semana son requeridos")]
        [Display(Name = "Horas Semana")]
        [Range(0, 120)]
        public Int16 HorasSemana { get; set; }

        [FontAwesome("fa-clock-o")]
        [Required(ErrorMessage = "Las horas del sábado son requeridas")]
        [Display(Name = "Horas Sábado")]
        [Range(0, 24)]
        public Int16 HorasSabado { get; set; }

        [FontAwesome("fa-clock-o")]
        [Required(ErrorMessage = "Las horas del domingo son requeridas")]
        [Display(Name = "Horas Domingo")]
        [Range(0, 24)]
        public Int16 HorasDomingo { get; set; }

        [Display(Name = "Detalle Inventario")]
        public virtual ICollection<InventarioEquipos> InventarioEquipos { get; set; }

        [Display(Name = "Acciones Ahorro")]
        public virtual ICollection<AccionesAhorroSedes> AccionesAhorroSedes { get; set; }

        [NotMapped]
        public bool IsSaved { get; set; }

        [NotMapped]
        public int ItemTiposEquiposID { get; set; }

        [NotMapped]
        public virtual ICollection<TiposEquipos> ItemTiposEquipos { get; set; }

        [NotMapped]
        public virtual ICollection<TiposEquipos> ItemTiposEquiposAction { get; set; }

        [NotMapped]
        public virtual ICollection<AccionAhorro> ItemAccionesAhorro { get; set; }

        [NotMapped]
        public virtual Equipos ItemEquipoID { get; set; }

        [NotMapped]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int ItemCantidad { get; set; }
    }

    /// <summary>
    /// Index ViewModel
    /// </summary>
    public class InventarioIndexModel
    {
        public int InventarioSedeID { get; set; }
        public string NombreEmpresa { get; set; }
        public string NombreSede { get; set; }
        public Int16 Dimension { get; set; }
        public string NombreCiudad { get; set; }
        public DateTime Fecha { get; set; }
        public string FechaShort { get { return Fecha.ToShortDateString(); } }
        public string NombreNivelTermico { get; set; }
        public string NombreAsesor { get; set; }
        public List<InventarioEquipos> DetailEquipos { get; set; }
        public List<AccionesAhorroSedes> DetailAcciones { get; set; }
    }

    /// <summary>
    /// Print ViewModel
    /// </summary>
    public class InventarioPrintModel
    {
        public int InventarioSedeID { get; set; }
        public string CodigoEmpresa { get; set; }
        public string NombreEmpresa { get; set; }
        public string SectorEconomico { get; set; }
        public string NombreSede { get; set; }
        public string NombreCiudad { get; set; }
        public string PrestadoraServicio { get; set; }
        public string ConsumoStr { get; set; }
        public decimal Consumo { get; set; }
        public string ValorKWHStr { get; set; }
        public decimal ValorKWH { get; set; }
        public Int16 MetrosCuadrados { get; set; }
        public string MetrosCuadradosStr { get; set; }
        public DateTime Fecha { get; set; }
        public string FechaShort { get { return Fecha.ToShortDateString(); } }
        public string NombreNivelTermico { get; set; }
        public string NombreAsesor { get; set; }
        public string CodigoAsesor { get; set; }
        public List<InventarioEquipos> DetailEquipos { get; set; }
        public List<AccionesAhorroSedes> DetailAcciones { get; set; }
        public string Content { get; set; }
        public string ContentResumen { get; set; }
    }

    public class InternalPrintModel
    {
        public string Content { get; set; }
        public string ContentResumen { get; set; }
    }

    public class InternalResumenModel
    {
        public string Tipo { get; set; }
        public decimal ConsumoMes { get; set; }
        public decimal Participacion { get; set; }
    }

    /// <summary>
    /// Edit ViewMode
    /// </summary>
    public class InventarioEditViewModel
    {
        public int PkID { get; set; }
        public bool IsSaved { get; set; }
        public DateTime Fec { get; set; }
        public int EmpID { get; set; }
        public int SedeID { get; set; }
        public int AsesID { get; set; }
        public string PrestardoraServ { get; set; }
        public decimal Cons { get; set; }
        public decimal ValK { get; set; }
        public Int16 MetCua { get; set; }
        public Int16 HorSem { get; set; }
        public Int16 HorSab { get; set; }
        public Int16 HorDom { get; set; }
        public int TipEq { get; set; }
        public int EqID { get; set; }
        public Int16 Can { get; set; }
        public int Act { get; set; }
    }
}