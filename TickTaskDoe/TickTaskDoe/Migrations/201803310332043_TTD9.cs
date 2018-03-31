namespace TickTaskDoe.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TTD9 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NationalityGreetings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nation = c.String(),
                        DisplayName = c.String(),
                        Greeting = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.AspNetUsers", "NationId", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "NationId");
            AddForeignKey("dbo.AspNetUsers", "NationId", "dbo.NationalityGreetings", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "NationId", "dbo.NationalityGreetings");
            DropIndex("dbo.AspNetUsers", new[] { "NationId" });
            DropColumn("dbo.AspNetUsers", "NationId");
            DropTable("dbo.NationalityGreetings");
        }
    }
}
