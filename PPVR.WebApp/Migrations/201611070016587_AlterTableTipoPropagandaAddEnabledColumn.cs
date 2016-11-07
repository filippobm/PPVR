namespace PPVR.WebApp.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlterTableTipoPropagandaAddEnabledColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TiposPropaganda", "Enabled", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TiposPropaganda", "Enabled");
        }
    }
}
