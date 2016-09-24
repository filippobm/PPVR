namespace PPVR.WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterCandidatosIndexes : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Candidatos", "IX_CANDIDATO_NOME");
            DropIndex("dbo.Candidatos", "IX_CANDIDATO_NUMERO_ELEITORAL");
            DropIndex("dbo.Candidatos", "IX_CANDIDATO_SIGLA_UNIDADE_FEDERACAO");
            DropIndex("dbo.Candidatos", "IX_CANDIDATO_DESCRICAO_UNIDADE_ELEITORAL");
            AddColumn("dbo.Candidatos", "EleicaoId", c => c.Short(nullable: false));
            AddColumn("dbo.Candidatos", "Eleicao_EleicaoId", c => c.Short());
            AlterColumn("dbo.Candidatos", "NomeUrna", c => c.String(nullable: false, maxLength: 60));
            CreateIndex("dbo.Candidatos", "EleicaoId");
            CreateIndex("dbo.Candidatos", "Nome", name: "IX_CANDIDATO_NOME");
            CreateIndex("dbo.Candidatos", "NomeUrna", name: "IX_CANDIDATO_NOME_URNA");
            CreateIndex("dbo.Candidatos", new[] { "SiglaUnidadeFederacao", "SiglaUnidadeEleitoral", "NumeroEleitoral" }, name: "IX_CANDIDATO_NUMERO_ELEITORAL_SIGLA_UE_SIGLA_UF");
            CreateIndex("dbo.Candidatos", "Eleicao_EleicaoId");
            AddForeignKey("dbo.Candidatos", "Eleicao_EleicaoId", "dbo.Eleicoes", "EleicaoId");
            AddForeignKey("dbo.Candidatos", "EleicaoId", "dbo.Eleicoes", "EleicaoId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Candidatos", "EleicaoId", "dbo.Eleicoes");
            DropForeignKey("dbo.Candidatos", "Eleicao_EleicaoId", "dbo.Eleicoes");
            DropIndex("dbo.Candidatos", new[] { "Eleicao_EleicaoId" });
            DropIndex("dbo.Candidatos", "IX_CANDIDATO_NUMERO_ELEITORAL_SIGLA_UE_SIGLA_UF");
            DropIndex("dbo.Candidatos", "IX_CANDIDATO_NOME_URNA");
            DropIndex("dbo.Candidatos", "IX_CANDIDATO_NOME");
            DropIndex("dbo.Candidatos", new[] { "EleicaoId" });
            AlterColumn("dbo.Candidatos", "NomeUrna", c => c.String());
            DropColumn("dbo.Candidatos", "Eleicao_EleicaoId");
            DropColumn("dbo.Candidatos", "EleicaoId");
            CreateIndex("dbo.Candidatos", "DescricaoUnidadeEleitoral", name: "IX_CANDIDATO_DESCRICAO_UNIDADE_ELEITORAL");
            CreateIndex("dbo.Candidatos", "SiglaUnidadeFederacao", name: "IX_CANDIDATO_SIGLA_UNIDADE_FEDERACAO");
            CreateIndex("dbo.Candidatos", "NumeroEleitoral", unique: true, name: "IX_CANDIDATO_NUMERO_ELEITORAL");
            CreateIndex("dbo.Candidatos", "Nome", name: "IX_CANDIDATO_NOME");
        }
    }
}
