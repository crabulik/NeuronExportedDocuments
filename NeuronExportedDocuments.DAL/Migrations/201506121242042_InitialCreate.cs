namespace NeuronExportedDocuments.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DocumentImages",
                c => new
                    {
                        DocumentImageId = c.Int(nullable: false, identity: true),
                        ServiceDocumentId = c.Int(nullable: false),
                        FileName = c.String(),
                        MimeType = c.String(),
                        ImageData = c.Binary(),
                    })
                .PrimaryKey(t => t.DocumentImageId)
                .ForeignKey("dbo.ServiceDocuments", t => t.ServiceDocumentId, cascadeDelete: true)
                .Index(t => t.ServiceDocumentId);
            
            CreateTable(
                "dbo.ServiceDocuments",
                c => new
                    {
                        ServiceDocumentId = c.Int(nullable: false, identity: true),
                        NeuronDbDocumentId = c.Int(nullable: false),
                        Name = c.String(),
                        DeliveryEMail = c.String(),
                        DeliveryPhone = c.String(),
                        CreatDate = c.DateTime(nullable: false),
                        PdfFileData = c.Binary(),
                        IsBlocked = c.Boolean(nullable: false),
                        FailedTimes = c.Int(nullable: false),
                        IsOpened = c.Boolean(nullable: false),
                        OpenDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ServiceDocumentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DocumentImages", "ServiceDocumentId", "dbo.ServiceDocuments");
            DropIndex("dbo.DocumentImages", new[] { "ServiceDocumentId" });
            DropTable("dbo.ServiceDocuments");
            DropTable("dbo.DocumentImages");
        }
    }
}
