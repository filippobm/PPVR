using PPVR.WebApp.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace PPVR.WebApp.DataAccess.Mappings
{
    public class IdeologiaMap : EntityTypeConfiguration<Ideologia>
    {
        public IdeologiaMap()
        {
            ToTable("Ideologias");

            HasKey(x => x.IdeologiaId);

            Property(x => x.Nome)
                .IsRequired()
                .HasMaxLength(30)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(
                        new IndexAttribute("IX_IDEOLOGIA_NOME")
                        {
                            IsUnique = true
                        }));
        }
    }
}