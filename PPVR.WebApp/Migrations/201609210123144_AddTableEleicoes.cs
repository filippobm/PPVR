namespace PPVR.WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTableEleicoes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Eleicoes",
                c => new
                    {
                        EleicaoId = c.Short(nullable: false, identity: true),
                        Ano = c.Byte(nullable: false),
                        Turno = c.Byte(nullable: false),
                        Descricao = c.String(nullable: false, maxLength: 60),
                        Enabled = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.EleicaoId)
                .Index(t => t.Ano, name: "IX_ELEICAO_ANO")
                .Index(t => t.Turno, name: "IX_ELEICAO_TURNO")
                .Index(t => t.Descricao, name: "IX_ELEICAO_DESCRICAO");
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Eleicoes", "IX_ELEICAO_DESCRICAO");
            DropIndex("dbo.Eleicoes", "IX_ELEICAO_TURNO");
            DropIndex("dbo.Eleicoes", "IX_ELEICAO_ANO");
            DropTable("dbo.Eleicoes");
        }
    }
}
