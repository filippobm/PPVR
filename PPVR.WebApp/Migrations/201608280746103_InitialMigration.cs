using System.Data.Entity.Migrations;
using PPVR.WebApp.Models;

namespace PPVR.WebApp.Migrations
{
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Candidatos",
                c => new
                {
                    CandidatoId = c.Int(false, true),
                    PartidoId = c.Byte(false),
                    Nome = c.String(false, 60),
                    CargoEletivo = c.Byte(false),
                    NumeroEleitoral = c.Int(false),
                    Enabled = c.Boolean(false),
                    CreatedAt = c.DateTime(false),
                    UpdatedAt = c.DateTime(),
                    Partido_PartidoId = c.Byte()
                })
                .PrimaryKey(t => t.CandidatoId)
                .ForeignKey("dbo.Partidos", t => t.Partido_PartidoId)
                .ForeignKey("dbo.Partidos", t => t.PartidoId)
                .Index(t => t.PartidoId)
                .Index(t => t.NumeroEleitoral, unique: true, name: "IX_CANDIDATO_NUMERO_ELEITORAL")
                .Index(t => t.Partido_PartidoId);

            CreateTable(
                "dbo.Ocorrencias",
                c => new
                {
                    OcorrenciaId = c.Long(false, true),
                    CandidatoId = c.Int(false),
                    EnderecoId = c.Long(false),
                    Foto = c.Binary(false, storeType: "image"),
                    Verificada = c.Boolean(false),
                    CreatedAt = c.DateTime(false),
                    UpdatedAt = c.DateTime()
                })
                .PrimaryKey(t => t.OcorrenciaId)
                .ForeignKey("dbo.Candidatos", t => t.CandidatoId)
                .ForeignKey("dbo.Enderecos", t => t.EnderecoId)
                .Index(t => t.CandidatoId)
                .Index(t => t.EnderecoId);

            CreateTable(
                "dbo.Enderecos",
                c => new
                {
                    EnderecoId = c.Long(false, true),
                    Estado = c.String(maxLength: 2, fixedLength: true),
                    Cidade = c.String(maxLength: 60),
                    Bairro = c.String(maxLength: 60),
                    CEP = c.String(maxLength: 9, fixedLength: true),
                    EnderecoFormatado = c.String(maxLength: 255),
                    Latitude = c.Double(),
                    Longitude = c.Double()
                })
                .PrimaryKey(t => t.EnderecoId);

            CreateTable(
                "dbo.Partidos",
                c => new
                {
                    PartidoId = c.Byte(false, true),
                    Nome = c.String(false, 60),
                    Sigla = c.String(false, 10, true),
                    NumeroEleitoral = c.Byte(false),
                    EspectroPolitico = c.Byte(false),
                    Enabled = c.Boolean(false),
                    CreatedAt = c.DateTime(false),
                    UpdatedAt = c.DateTime()
                })
                .PrimaryKey(t => t.PartidoId)
                .Index(t => t.Nome, unique: true, name: "IX_PARTIDO_NOME")
                .Index(t => t.Sigla, unique: true, name: "IX_PARTIDO_SIGLA")
                .Index(t => t.NumeroEleitoral, unique: true, name: "IX_PARTIDO_NUMERO_ELEITORAL");

            CreateTable(
                "dbo.Ideologias",
                c => new
                {
                    IdeologiaId = c.Short(false, true),
                    Nome = c.String(false, 30),
                    Enabled = c.Boolean(false)
                })
                .PrimaryKey(t => t.IdeologiaId)
                .Index(t => t.Nome, unique: true, name: "IX_IDEOLOGIA_NOME");

            CreateTable(
                "dbo.AspNetRoles",
                c => new
                {
                    Id = c.String(false, 128),
                    Name = c.String(false, 256)
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");

            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                {
                    UserId = c.String(false, 128),
                    RoleId = c.String(false, 128)
                })
                .PrimaryKey(t => new {t.UserId, t.RoleId})
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);

            CreateTable(
                "dbo.AspNetUsers",
                c => new
                {
                    Id = c.String(false, 128),
                    Email = c.String(maxLength: 256),
                    EmailConfirmed = c.Boolean(false),
                    PasswordHash = c.String(),
                    SecurityStamp = c.String(),
                    PhoneNumber = c.String(),
                    PhoneNumberConfirmed = c.Boolean(false),
                    TwoFactorEnabled = c.Boolean(false),
                    LockoutEndDateUtc = c.DateTime(),
                    LockoutEnabled = c.Boolean(false),
                    AccessFailedCount = c.Int(false),
                    UserName = c.String(false, 256)
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");

            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                {
                    Id = c.Int(false, true),
                    UserId = c.String(false, 128),
                    ClaimType = c.String(),
                    ClaimValue = c.String()
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);

            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                {
                    LoginProvider = c.String(false, 128),
                    ProviderKey = c.String(false, 128),
                    UserId = c.String(false, 128)
                })
                .PrimaryKey(t => new {t.LoginProvider, t.ProviderKey, t.UserId})
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);

            CreateTable(
                "dbo.IdeologiasPartidos",
                c => new
                {
                    PartidoId = c.Byte(false),
                    IdeologiaId = c.Short(false)
                })
                .PrimaryKey(t => new {t.PartidoId, t.IdeologiaId})
                .ForeignKey("dbo.Partidos", t => t.PartidoId)
                .ForeignKey("dbo.Ideologias", t => t.IdeologiaId)
                .Index(t => t.PartidoId)
                .Index(t => t.IdeologiaId);

            InsertPartidos();
            InsertIdeologias();
        }

        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Candidatos", "PartidoId", "dbo.Partidos");
            DropForeignKey("dbo.IdeologiasPartidos", "IdeologiaId", "dbo.Ideologias");
            DropForeignKey("dbo.IdeologiasPartidos", "PartidoId", "dbo.Partidos");
            DropForeignKey("dbo.Candidatos", "Partido_PartidoId", "dbo.Partidos");
            DropForeignKey("dbo.Ocorrencias", "EnderecoId", "dbo.Enderecos");
            DropForeignKey("dbo.Ocorrencias", "CandidatoId", "dbo.Candidatos");
            DropIndex("dbo.IdeologiasPartidos", new[] {"IdeologiaId"});
            DropIndex("dbo.IdeologiasPartidos", new[] {"PartidoId"});
            DropIndex("dbo.AspNetUserLogins", new[] {"UserId"});
            DropIndex("dbo.AspNetUserClaims", new[] {"UserId"});
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] {"RoleId"});
            DropIndex("dbo.AspNetUserRoles", new[] {"UserId"});
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Ideologias", "IX_IDEOLOGIA_NOME");
            DropIndex("dbo.Partidos", "IX_PARTIDO_NUMERO_ELEITORAL");
            DropIndex("dbo.Partidos", "IX_PARTIDO_SIGLA");
            DropIndex("dbo.Partidos", "IX_PARTIDO_NOME");
            DropIndex("dbo.Ocorrencias", new[] {"EnderecoId"});
            DropIndex("dbo.Ocorrencias", new[] {"CandidatoId"});
            DropIndex("dbo.Candidatos", new[] {"Partido_PartidoId"});
            DropIndex("dbo.Candidatos", "IX_CANDIDATO_NUMERO_ELEITORAL");
            DropIndex("dbo.Candidatos", new[] {"PartidoId"});
            DropTable("dbo.IdeologiasPartidos");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Ideologias");
            DropTable("dbo.Partidos");
            DropTable("dbo.Enderecos");
            DropTable("dbo.Ocorrencias");
            DropTable("dbo.Candidatos");
        }

        private void InsertPartidos()
        {
            var insert =
                "INSERT INTO dbo.Partidos (Nome,NumeroEleitoral,Sigla,[Enabled],EspectroPolitico,[CreatedAt]) VALUES ('{0}',{1},'{2}',1,{3},GETDATE());";

            Sql(string.Format(insert, "Democratas", 25, "DEM", EspectroPolitico.CentroDireita.GetHashCode()));
            Sql(string.Format(insert, "Partido Comunista Brasileiro", 21, "PCB",
                EspectroPolitico.ExtremaEsquerda.GetHashCode()));
            Sql(string.Format(insert, "Partido Comunista do Brasil", 65, "PCdoB",
                EspectroPolitico.EsquerdaAExtremaEsquerda.GetHashCode()));
            Sql(string.Format(insert, "Partido da Causa Operária", 29, "PCO",
                EspectroPolitico.ExtremaEsquerda.GetHashCode()));
            Sql(string.Format(insert, "Partido da Mobilização Nacional", 33, "PMN",
                EspectroPolitico.CentroEsquerda.GetHashCode()));
            Sql(string.Format(insert, "Partido da Mulher Brasileira", 35, "PMB",
                EspectroPolitico.CentroEsquerda.GetHashCode()));
            Sql(string.Format(insert, "Partido da República", 22, "PR", EspectroPolitico.CentroDireita.GetHashCode()));
            Sql(string.Format(insert, "Partido da Social Democracia Brasileira", 45, "PSDB",
                EspectroPolitico.CentroEsquerda.GetHashCode()));
            Sql(string.Format(insert, "Partido Democrático Trabalhista", 12, "PDT",
                EspectroPolitico.CentroEsquerda.GetHashCode()));
            Sql(string.Format(insert, "Partido do Movimento Democrático Brasileiro", 15, "PMDB",
                EspectroPolitico.Centro.GetHashCode()));
            Sql(string.Format(insert, "Partido dos Trabalhadores", 13, "PT", EspectroPolitico.Esquerda.GetHashCode()));
            Sql(string.Format(insert, "Partido Ecológico Nacional", 51, "PEN",
                EspectroPolitico.CentroDireita.GetHashCode()));
            Sql(string.Format(insert, "Partido Humanista da Solidariedade", 31, "PHS",
                EspectroPolitico.CentroDireitaADireita.GetHashCode()));
            Sql(string.Format(insert, "Partido Novo", 30, "NOVO", EspectroPolitico.Centro.GetHashCode()));
            Sql(string.Format(insert, "Partido Pátria Livre", 54, "PPL",
                EspectroPolitico.CentroEsquerdaAEsquerda.GetHashCode()));
            Sql(string.Format(insert, "Partido Popular Socialista", 23, "PPS",
                EspectroPolitico.CentroEsquerda.GetHashCode()));
            Sql(string.Format(insert, "Partido Progressista", 11, "PP",
                EspectroPolitico.CentroDireitaADireita.GetHashCode()));
            Sql(string.Format(insert, "Partido Renovador Trabalhista Brasileiro", 28, "PRTB",
                EspectroPolitico.Direita.GetHashCode()));
            Sql(string.Format(insert, "Partido Republicano Brasileiro", 10, "PRB",
                EspectroPolitico.CentroDireita.GetHashCode()));
            Sql(string.Format(insert, "Partido Republicano da Ordem Social", 90, "PROS",
                EspectroPolitico.CentroEsquerda.GetHashCode()));
            Sql(string.Format(insert, "Partido Republicano Progressista", 44, "PRP",
                EspectroPolitico.CentroDireita.GetHashCode()));
            Sql(string.Format(insert, "Partido Social Cristão", 20, "PSC", EspectroPolitico.Direita.GetHashCode()));
            Sql(string.Format(insert, "Partido Social Democrata Cristão", 27, "PSDC",
                EspectroPolitico.DireitaAExtremaDireita.GetHashCode()));
            Sql(string.Format(insert, "Partido Social Democrático", 55, "PSD", EspectroPolitico.Centro.GetHashCode()));
            Sql(string.Format(insert, "Partido Social Liberal", 17, "PSL", EspectroPolitico.CentroDireita.GetHashCode()));
            Sql(string.Format(insert, "Partido Socialismo e Liberdade", 50, "PSOL",
                EspectroPolitico.EsquerdaAExtremaEsquerda.GetHashCode()));
            Sql(string.Format(insert, "Partido Socialista Brasileiro", 40, "PSB",
                EspectroPolitico.CentroEsquerdaAEsquerda.GetHashCode()));
            Sql(string.Format(insert, "Partido Socialista dos Trabalhadores Unificado", 16, "PSTU",
                EspectroPolitico.ExtremaEsquerda.GetHashCode()));
            Sql(string.Format(insert, "Partido Trabalhista Brasileiro", 14, "PTB", EspectroPolitico.Centro.GetHashCode()));
            Sql(string.Format(insert, "Partido Trabalhista Cristão", 36, "PTC",
                EspectroPolitico.CentroDireitaADireita.GetHashCode()));
            Sql(string.Format(insert, "Partido Trabalhista do Brasil", 70, "PTdoB",
                EspectroPolitico.CentroEsquerda.GetHashCode()));
            Sql(string.Format(insert, "Partido Trabalhista Nacional", 19, "PTN",
                EspectroPolitico.CentroEsquerda.GetHashCode()));
            Sql(string.Format(insert, "Partido Verde", 43, "PV", EspectroPolitico.CentroEsquerda.GetHashCode()));
            Sql(string.Format(insert, "Rede Sustentabilidade", 18, "REDE", EspectroPolitico.CentroEsquerda.GetHashCode()));
            Sql(string.Format(insert, "Solidariedade", 77, "SD", EspectroPolitico.CentroEsquerda.GetHashCode()));
        }

        private void InsertIdeologias()
        {
            var insert = "INSERT INTO dbo.Ideologias(Nome,[Enabled]) VALUES ('{0}',1)";

            Sql(string.Format(insert, "Ambientalismo"));
            Sql(string.Format(insert, "Brizolismo"));
            Sql(string.Format(insert, "Capitalismo"));
            Sql(string.Format(insert, "Centrismo"));
            Sql(string.Format(insert, "Comunismo"));
            Sql(string.Format(insert, "Conservadorismo"));
            Sql(string.Format(insert, "Conservadorismo liberal"));
            Sql(string.Format(insert, "Conservadorismo social"));
            Sql(string.Format(insert, "Democracia cristã"));
            Sql(string.Format(insert, "Democracia liberal"));
            Sql(string.Format(insert, "Desenvolvimentismo"));
            Sql(string.Format(insert, "Direita cristã"));
            Sql(string.Format(insert, "Direitos da mulher"));
            Sql(string.Format(insert, "Distributismo"));
            Sql(string.Format(insert, "Ecossocialismo"));
            Sql(string.Format(insert, "Federalismo"));
            Sql(string.Format(insert, "Getulismo"));
            Sql(string.Format(insert, "Humanismo"));
            Sql(string.Format(insert, "Intervencionismo"));
            Sql(string.Format(insert, "Khrushchevismo"));
            Sql(string.Format(insert, "Libertarianismo Bleeding-Heart"));
            Sql(string.Format(insert, "Libertarismo"));
            Sql(string.Format(insert, "Liberalismo económico"));
            Sql(string.Format(insert, "Liberalismo social"));
            Sql(string.Format(insert, "Lulismo"));
            Sql(string.Format(insert, "Maoismo"));
            Sql(string.Format(insert, "Marxismo"));
            Sql(string.Format(insert, "Marxismo-leninismo"));
            Sql(string.Format(insert, "Mobilização"));
            Sql(string.Format(insert, "Nacionalismo"));
            Sql(string.Format(insert, "Participalismo"));
            Sql(string.Format(insert, "Parlamentarismo"));
            Sql(string.Format(insert, "Populismo"));
            Sql(string.Format(insert, "Progressismo"));
            Sql(string.Format(insert, "Protecionismo"));
            Sql(string.Format(insert, "Reformismo"));
            Sql(string.Format(insert, "Republicanismo"));
            Sql(string.Format(insert, "Sincretismo político"));
            Sql(string.Format(insert, "Socialismo"));
            Sql(string.Format(insert, "Socialismo científico"));
            Sql(string.Format(insert, "Socialismo democrático"));
            Sql(string.Format(insert, "Social-democracia"));
            Sql(string.Format(insert, "Sustentabilidade"));
            Sql(string.Format(insert, "Terceira via"));
            Sql(string.Format(insert, "Trabalhismo"));
            Sql(string.Format(insert, "Trotskismo"));
        }
    }
}