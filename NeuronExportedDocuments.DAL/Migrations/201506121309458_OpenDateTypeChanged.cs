namespace NeuronExportedDocuments.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OpenDateTypeChanged : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ServiceDocuments", "OpenDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ServiceDocuments", "OpenDate", c => c.DateTime(nullable: false));
        }
    }
}
