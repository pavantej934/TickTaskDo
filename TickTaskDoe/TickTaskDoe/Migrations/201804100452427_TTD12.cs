namespace TickTaskDoe.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TTD12 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ToDoTasks", "EmailNotification", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ToDoTasks", "EmailNotification");
        }
    }
}
