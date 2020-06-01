namespace MVCPresentationLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WorkerID : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "WorkerID", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "WorkerID");
        }
    }
}
