using System.Data.Entity.Migrations;

namespace PPVR.WebApp.Migrations
{
    public partial class AlterOcorrenciaCandidatoAndEnderecoFromRequiredToOptional : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Ocorrencias", new[] {"CandidatoId"});
            DropIndex("dbo.Ocorrencias", new[] {"EnderecoId"});
            AlterColumn("dbo.Ocorrencias", "CandidatoId", c => c.Int());
            AlterColumn("dbo.Ocorrencias", "EnderecoId", c => c.Long());
            CreateIndex("dbo.Ocorrencias", "CandidatoId");
            CreateIndex("dbo.Ocorrencias", "EnderecoId");
        }

        public override void Down()
        {
            DropIndex("dbo.Ocorrencias", new[] {"EnderecoId"});
            DropIndex("dbo.Ocorrencias", new[] {"CandidatoId"});
            AlterColumn("dbo.Ocorrencias", "EnderecoId", c => c.Long(false));
            AlterColumn("dbo.Ocorrencias", "CandidatoId", c => c.Int(false));
            CreateIndex("dbo.Ocorrencias", "EnderecoId");
            CreateIndex("dbo.Ocorrencias", "CandidatoId");
        }
    }
}