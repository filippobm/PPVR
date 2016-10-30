namespace PPVR.WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterEnderecosLatitudeLongitudeNotRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Enderecos", "Latitude", c => c.Double(nullable: false));
            AlterColumn("dbo.Enderecos", "Longitude", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Enderecos", "Longitude", c => c.Double());
            AlterColumn("dbo.Enderecos", "Latitude", c => c.Double());
        }
    }
}
