namespace PPVR.WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTableTiposPropaganda : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TiposPropaganda",
                c => new
                    {
                        TipoPropagandaId = c.Int(nullable: false, identity: true),
                        Descricao = c.String(nullable: false, maxLength: 30),
                        ValorMedio = c.Decimal(nullable: false, precision: 7, scale: 2),
                    })
                .PrimaryKey(t => t.TipoPropagandaId)
                .Index(t => t.Descricao, unique: true, name: "IX_TIPOS_PROPAGANDA_DESCRICAO");
            
            AddColumn("dbo.Ocorrencias", "TipoPropagandaId", c => c.Int(nullable: false));
            CreateIndex("dbo.Ocorrencias", "TipoPropagandaId");
            AddForeignKey("dbo.Ocorrencias", "TipoPropagandaId", "dbo.TiposPropaganda", "TipoPropagandaId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ocorrencias", "TipoPropagandaId", "dbo.TiposPropaganda");
            DropIndex("dbo.TiposPropaganda", "IX_TIPOS_PROPAGANDA_DESCRICAO");
            DropIndex("dbo.Ocorrencias", new[] { "TipoPropagandaId" });
            DropColumn("dbo.Ocorrencias", "TipoPropagandaId");
            DropTable("dbo.TiposPropaganda");
        }
    }
}
