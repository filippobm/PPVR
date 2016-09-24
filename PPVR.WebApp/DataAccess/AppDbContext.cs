using Microsoft.AspNet.Identity.EntityFramework;
using PPVR.WebApp.DataAccess.InitConfig;
using PPVR.WebApp.DataAccess.Mappings;
using PPVR.WebApp.Models;
using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;

namespace PPVR.WebApp.DataAccess
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext()
            : base("AppConnectionString", false)
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;

            Database.SetInitializer(new ContextInitializer());
            Database.Initialize(true);
        }

        public static AppDbContext Create()
        {
            return new AppDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            // Mappings
            modelBuilder.Configurations.Add(new CandidatoMap());
            modelBuilder.Configurations.Add(new EleicaoMap());
            modelBuilder.Configurations.Add(new EnderecoMap());
            modelBuilder.Configurations.Add(new OcorrenciaMap());
            modelBuilder.Configurations.Add(new IdeologiaMap());
            modelBuilder.Configurations.Add(new PartidoMap());
        }

        public override int SaveChanges()
        {
            // Set Enabled = true
            foreach (var entry in ChangeTracker.Entries().Where(x => x.Entity.GetType().GetProperty("Enabled") != null))
            {
                if (entry.State == EntityState.Added)
                    entry.Property("Enabled").CurrentValue = true;
            }

            // Set CreatedAt
            foreach (var entry in ChangeTracker.Entries()
                .Where(x => x.Entity.GetType().GetProperty("CreatedAt") != null))
            {
                if (entry.State == EntityState.Added)
                    entry.Property("CreatedAt").CurrentValue = DateTime.Now;
            }

            // Set UpdatedAt
            foreach (var entry in ChangeTracker.Entries()
                .Where(x => x.Entity.GetType().GetProperty("UpdatedAt") != null))
            {
                if (entry.State == EntityState.Modified)
                    entry.Property("UpdatedAt").CurrentValue = DateTime.Now;
            }
            return base.SaveChanges();
        }

        #region DbSets

        public DbSet<Candidato> Candidatos { get; set; }
        public DbSet<Eleicao> Eleicoes { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }
        public DbSet<Ideologia> Ideologias { get; set; }
        public DbSet<Ocorrencia> Ocorrencias { get; set; }
        public DbSet<Partido> Partidos { get; set; }

        #endregion
    }
}