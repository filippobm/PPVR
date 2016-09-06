namespace PPVR.WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterCandidatoAddCidadeAddEstado : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Candidatos", "Estado", c => c.String(nullable: false, maxLength: 2, fixedLength: true, unicode: false));
            AddColumn("dbo.Candidatos", "Cidade", c => c.String(nullable: false, maxLength: 60));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Candidatos", "Cidade");
            DropColumn("dbo.Candidatos", "Estado");
        }
    }
}
