namespace PPVR.WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterCandidatosAddNomeUrna : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Candidatos", "NomeUrna", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Candidatos", "NomeUrna");
        }
    }
}
