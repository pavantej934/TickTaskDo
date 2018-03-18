namespace TickTaskDoe.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TTD8 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ToDoes", newName: "ToDoTasks");
            AddColumn("dbo.ToDoLists", "Desc", c => c.String());
            CreateIndex("dbo.ToDoTasks", "ListId");
            AddForeignKey("dbo.ToDoTasks", "ListId", "dbo.ToDoLists", "Id", cascadeDelete: true);
            DropColumn("dbo.ToDoLists", "ListName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ToDoLists", "ListName", c => c.String());
            DropForeignKey("dbo.ToDoTasks", "ListId", "dbo.ToDoLists");
            DropIndex("dbo.ToDoTasks", new[] { "ListId" });
            DropColumn("dbo.ToDoLists", "Desc");
            RenameTable(name: "dbo.ToDoTasks", newName: "ToDoes");
        }
    }
}
