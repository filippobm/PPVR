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

            Property(x => x.Descricao)
                .IsRequired()
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(
                        new IndexAttribute("IX_ELEICAO_DESCRICAO")
                        {
                            IsUnique = true
                        }));

            Property(x => x.Ano)
                .IsRequired();
        }
    }
}