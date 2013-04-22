using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoServer.Domain
{
	public class Race : IntEntity
	{
		public int EventId { get; set; }
		public Event Event { get; set; }
		public int DistanceId { get; set; }
		public Distance Distance { get; set; }
		public override	string ToString(){ return string.Format("{0}.{1}", Event.EventName, Distance.RaceDistance); }
		public DateTime? ReferenceTime { get; set; }
	}
}
