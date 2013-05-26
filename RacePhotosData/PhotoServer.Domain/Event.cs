using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoServer.Domain
{
	public class Event : IntEntity
	{
		[MaxLength(150)]
		public string EventName { get; set; }
		public DateTime? RaceDate { get; set; }
		[MaxLength(150)]
		public string Location { get; set; }
	}
}
