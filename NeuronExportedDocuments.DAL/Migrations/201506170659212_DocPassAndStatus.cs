using NeuronExportedDocuments.Models.Enums;

namespace NeuronExportedDocuments.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DocPassAndStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ServiceDocuments", "Status", c => c.Int(nullable: false));
            AddColumn("dbo.ServiceDocuments", "PublishId", c => c.String());
            AddColumn("dbo.ServiceDocuments", "PublishPassword", c => c.String());

            Sql("UPDATE [dbo].[ServiceDocuments] SET Status = 1 ");
        }
        
        public override void Down()
        {
            DropColumn("dbo.ServiceDocuments", "PublishPassword");
            DropColumn("dbo.ServiceDocuments", "PublishId");
            DropColumn("dbo.ServiceDocuments", "Status");
        }
    }
}
