namespace TickTaskDoe.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TTD6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "FirstName", c => c.String(maxLength: 256));
            AddColumn("dbo.AspNetUsers", "LastName", c => c.String(maxLength: 256));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "FirstName");
            DropColumn("dbo.AspNetUsers", "LastName");
        }
    }
}
