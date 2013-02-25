using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhotoServer;

namespace PhotoServer_Tests.Support
{
	public class Bootstrap : WebApiApplication
	{
		public void Startup()
		{
			base.Application_Start();
		}
	}
}
