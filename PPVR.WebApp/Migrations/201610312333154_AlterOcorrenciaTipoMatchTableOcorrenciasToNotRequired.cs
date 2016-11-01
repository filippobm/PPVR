namespace PPVR.WebApp.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlterOcorrenciaTipoMatchTableOcorrenciasToNotRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Ocorrencias", "OcorrenciaTipoMatch", c => c.Byte());
        }

        public override void Down()
        {
            AlterColumn("dbo.Ocorrencias", "OcorrenciaTipoMatch", c => c.Byte(nullable: false));
        }
    }
}
