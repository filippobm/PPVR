namespace PPVR.WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOcorrenciaTipoMatchToTableOcorrencias : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ocorrencias", "OcorrenciaTipoMatch", c => c.Byte(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Ocorrencias", "OcorrenciaTipoMatch");
        }
    }
}
