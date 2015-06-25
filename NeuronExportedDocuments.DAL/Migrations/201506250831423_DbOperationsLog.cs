namespace NeuronExportedDocuments.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DbOperationsLog : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DocumentLogOperations",
                c => new
                    {
                        DocumentLogOperationId = c.Int(nullable: false, identity: true),
                        ServiceDocumentId = c.Int(nullable: false),
                        OperationType = c.Int(nullable: false),
                        OperationDate = c.DateTime(nullable: false),
                        ConnectionIpAddress = c.String(nullable: false),
                        Info = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.DocumentLogOperationId)
                .ForeignKey("dbo.ServiceDocuments", t => t.ServiceDocumentId, cascadeDelete: true)
                .Index(t => t.ServiceDocumentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DocumentLogOperations", "ServiceDocumentId", "dbo.ServiceDocuments");
            DropIndex("dbo.DocumentLogOperations", new[] { "ServiceDocumentId" });
            DropTable("dbo.DocumentLogOperations");
        }
    }
}
