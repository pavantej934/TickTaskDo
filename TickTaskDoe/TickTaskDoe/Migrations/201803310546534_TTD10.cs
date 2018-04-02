namespace TickTaskDoe.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TTD10 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.NationalityGreetings", "DisplayName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.NationalityGreetings", "DisplayName", c => c.String());
        }
    }
}
