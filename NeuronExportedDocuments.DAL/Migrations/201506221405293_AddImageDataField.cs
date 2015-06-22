namespace NeuronExportedDocuments.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddImageDataField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ServiceDocuments", "ImageData", c => c.Binary());
            AddColumn("dbo.ServiceDocuments", "IsImagesInZip", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ServiceDocuments", "IsImagesInZip");
            DropColumn("dbo.ServiceDocuments", "ImageData");
        }
    }
}
