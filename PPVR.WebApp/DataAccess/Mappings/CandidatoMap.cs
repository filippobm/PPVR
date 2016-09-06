using PPVR.WebApp.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace PPVR.WebApp.DataAccess.Mappings
{
    public class CandidatoMap : EntityTypeConfiguration<Candidato>
    {
        public CandidatoMap()
        {
            ToTable("Candidatos");

            HasKey(x => x.CandidatoId);

            Property(x => x.Nome)
                .IsRequired()
                .HasMaxLength(60)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(
                        new IndexAttribute("IX_CANDIDATO_NOME", 1)));

            Property(x => x.Cidade)
                .IsRequired()
                .HasMaxLength(60)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(
                        new IndexAttribute("IX_CANDIDATO_CIDADE", 3)));

            Property(x => x.Estado)
                .IsRequired()
                .HasColumnType("char")
                .HasMaxLength(2)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(
                        new IndexAttribute("IX_CANDIDATO_ESTADO", 4)));

            Property(x => x.CargoEletivo)
                .IsRequired();

            Property(x => x.NumeroEleitoral)
                .IsRequired()
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(
                        new IndexAttribute("IX_CANDIDATO_NUMERO_ELEITORAL", 2)
                        {
                            IsUnique = true
                        }));

            HasRequired(x => x.Partido)
                .WithMany()
                .HasForeignKey(x => x.PartidoId);
        }
    }
}