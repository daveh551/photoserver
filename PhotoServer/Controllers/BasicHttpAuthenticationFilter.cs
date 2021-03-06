﻿
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
namespace PhotoServer.Controllers
{


/// <summary>
/// HTTP authentication filter for ASP.NET Web API
/// </summary>
public abstract class BasicHttpAuthorizeAttribute : AuthorizeAttribute
{
    private const string BasicAuthResponseHeader = "WWW-Authenticate";
    private const string BasicAuthResponseHeaderValue = "Basic";
 
    public override void OnAuthorization(HttpActionContext actionContext)
    {
        if (actionContext == null)
            throw new ArgumentNullException("actionContext");
        if (AuthorizationDisabled(actionContext)
            || AuthorizeRequest(actionContext.ControllerContext.Request))
            return;
        this.HandleUnauthorizedRequest(actionContext);
    }
 
    protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
    {
        if (actionContext == null)
            throw new ArgumentNullException("actionContext");
        actionContext.Response = CreateUnauthorizedResponse(actionContext
            .ControllerContext.Request);
    }
 
    private HttpResponseMessage CreateUnauthorizedResponse(HttpRequestMessage request)
    {
        var result = new HttpResponseMessage()
        {
            StatusCode = HttpStatusCode.Unauthorized,
            RequestMessage = request
        };
 
        //we need to include WWW-Authenticate header in our response,
        //so our client knows we are using HTTP authentication
        result.Headers.Add(BasicAuthResponseHeader, BasicAuthResponseHeaderValue);
        return result;
    }
 
    private static bool AuthorizationDisabled(HttpActionContext actionContext)
    {
        //support new AllowAnonymousAttribute
        if (!actionContext.ActionDescriptor
            .GetCustomAttributes<AllowAnonymousAttribute>().Any())
            return actionContext.ControllerContext
                .ControllerDescriptor
                .GetCustomAttributes<AllowAnonymousAttribute>().Any();
        else
            return true;
    }
 
    private bool AuthorizeRequest(HttpRequestMessage request)
    {
        AuthenticationHeaderValue authValue = request.Headers.Authorization;
        if (authValue == null || string.IsNullOrWhiteSpace(authValue.Parameter)
            || string.IsNullOrWhiteSpace(authValue.Scheme)
            || authValue.Scheme != BasicAuthResponseHeaderValue)
        {
            return false;
        }
 
        string[] parsedHeader = ParseAuthorizationHeader(authValue.Parameter);
        if (parsedHeader == null)
        {
            return false;
        }
        IPrincipal principal = null;
        if (TryCreatePrincipal(parsedHeader[0], parsedHeader[1], out principal))
        {
            HttpContext.Current.User = principal;
            return CheckRoles(principal) && CheckUsers(principal);
        }
        else
        {
            return false;
        }
    }
 
    private bool CheckUsers(IPrincipal principal)
    {
        string[] users = UsersSplit;
        if (users.Length == 0) return true;
        //NOTE: This is a case sensitive comparison
        return users.Any(u=>principal.Identity.Name == u);
    }
 
    private bool CheckRoles(IPrincipal principal)
    {
        string[] roles = RolesSplit;
        if(roles.Length == 0) return true;
        return roles.Any(principal.IsInRole);
    }
 
    private string[] ParseAuthorizationHeader(string authHeader)
    {
        string[] credentials = Encoding.ASCII.GetString(Convert
                                                        .FromBase64String(authHeader))
                                                        .Split(
                                                        new[] { ':' });
        if (credentials.Length != 2 || string.IsNullOrEmpty(credentials[0])
            || string.IsNullOrEmpty(credentials[1])) return null;
        return credentials;
    }
 
    protected string[] RolesSplit
    {
        get { return SplitStrings(Roles); }
    }
 
    protected string[] UsersSplit
    {
        get { return SplitStrings(Users); }
    }
 
    protected static string[] SplitStrings(string input)
    {
        if(string.IsNullOrWhiteSpace(input)) return new string[0];
        var result = input.Split(',')
            .Where(s=>!string.IsNullOrWhiteSpace(s.Trim()));
        return result.Select(s => s.Trim()).ToArray();
    }
 
    /// <summary>
    /// Implement to include authentication logic and create IPrincipal
    /// </summary>
    protected abstract bool TryCreatePrincipal(string user, string password,
        out IPrincipal principal);
}
}