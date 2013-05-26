namespace PhotoServer.DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddShortFieldsToPhotos : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Photos", "ISOspeed", c => c.Short(nullable: false));
            AddColumn("dbo.Photos", "FocalLength", c => c.Short(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Photos", "FocalLength");
            DropColumn("dbo.Photos", "ISOspeed");
        }
    }
}
