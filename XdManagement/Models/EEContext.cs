using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Text;

namespace EficienciaEnergetica.Models
{
    /// <summary>
    /// EficienciaEnergetica BD Context
    /// </summary>
    public class EEContext : DbContext
    {
        /// <summary>
        /// Basic Constructor
        /// </summary>
        public EEContext() : base("name=EEContext")
        {
        }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Debug.WriteLine($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation errors: ");
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            catch (DbUpdateException dbu)
            {
                var exception = HandleDbUpdateException(dbu);
                throw exception;
            }
        }
        /// <summary>
        /// Handle Db Update Exception
        /// </summary>
        /// <param name="dbu"></param>
        /// <returns></returns>
        private Exception HandleDbUpdateException(DbUpdateException dbu)
        {
            var builder = new StringBuilder("A DbUpdateException was caught while saving changes. ");

            try
            {
                foreach (var result in dbu.Entries)
                {
                    builder.AppendFormat("Type: {0} was part of the problem. ", result.Entity.GetType().Name);
                }
            }
            catch (Exception e)
            {
                builder.Append("Error parsing DbUpdateException: " + e.ToString());
            }

            string message = builder.ToString();
            return new Exception(message, dbu);
        }

        /// <summary>
        /// On Model Creating
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            if (modelBuilder == null)
                throw new ArgumentNullException("modelBuilder");

            modelBuilder.Entity<Equipos>().Property(equipo => equipo.Consumo).HasPrecision(18, 3);
        }

        public DbSet<Ciudades> Ciudades { get; set; }
        public DbSet<SectoresEconomicos> SectoresEconomicos { get; set; }
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Sede> Sedes { get; set; }
        public DbSet<FuenteEnergetica> FuenteEnergeticas { get; set; }
        public DbSet<Dispositivo> Dispositivos { get; set; }
        public DbSet<Consumo> Consumo { get; set; }
        public DbSet<Peticion> Peticións { get; set; }
        public DbSet<PeriodoFacturacion> PeriodoFacturacions { get; set; }
        public DbSet<Años> Años { get; set; }
        public DbSet<Meses> Meses { get; set; }
        public DbSet<NivelTermico> NivelTermico { get; set; }
        public DbSet<Asesor> Asesors { get; set; }
        public DbSet<TiposEquipos> TiposEquipos { get; set; }
        public DbSet<Equipos> Equipos { get; set; }
        public DbSet<AccionAhorro> AccionAhorro { get; set; }
        public DbSet<Inventario> Inventario { get; set; }
        public DbSet<InventarioEquipos> InventarioEquipos { get; set; }
        public DbSet<AccionesAhorroSedes> AccionesAhorroSedes { get; set; }
    }
}
