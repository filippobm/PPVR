namespace PPVR.WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Candidatos",
                c => new
                    {
                        CandidatoId = c.Int(nullable: false, identity: true),
                        PartidoId = c.Byte(nullable: false),
                        Nome = c.String(nullable: false, maxLength: 60),
                        CargoEletivo = c.Byte(nullable: false),
                        NumeroEleitoral = c.Int(nullable: false),
                        Enabled = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                        Partido_PartidoId = c.Byte(),
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
                        OcorrenciaId = c.Long(nullable: false, identity: true),
                        CandidatoId = c.Int(nullable: false),
                        EnderecoId = c.Long(nullable: false),
                        Foto = c.Binary(nullable: false, storeType: "image"),
                        Verificada = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
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
                        EnderecoId = c.Long(nullable: false, identity: true),
                        Estado = c.String(maxLength: 2, fixedLength: true),
                        Cidade = c.String(maxLength: 60),
                        Bairro = c.String(maxLength: 60),
                        CEP = c.String(maxLength: 9, fixedLength: true),
                        EnderecoFormatado = c.String(maxLength: 255),
                        Latitude = c.Double(),
                        Longitude = c.Double(),
                    })
                .PrimaryKey(t => t.EnderecoId);
            
            CreateTable(
                "dbo.Partidos",
                c => new
                    {
                        PartidoId = c.Byte(nullable: false, identity: true),
                        Nome = c.String(nullable: false, maxLength: 60),
                        Sigla = c.String(nullable: false, maxLength: 10, fixedLength: true),
                        NumeroEleitoral = c.Byte(nullable: false),
                        EspectroPolitico = c.Byte(nullable: false),
                        Enabled = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.PartidoId)
                .Index(t => t.Nome, unique: true, name: "IX_PARTIDO_NOME")
                .Index(t => t.Sigla, unique: true, name: "IX_PARTIDO_SIGLA")
                .Index(t => t.NumeroEleitoral, unique: true, name: "IX_PARTIDO_NUMERO_ELEITORAL");
            
            CreateTable(
                "dbo.Ideologias",
                c => new
                    {
                        IdeologiaId = c.Short(nullable: false, identity: true),
                        Nome = c.String(nullable: false, maxLength: 30),
                        Enabled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IdeologiaId)
                .Index(t => t.Nome, unique: true, name: "IX_IDEOLOGIA_NOME");
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.IdeologiasPartidos",
                c => new
                    {
                        PartidoId = c.Byte(nullable: false),
                        IdeologiaId = c.Short(nullable: false),
                    })
                .PrimaryKey(t => new { t.PartidoId, t.IdeologiaId })
                .ForeignKey("dbo.Partidos", t => t.PartidoId)
                .ForeignKey("dbo.Ideologias", t => t.IdeologiaId)
                .Index(t => t.PartidoId)
                .Index(t => t.IdeologiaId);
            
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
            DropIndex("dbo.IdeologiasPartidos", new[] { "IdeologiaId" });
            DropIndex("dbo.IdeologiasPartidos", new[] { "PartidoId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Ideologias", "IX_IDEOLOGIA_NOME");
            DropIndex("dbo.Partidos", "IX_PARTIDO_NUMERO_ELEITORAL");
            DropIndex("dbo.Partidos", "IX_PARTIDO_SIGLA");
            DropIndex("dbo.Partidos", "IX_PARTIDO_NOME");
            DropIndex("dbo.Ocorrencias", new[] { "EnderecoId" });
            DropIndex("dbo.Ocorrencias", new[] { "CandidatoId" });
            DropIndex("dbo.Candidatos", new[] { "Partido_PartidoId" });
            DropIndex("dbo.Candidatos", "IX_CANDIDATO_NUMERO_ELEITORAL");
            DropIndex("dbo.Candidatos", new[] { "PartidoId" });
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
    }
}
