namespace PhotoServer.DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPhotographerRecord : DbMigration
    {
        public override void Up()
        {
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
            
            AddColumn("dbo.Photos", "FStop", c => c.String(maxLength: 10));
            AddColumn("dbo.Photos", "ExposureTime", c => c.String(maxLength: 10));
            AddColumn("dbo.Photos", "ISOspeed", c => c.String(maxLength: 10));
            AddColumn("dbo.Photos", "CreatedBy", c => c.String(maxLength: 50));
            AddColumn("dbo.Photos", "CreatedDate", c => c.DateTime());
            AddColumn("dbo.Photos", "Photographer_Id", c => c.Int());
            AlterColumn("dbo.Photos", "Station", c => c.String(maxLength: 50));
            AlterColumn("dbo.Photos", "Card", c => c.String(maxLength: 10));
            AlterColumn("dbo.Photos", "Path", c => c.String(maxLength: 512));
            AlterColumn("dbo.Photos", "Server", c => c.String(maxLength: 100));
            AlterColumn("dbo.Events", "EventName", c => c.String(maxLength: 150));
            AlterColumn("dbo.Events", "Location", c => c.String(maxLength: 150));
            AlterColumn("dbo.Distances", "RaceDistance", c => c.String(maxLength: 30));
            AddForeignKey("dbo.Photos", "Photographer_Id", "dbo.Photographers", "Id");
            CreateIndex("dbo.Photos", "Photographer_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Photos", new[] { "Photographer_Id" });
            DropForeignKey("dbo.Photos", "Photographer_Id", "dbo.Photographers");
            AlterColumn("dbo.Distances", "RaceDistance", c => c.String());
            AlterColumn("dbo.Events", "Location", c => c.String());
            AlterColumn("dbo.Events", "EventName", c => c.String());
            AlterColumn("dbo.Photos", "Server", c => c.String());
            AlterColumn("dbo.Photos", "Path", c => c.String());
            AlterColumn("dbo.Photos", "Card", c => c.String());
            AlterColumn("dbo.Photos", "Station", c => c.String());
            DropColumn("dbo.Photos", "Photographer_Id");
            DropColumn("dbo.Photos", "CreatedDate");
            DropColumn("dbo.Photos", "CreatedBy");
            DropColumn("dbo.Photos", "ISOspeed");
            DropColumn("dbo.Photos", "ExposureTime");
            DropColumn("dbo.Photos", "FStop");
            DropTable("dbo.Photographers");
        }
    }
}
