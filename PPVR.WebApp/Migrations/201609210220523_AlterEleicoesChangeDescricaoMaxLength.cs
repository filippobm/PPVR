namespace PPVR.WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterEleicoesChangeDescricaoMaxLength : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Eleicoes", "IX_ELEICAO_ANO_TURNO_DESCRICAO");
            AlterColumn("dbo.Eleicoes", "Descricao", c => c.String(nullable: false, maxLength: 100));
            CreateIndex("dbo.Eleicoes", new[] { "Ano", "Turno", "Descricao" }, unique: true, name: "IX_ELEICAO_ANO_TURNO_DESCRICAO");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Eleicoes", "IX_ELEICAO_ANO_TURNO_DESCRICAO");
            AlterColumn("dbo.Eleicoes", "Descricao", c => c.String(nullable: false, maxLength: 60));
            CreateIndex("dbo.Eleicoes", new[] { "Ano", "Turno", "Descricao" }, unique: true, name: "IX_ELEICAO_ANO_TURNO_DESCRICAO");
        }
    }
}
