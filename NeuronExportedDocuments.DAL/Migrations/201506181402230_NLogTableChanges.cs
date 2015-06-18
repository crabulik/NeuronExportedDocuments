namespace NeuronExportedDocuments.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NLogTableChanges : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.NLogErrors", "Source", c => c.String(nullable: false));
            AlterColumn("dbo.NLogErrors", "Logger", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.NLogErrors", "Logger", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.NLogErrors", "Source", c => c.String(nullable: false, maxLength: 50));
        }
    }
}
