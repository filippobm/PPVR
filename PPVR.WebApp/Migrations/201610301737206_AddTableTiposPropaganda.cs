using System.Data.Entity.Migrations;

namespace PPVR.WebApp.Migrations
{
    public partial class AddTableTiposPropaganda : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TiposPropaganda",
                c => new
                {
                    TipoPropagandaId = c.Int(false, true),
                    Descricao = c.String(false, 30),
                    ValorMedio = c.Decimal(false, 7, 2)
                })
                .PrimaryKey(t => t.TipoPropagandaId)
                .Index(t => t.Descricao, unique: true, name: "IX_TIPOS_PROPAGANDA_DESCRICAO");

            AddColumn("dbo.Ocorrencias", "TipoPropagandaId", c => c.Int(false));
            CreateIndex("dbo.Ocorrencias", "TipoPropagandaId");
            AddForeignKey("dbo.Ocorrencias", "TipoPropagandaId", "dbo.TiposPropaganda", "TipoPropagandaId");
        }

        public override void Down()
        {
            DropForeignKey("dbo.Ocorrencias", "TipoPropagandaId", "dbo.TiposPropaganda");
            DropIndex("dbo.TiposPropaganda", "IX_TIPOS_PROPAGANDA_DESCRICAO");
            DropIndex("dbo.Ocorrencias", new[] {"TipoPropagandaId"});
            DropColumn("dbo.Ocorrencias", "TipoPropagandaId");
            DropTable("dbo.TiposPropaganda");
        }
    }
}