using CustomAuthenticationMVC.CustomAuthentication;
using MvcMusicStore.App_Start;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using MvcMusicStore.Models;

namespace MvcMusicStore
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            System.Data.Entity.Database.SetInitializer(
            new MvcMusicStore.Models.SampleData());
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            HttpCookie authCookie = Request.Cookies["Cookie1"];
            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

                var serializeModel = JsonConvert.DeserializeObject<CustomSerializeModel>(authTicket.UserData);
                CustomPrincipal principal = new CustomPrincipal(authTicket.Name);
                principal.UserId = serializeModel.UserId;
                /*principal.FirstName = serializeModel.FirstName;
                principal.LastName = serializeModel.LastName;*/
                principal.Roles = serializeModel.RoleName.ToArray<string>();

                HttpContext.Current.User = principal;
            }
        }
    }
}
