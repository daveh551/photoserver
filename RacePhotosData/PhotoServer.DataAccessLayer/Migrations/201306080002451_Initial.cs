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
                        EventId = c.Int(nullable: false),
                        Station = c.String(maxLength: 50),
                        Card = c.String(maxLength: 10),
                        Sequence = c.Int(),
                        Path = c.String(maxLength: 512),
                        TimeStamp = c.DateTime(),
                        Hres = c.Int(),
                        Vres = c.Int(),
                        FStop = c.String(maxLength: 10),
                        ShutterSpeed = c.String(maxLength: 10),
                        ISOspeed = c.Short(),
                        FocalLength = c.Short(),
                        BasedOn = c.Guid(),
                        FileSize = c.Long(nullable: false),
                        LastAccessed = c.DateTime(),
                        Server = c.String(maxLength: 100),
                        CreatedBy = c.String(maxLength: 50),
                        CreatedDate = c.DateTime(),
                        Photographer_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Events", t => t.EventId, cascadeDelete: true)
                .ForeignKey("dbo.Photographers", t => t.Photographer_Id)
                .Index(t => t.EventId)
                .Index(t => t.Photographer_Id);
            
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EventName = c.String(maxLength: 150),
                        RaceDate = c.DateTime(),
                        Location = c.String(maxLength: 150),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Photographers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 100),
                        Initials = c.String(maxLength: 6),
                        PhoneNumber = c.String(maxLength: 20),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Distances",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RaceDistance = c.String(maxLength: 30),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Photos", new[] { "Photographer_Id" });
            DropIndex("dbo.Photos", new[] { "EventId" });
            DropForeignKey("dbo.Photos", "Photographer_Id", "dbo.Photographers");
            DropForeignKey("dbo.Photos", "EventId", "dbo.Events");
            DropTable("dbo.Distances");
            DropTable("dbo.Photographers");
            DropTable("dbo.Events");
            DropTable("dbo.Photos");
        }
    }
}
