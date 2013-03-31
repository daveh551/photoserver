using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using Elmah;
using WebMatrix.WebData;

namespace PhotoServer.Modules
{
	public class HttpAuthenticationModule : IHttpModule
	{
		
		public void Dispose()
		{

		}

		public void Init(HttpApplication context)
		{
			var provider = Membership.Provider;
			if (!WebSecurity.Initialized)
				WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "UserName", false );
			
			context.AuthenticateRequest += OnAuthenticateRequest;
		}

		private void OnAuthenticateRequest(object sender, EventArgs e)
		{
			var app = sender as HttpApplication;
			var credentials = app.Context.Request.Headers["Authorization"];
			if (string.IsNullOrEmpty(credentials)) return;
			//var userPassword = System.Convert.FromBase64String(credentials);
			//var userString = (new System.Text.UTF8Encoding()).GetString(userPassword);
			var encodedPassword = AuthenticationHeaderValue.Parse(credentials).Parameter;
			var userPassword = new System.Text.UTF8Encoding().GetString(System.Convert.FromBase64String(encodedPassword));
			var passwordParts = userPassword.Split(':');
			var userName = passwordParts[0];
			var password = passwordParts[1];

			if (!WebSecurity.Initialized)
				throw new System.ApplicationException("WebSecurity database became unitialized");
			if (Membership.Provider.ValidateUser(userName, password))
			{
				var identity = new BasicIdentity(userName);
				var roles = Roles.Provider.GetRolesForUser(userName);
				var principal = new GenericPrincipal(identity, roles);

				app.Context.User = principal;
				if (HttpContext.Current != null)
					HttpContext.Current.User = principal;

			}
			

		}
	}
}