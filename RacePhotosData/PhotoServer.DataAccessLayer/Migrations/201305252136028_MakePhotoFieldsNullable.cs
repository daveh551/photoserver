namespace PhotoServer.DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakePhotoFieldsNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Photos", "Sequence", c => c.Int());
            AlterColumn("dbo.Photos", "ISOspeed", c => c.Short());
            AlterColumn("dbo.Photos", "FocalLength", c => c.Short());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Photos", "FocalLength", c => c.Short(nullable: false));
            AlterColumn("dbo.Photos", "ISOspeed", c => c.Short(nullable: false));
            AlterColumn("dbo.Photos", "Sequence", c => c.Int(nullable: false));
        }
    }
}
