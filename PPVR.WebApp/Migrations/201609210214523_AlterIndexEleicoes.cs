namespace PPVR.WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterIndexEleicoes : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Eleicoes", "IX_ELEICAO_ANO");
            DropIndex("dbo.Eleicoes", "IX_ELEICAO_TURNO");
            DropIndex("dbo.Eleicoes", "IX_ELEICAO_DESCRICAO");
            CreateIndex("dbo.Eleicoes", new[] { "Ano", "Turno", "Descricao" }, unique: true, name: "IX_ELEICAO_ANO_TURNO_DESCRICAO");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Eleicoes", "IX_ELEICAO_ANO_TURNO_DESCRICAO");
            CreateIndex("dbo.Eleicoes", "Descricao", name: "IX_ELEICAO_DESCRICAO");
            CreateIndex("dbo.Eleicoes", "Turno", name: "IX_ELEICAO_TURNO");
            CreateIndex("dbo.Eleicoes", "Ano", name: "IX_ELEICAO_ANO");
        }
    }
}
