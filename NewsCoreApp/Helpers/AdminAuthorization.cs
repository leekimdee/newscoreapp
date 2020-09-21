using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsCoreApp.Helpers
{
    public class AdminAuthorization : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var routeData = context.RouteData;
            var area = routeData.Values["area"];
            var user = context.HttpContext.User;
            if (area != null && area.ToString() == "Admin")
            {
                if(!user.Identity.IsAuthenticated)
                    context.Result = new RedirectResult("~/Admin/Login");
                return;
            }
        }
    }
}
