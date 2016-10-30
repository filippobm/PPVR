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

            HasRequired(x => x.Candidato)
                .WithMany(x => x.Ocorrencias)
                .HasForeignKey(x => x.CandidatoId);

            HasRequired(x => x.Endereco)
                .WithMany(x => x.Ocorrencias)
                .HasForeignKey(x => x.EnderecoId);

            HasRequired(x => x.TipoPropaganda)
                .WithMany(x => x.Ocorrencias)
                .HasForeignKey(x => x.TipoPropagandaId);
        }
    }
}