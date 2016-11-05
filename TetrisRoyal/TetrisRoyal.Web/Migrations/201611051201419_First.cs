namespace TetrisRoyal.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class First : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Games",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        HostId = c.String(),
                        ChallengerId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Players",
                c => new
                    {
                        ConnectionId = c.String(nullable: false, maxLength: 128),
                        Username = c.String(),
                    })
                .PrimaryKey(t => t.ConnectionId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Players");
            DropTable("dbo.Games");
        }
    }
}
