namespace PhotoServer.DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Photos",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        RaceId = c.Int(nullable: false),
                        Station = c.String(),
                        Card = c.String(),
                        Sequence = c.Int(nullable: false),
                        Path = c.String(),
                        TimeStamp = c.DateTime(),
                        Hres = c.Int(),
                        Vres = c.Int(),
                        BasedOn = c.Guid(),
                        FileSize = c.Long(nullable: false),
                        LastAccessed = c.DateTime(),
                        Server = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Races", t => t.RaceId, cascadeDelete: true)
                .Index(t => t.RaceId);
            
            CreateTable(
                "dbo.Races",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EventId = c.Int(nullable: false),
                        DistanceId = c.Int(nullable: false),
                        ReferenceTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Events", t => t.EventId, cascadeDelete: true)
                .ForeignKey("dbo.Distances", t => t.DistanceId, cascadeDelete: true)
                .Index(t => t.EventId)
                .Index(t => t.DistanceId);
            
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EventName = c.String(),
                        RaceDate = c.DateTime(),
                        Location = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Distances",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RaceDistance = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Races", new[] { "DistanceId" });
            DropIndex("dbo.Races", new[] { "EventId" });
            DropIndex("dbo.Photos", new[] { "RaceId" });
            DropForeignKey("dbo.Races", "DistanceId", "dbo.Distances");
            DropForeignKey("dbo.Races", "EventId", "dbo.Events");
            DropForeignKey("dbo.Photos", "RaceId", "dbo.Races");
            DropTable("dbo.Distances");
            DropTable("dbo.Events");
            DropTable("dbo.Races");
            DropTable("dbo.Photos");
        }
    }
}
