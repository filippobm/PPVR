namespace PPVR.WebApp.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlterOcorrenciaFotoToFotoPathAndChangeTypeFromImageToString : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ocorrencias", "FotoPath", c => c.String(nullable: false, maxLength: 255));
            DropColumn("dbo.Ocorrencias", "Foto");
        }

        public override void Down()
        {
            AddColumn("dbo.Ocorrencias", "Foto", c => c.Binary(nullable: false, storeType: "image"));
            DropColumn("dbo.Ocorrencias", "FotoPath");
        }
    }
}
