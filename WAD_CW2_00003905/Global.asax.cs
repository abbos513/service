using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace WAD_CW2_00003905
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //AreaRegistration.RegisterAllAreas();
            //RouteConfig.RegisterRoutes(RouteTable.Routes);

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        public void Application_AcquireRequestState()
        {
            try
            {
                HttpCookie authCookie = Request.Cookies.Get("UserLoginData");
                if (authCookie != null && !string.IsNullOrEmpty(authCookie.Value) && FormsAuthentication.Decrypt(authCookie.Value) != null)
                {
                    var ticket = FormsAuthentication.Decrypt(authCookie.Value);
                    var userName = ticket.Name;
                    Session["User"] = userName;
                }
                else
                {
                    Session["User"] = null;
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
