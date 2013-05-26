namespace PhotoServer.DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdjustPhotoFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Photos", "ShutterSpeed", c => c.String(maxLength: 10));
            DropColumn("dbo.Photos", "ExposureTime");
            DropColumn("dbo.Photos", "ISOspeed");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Photos", "ISOspeed", c => c.String(maxLength: 10));
            AddColumn("dbo.Photos", "ExposureTime", c => c.String(maxLength: 10));
            DropColumn("dbo.Photos", "ShutterSpeed");
        }
    }
}
