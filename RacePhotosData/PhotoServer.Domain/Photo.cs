using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoServer.Domain
{
    public class Photo : GuidEntity
    {
	    public const string DEFAULTSTATION = "FinishLine";

	    public virtual string  Race { get; set; }
	    public virtual string Station { get; set; }
	    public virtual string Card { get; set; }
	    public virtual int Sequence { get; set; }
        public virtual string Path { get; set; }
	    public virtual DateTime? TimeStamp { get; set; }
	    public virtual int? Hres { get; set; }
	    public virtual int? Vres { get; set; }
	    public Guid? BasedOn { get; set; }
	    public long FileSize { get; set; }
	    public DateTime? LastAccessed { get; set; }

	    public Photo(string race, string station, string card, int seq)
	    {

		    Race = race;
		    Station = string.IsNullOrWhiteSpace(station) ? DEFAULTSTATION : station;
		    Card = string.IsNullOrWhiteSpace(card) ? "1" : card;
		    Sequence = seq;

		    Path = string.Format("{0}/{1}/{2}/{3:000}.jpg", Race, Station, Card, Sequence);


	    }

	    public Photo()
	    {
		    
	    }
    }
}
