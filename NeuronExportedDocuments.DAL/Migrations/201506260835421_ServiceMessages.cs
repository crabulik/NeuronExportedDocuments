namespace NeuronExportedDocuments.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ServiceMessages : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ServiceMessages",
                c => new
                    {
                        ServiceMessageId = c.Int(nullable: false, identity: true),
                        Key = c.Int(nullable: false),
                        Message = c.String(),
                        IsDefault = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ServiceMessageId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ServiceMessages");
        }
    }
}
