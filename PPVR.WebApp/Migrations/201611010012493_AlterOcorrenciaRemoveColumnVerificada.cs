namespace PPVR.WebApp.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlterOcorrenciaRemoveColumnVerificada : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Ocorrencias", "Verificada");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Ocorrencias", "Verificada", c => c.Boolean(nullable: false));
        }
    }
}
