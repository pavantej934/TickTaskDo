namespace TickTaskDoe.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TTD11 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ToDoTasks", "DueDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ToDoTasks", "DueDate");
        }
    }
}
