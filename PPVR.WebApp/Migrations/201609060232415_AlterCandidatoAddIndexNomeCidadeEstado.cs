namespace PPVR.WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterCandidatoAddIndexNomeCidadeEstado : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Candidatos", "IX_CANDIDATO_NUMERO_ELEITORAL");
            CreateIndex("dbo.Candidatos", "Nome", name: "IX_CANDIDATO_NOME");
            CreateIndex("dbo.Candidatos", "NumeroEleitoral", unique: true, name: "IX_CANDIDATO_NUMERO_ELEITORAL");
            CreateIndex("dbo.Candidatos", "Estado", name: "IX_CANDIDATO_ESTADO");
            CreateIndex("dbo.Candidatos", "Cidade", name: "IX_CANDIDATO_CIDADE");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Candidatos", "IX_CANDIDATO_CIDADE");
            DropIndex("dbo.Candidatos", "IX_CANDIDATO_ESTADO");
            DropIndex("dbo.Candidatos", "IX_CANDIDATO_NUMERO_ELEITORAL");
            DropIndex("dbo.Candidatos", "IX_CANDIDATO_NOME");
            CreateIndex("dbo.Candidatos", "NumeroEleitoral", unique: true, name: "IX_CANDIDATO_NUMERO_ELEITORAL");
        }
    }
}
