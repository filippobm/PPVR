using PPVR.WebApp.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace PPVR.WebApp.DAL.Mappings
{
    public class PartidoMap : EntityTypeConfiguration<Partido>
    {
        public PartidoMap()
        {
            ToTable("Partidos");

            HasKey(x => x.PartidoId)
                .Property(x => x.PartidoId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.Nome)
                .IsRequired()
                .HasMaxLength(60)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(
                        new IndexAttribute("IX_PARTIDO_NOME")
                        {
                            IsUnique = true
                        }));

            Property(x => x.Sigla)
                .IsRequired()
                .HasMaxLength(10)
                .HasColumnType("nchar")
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(
                        new IndexAttribute("IX_PARTIDO_SIGLA")
                        {
                            IsUnique = true
                        }));

            Property(x => x.NumeroEleitoral)
                .IsRequired()
                .HasColumnType("tinyint")
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(
                        new IndexAttribute("IX_PARTIDO_NUMERO_ELEITORAL")
                        {
                            IsUnique = true
                        }));

            HasMany(x => x.Ideologias)
                .WithMany(x => x.Partidos)
                .Map(x =>
                {
                    x.MapLeftKey("PartidoId");
                    x.MapRightKey("IdeologiaId");
                    x.ToTable("IdeologiasPartidos");
                });
        }
    }
}