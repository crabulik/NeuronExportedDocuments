namespace NeuronExportedDocuments.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SetRangesForDocument : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ServiceDocuments", "PublishId", c => c.String(maxLength: 12));
            AlterColumn("dbo.ServiceDocuments", "PublishPassword", c => c.String(maxLength: 8));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ServiceDocuments", "PublishPassword", c => c.String());
            AlterColumn("dbo.ServiceDocuments", "PublishId", c => c.String());
        }
    }
}
