using PPVR.WebApp.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace PPVR.WebApp.DataAccess.Mappings
{
    public class TipoPropagandaMap : EntityTypeConfiguration<TipoPropaganda>
    {
        public TipoPropagandaMap()
        {
            ToTable("TiposPropaganda");

            HasKey(x => x.TipoPropagandaId)
                .Property(x => x.TipoPropagandaId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.Descricao)
                .IsRequired()
                .HasMaxLength(30)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_TIPOS_PROPAGANDA_DESCRICAO") { IsUnique = true }));

            Property(x => x.ValorMedio)
                .HasPrecision(7, 2);
        }
    }
}