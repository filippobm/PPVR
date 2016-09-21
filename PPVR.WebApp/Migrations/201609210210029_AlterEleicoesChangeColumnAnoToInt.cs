namespace PPVR.WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterEleicoesChangeColumnAnoToInt : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Eleicoes", "IX_ELEICAO_ANO");
            AlterColumn("dbo.Eleicoes", "Ano", c => c.Int(nullable: false));
            CreateIndex("dbo.Eleicoes", "Ano", name: "IX_ELEICAO_ANO");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Eleicoes", "IX_ELEICAO_ANO");
            AlterColumn("dbo.Eleicoes", "Ano", c => c.Byte(nullable: false));
            CreateIndex("dbo.Eleicoes", "Ano", name: "IX_ELEICAO_ANO");
        }
    }
}
