using Microsoft.AspNetCore.Mvc;
using NewsCoreApp.Application;
using NewsCoreApp.Data.Entities;
using NewsCoreApp.Extensions;
using NewsCoreApp.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NewsCoreApp.Areas.Admin.Components
{
    public class SideBarViewComponent : ViewComponent
    {
        private FunctionService _functionService;

        public SideBarViewComponent()
        {
            _functionService = new FunctionService();
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var roles = ((ClaimsPrincipal)User).GetSpecificClaim("Roles");
            List<Function> functions;
            if (roles.Split(";").Contains(CommonConstants.AdminRole))
            {
                functions = await _functionService.GetAll();
            }
            else
            {
                //TODO: Get by permission
                functions = new List<Function>();
            }
            return View(functions);
        }
    }
}
