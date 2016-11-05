namespace TetrisRoyal.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Improvments : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Games", "HostId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Games", "ChallengerId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Games", "HostId");
            CreateIndex("dbo.Games", "ChallengerId");
            AddForeignKey("dbo.Games", "ChallengerId", "dbo.Players", "ConnectionId");
            AddForeignKey("dbo.Games", "HostId", "dbo.Players", "ConnectionId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Games", "HostId", "dbo.Players");
            DropForeignKey("dbo.Games", "ChallengerId", "dbo.Players");
            DropIndex("dbo.Games", new[] { "ChallengerId" });
            DropIndex("dbo.Games", new[] { "HostId" });
            AlterColumn("dbo.Games", "ChallengerId", c => c.String());
            AlterColumn("dbo.Games", "HostId", c => c.String());
        }
    }
}
