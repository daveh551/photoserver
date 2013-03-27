using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoServer.Domain
{
	public class Event : IntEntity
	{
		public string EventName { get; set; }
		public DateTime? RaceDate { get; set; }
		public string Location { get; set; }
	}
}
