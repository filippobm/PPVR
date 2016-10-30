using PPVR.WebApp.Models;
using System.Data.Entity.ModelConfiguration;

namespace PPVR.WebApp.DataAccess.Mappings
{
    public class OcorrenciaMap : EntityTypeConfiguration<Ocorrencia>
    {
        public OcorrenciaMap()
        {
            ToTable("Ocorrencias");

            HasKey(x => x.OcorrenciaId);

            Property(x => x.FotoPath)
                .IsRequired()
                .HasMaxLength(255);

            HasOptional(x => x.Candidato);

            HasOptional(x => x.Endereco);

            HasRequired(x => x.TipoPropaganda)
                .WithMany(x => x.Ocorrencias)
                .HasForeignKey(x => x.TipoPropagandaId);
        }
    }
}