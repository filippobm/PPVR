using PPVR.WebApp.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace PPVR.WebApp.DataAccess.Mappings
{
    public class EleicaoMap : EntityTypeConfiguration<Eleicao>
    {
        public EleicaoMap()
        {
            ToTable("Eleicoes");

            HasKey(x => x.EleicaoId);

            Property(x => x.Ano)
                .IsRequired()
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(
                        new IndexAttribute("IX_ELEICAO_ANO", 1)));

            Property(x => x.Turno)
                .IsRequired()
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(
                        new IndexAttribute("IX_ELEICAO_TURNO", 2)));

            Property(x => x.Descricao)
                .IsRequired()
                .HasMaxLength(60)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(
                        new IndexAttribute("IX_ELEICAO_DESCRICAO", 3)));
        }
    }
}