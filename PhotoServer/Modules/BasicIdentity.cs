using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace PhotoServer.Modules
{
	public class BasicIdentity : IIdentity

	{
		public string AuthenticationType
		{
			get { return "Basic"; }
		}

		private bool _isAuthenticated;

		public bool IsAuthenticated
		{
			get { return _isAuthenticated; }
		}

		private readonly string _name;

		public string Name
		{
			get { return _name; }
		}

		public BasicIdentity(string name)
		{
			_name = name;
			_isAuthenticated = true;
		}
	}
}