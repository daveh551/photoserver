using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoServer.DataAccessLayer
{
	public interface IDataSource : IDisposable
	{
		int SaveChanges();
		void Update(object item);
	}
}
