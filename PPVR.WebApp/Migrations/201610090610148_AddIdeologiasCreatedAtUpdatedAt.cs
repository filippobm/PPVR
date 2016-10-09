namespace PPVR.WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIdeologiasCreatedAtUpdatedAt : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ideologias", "CreatedAt", c => c.DateTime(nullable: false));
            AddColumn("dbo.Ideologias", "UpdatedAt", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Ideologias", "UpdatedAt");
            DropColumn("dbo.Ideologias", "CreatedAt");
        }
    }
}
