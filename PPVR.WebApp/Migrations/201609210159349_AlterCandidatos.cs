namespace PPVR.WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterCandidatos : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Candidatos", "IX_CANDIDATO_ESTADO");
            DropIndex("dbo.Candidatos", "IX_CANDIDATO_CIDADE");
            AddColumn("dbo.Candidatos", "SiglaUnidadeFederacao", c => c.String(nullable: false, maxLength: 2, fixedLength: true, unicode: false));
            AddColumn("dbo.Candidatos", "SiglaUnidadeEleitoral", c => c.String(nullable: false, maxLength: 60));
            AddColumn("dbo.Candidatos", "DescricaoUnidadeEleitoral", c => c.String(nullable: false, maxLength: 255));
            CreateIndex("dbo.Candidatos", "SiglaUnidadeFederacao", name: "IX_CANDIDATO_SIGLA_UNIDADE_FEDERACAO");
            CreateIndex("dbo.Candidatos", "DescricaoUnidadeEleitoral", name: "IX_CANDIDATO_DESCRICAO_UNIDADE_ELEITORAL");
            DropColumn("dbo.Candidatos", "Estado");
            DropColumn("dbo.Candidatos", "Cidade");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Candidatos", "Cidade", c => c.String(nullable: false, maxLength: 60));
            AddColumn("dbo.Candidatos", "Estado", c => c.String(nullable: false, maxLength: 2, fixedLength: true, unicode: false));
            DropIndex("dbo.Candidatos", "IX_CANDIDATO_DESCRICAO_UNIDADE_ELEITORAL");
            DropIndex("dbo.Candidatos", "IX_CANDIDATO_SIGLA_UNIDADE_FEDERACAO");
            DropColumn("dbo.Candidatos", "DescricaoUnidadeEleitoral");
            DropColumn("dbo.Candidatos", "SiglaUnidadeEleitoral");
            DropColumn("dbo.Candidatos", "SiglaUnidadeFederacao");
            CreateIndex("dbo.Candidatos", "Cidade", name: "IX_CANDIDATO_CIDADE");
            CreateIndex("dbo.Candidatos", "Estado", name: "IX_CANDIDATO_ESTADO");
        }
    }
}
