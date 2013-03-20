using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoServer.Domain
{
	class Race : IntEntity
	{
		public string RaceName { get; set; }
		public DateTime? RaceDate { get; set; }
		public string Location { get; set; }
		public IEnumerable<string> Distance { get; set; }
	}
}
