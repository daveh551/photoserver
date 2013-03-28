using PhotoServer.Domain;

namespace PhotoServer.DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<PhotoServer.DataAccessLayer.EFPhotoServerDataSource>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }


        protected override void Seed(PhotoServer.DataAccessLayer.EFPhotoServerDataSource context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
			context.DistanceDataSet.AddOrUpdate(d => d.Id,
				new Distance {Id=1,RaceDistance = "1 Mile" },
				new Distance {Id=2,RaceDistance = "2 Mile" },
				new Distance {Id=3,RaceDistance = "5K" },
				new Distance {Id=4,RaceDistance = "5 Mile" },
				new Distance {Id=5,RaceDistance = "10K" },
				new Distance {Id=6,RaceDistance = "8 Mile" },
				new Distance {Id=7,RaceDistance = "15K" },
				new Distance {Id=8,RaceDistance = "Half Marathon" },
				new Distance {Id=9,RaceDistance = "Marathon"}
				);
			context.EventDataSet.AddOrUpdate(e => e.Id,
				new Event {EventName = "Run For the Next Generation", Id=1, Location = "Denton, TX", RaceDate = new DateTime(2011,10,22)});

			context.RaceDataSet.AddOrUpdate( r => r.Id,
				new Race() { Id = 1, EventId = 1, DistanceId = 3});
        }
    }
}
