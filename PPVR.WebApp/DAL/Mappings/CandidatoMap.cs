using PPVR.WebApp.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace PPVR.WebApp.DAL.Mappings
{
    public class CandidatoMap : EntityTypeConfiguration<Candidato>
    {
        public CandidatoMap()
        {
            ToTable("Candidatos");

            HasKey(x => x.CandidatoId);

            Property(x => x.Nome)
                .IsRequired()
                .HasMaxLength(60);

            Property(x => x.CargoEletivo)
                .IsRequired();

            Property(x => x.NumeroEleitoral)
                .IsRequired()
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(
                        new IndexAttribute("IX_CANDIDATO_NUMERO_ELEITORAL")
                        {
                            IsUnique = true
                        }));

            HasRequired(x => x.Partido)
                .WithMany()
                .HasForeignKey(x => x.PartidoId);
        }
    }
}