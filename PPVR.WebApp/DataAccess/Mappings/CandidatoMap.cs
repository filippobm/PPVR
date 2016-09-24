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

            Property(x => x.NomeUrna)
                .IsRequired()
                .HasMaxLength(60)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(
                        new IndexAttribute("IX_CANDIDATO_NOME_URNA")));

            Property(x => x.Nome)
                .IsRequired()
                .HasMaxLength(60)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(
                        new IndexAttribute("IX_CANDIDATO_NOME")));

            Property(x => x.SiglaUnidadeFederacao)
                .IsRequired()
                .HasColumnType("char")
                .HasMaxLength(2)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(
                        new IndexAttribute("IX_CANDIDATO_NUMERO_ELEITORAL_SIGLA_UE_SIGLA_UF", 1)));

            Property(x => x.SiglaUnidadeEleitoral)
                .IsRequired()
                .HasMaxLength(60)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(
                        new IndexAttribute("IX_CANDIDATO_NUMERO_ELEITORAL_SIGLA_UE_SIGLA_UF", 2)));

            Property(x => x.NumeroEleitoral)
                .IsRequired()
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(
                        new IndexAttribute("IX_CANDIDATO_NUMERO_ELEITORAL_SIGLA_UE_SIGLA_UF", 3)));

            Property(x => x.DescricaoUnidadeEleitoral)
                .IsRequired()
                .HasMaxLength(255);

            Property(x => x.CargoEletivo)
                .IsRequired();

            HasRequired(x => x.Partido)
                .WithMany()
                .HasForeignKey(x => x.PartidoId);

            HasRequired(x => x.Eleicao)
                .WithMany()
                .HasForeignKey(x => x.EleicaoId);
        }
    }
}