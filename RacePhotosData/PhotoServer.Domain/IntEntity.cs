using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoServer.Domain
{
	public abstract class IntEntity : IEntity<int>
	{
		public int Id { get; set; }
	}
}
