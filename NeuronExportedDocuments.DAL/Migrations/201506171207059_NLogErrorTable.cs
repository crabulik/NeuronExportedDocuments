namespace NeuronExportedDocuments.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NLogErrorTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NLogErrors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RecordTime = c.DateTime(nullable: false),
                        Host = c.String(nullable: false),
                        Type = c.String(nullable: false, maxLength: 50),
                        Source = c.String(nullable: false, maxLength: 50),
                        Message = c.String(nullable: false),
                        Level = c.String(nullable: false, maxLength: 50),
                        Logger = c.String(nullable: false, maxLength: 50),
                        Stacktrace = c.String(nullable: false),
                        AllXml = c.String(nullable: false, storeType: "ntext"),
                        TimeStamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.NLogErrors");
        }
    }
}
