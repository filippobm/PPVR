namespace PPVR.WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUpdatedAtCreatedAtToTableTiposPartido : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TiposPropaganda", "CreatedAt", c => c.DateTime(nullable: false));
            AddColumn("dbo.TiposPropaganda", "UpdatedAt", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TiposPropaganda", "UpdatedAt");
            DropColumn("dbo.TiposPropaganda", "CreatedAt");
        }
    }
}
