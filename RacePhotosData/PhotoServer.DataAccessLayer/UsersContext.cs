using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhotoServer.Domain;

namespace PhotoServer.DataAccessLayer
{
	public class UsersContext : DbContext
	{
			public UsersContext()
			: base("DefaultConnection")
		{
		}

		public DbSet<UserProfile> UserProfiles { get; set; }
	}
}
