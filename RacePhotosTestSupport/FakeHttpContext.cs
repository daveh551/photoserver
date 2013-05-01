using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace RacePhotosTestSupport
{
	public class FakeHttpContext : HttpContextBase
	{
		public override HttpServerUtilityBase Server
		{
			get { return new FakeHttpServerUtility(); }
		}
	}

	public class FakeHttpServerUtility : HttpServerUtilityBase
	{
		public override string MapPath(string path)
		{
			string returnPath;
			if (path.StartsWith("~/"))
			{
				returnPath = Path.Combine(@"..\..\..\PhotoServer", path.Substring((2)));
				return returnPath;
			}
			else
			{
				throw new ArgumentException(string.Format("path {0} does not start with \"~/\"", path));
			}
		}
	}
}
