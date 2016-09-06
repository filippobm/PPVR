using PPVR.WebApp.Models;
using System.Data.Entity.ModelConfiguration;

namespace PPVR.WebApp.DataAccess.Mappings
{
    public class EnderecoMap : EntityTypeConfiguration<Endereco>
    {
        public EnderecoMap()
        {
            ToTable("Enderecos");

            HasKey(x => x.EnderecoId);

            Property(x => x.Estado)
                .HasColumnType("nchar")
                .HasMaxLength(2);

            Property(x => x.Cidade)
                .HasMaxLength(60);

            Property(x => x.Bairro)
                .HasMaxLength(60);

            Property(x => x.CEP)
                .HasColumnType("nchar")
                .HasMaxLength(9);

            Property(x => x.EnderecoFormatado)
                .HasMaxLength(255);
        }
    }
}